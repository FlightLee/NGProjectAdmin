//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Entity.CoreEnum;
using NGProjectAdmin.Repository.BusinessRepository.RedisRepository;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.OrganizationRepository;
using NGProjectAdmin.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.OrganizationService
{
    /// <summary>
    /// 机构业务层实现
    /// </summary>
    public class OrganizationService : NGAdminBaseService<SysOrganization>, IOrganizationService
    {
        #region 属性及构造函数

        /// <summary>
        ///机构仓储实例
        /// </summary>
        private readonly IOrganizationRepository organizationRepository;

        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// Redis仓储实例
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="organizationRepository"></param>
        public OrganizationService(IOrganizationRepository organizationRepository,
                                   IHttpContextAccessor context,
                                   IRedisRepository redisRepository) : base(organizationRepository)
        {
            this.organizationRepository = organizationRepository;
            this.context = context;
            this.redisRepository = redisRepository;
        }

        #endregion

        #region 业务层公有方法

        #region 获取机构树

        /// <summary>
        /// 获取机构树
        /// </summary>
        /// <returns>ActionResult</returns>
        public async Task<QueryResult<SysOrganizationDTO>> GetOrgTreeNodes()
        {
            var orgs = await this.redisRepository.GetAsync<List<SysOrganizationDTO>>(NGAdminGlobalContext.SystemCacheConfig.OrgCacheName);

            //获取用户机构编号
            var orgId = NGAdminSessionContext.GetUserOrgId(this.context);
            var root = orgs.Where(t => t.Id == orgId).FirstOrDefault();

            this.GetNodeChildren(root, orgs);

            var queryResult = new QueryResult<SysOrganizationDTO>();
            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            queryResult.List = new List<SysOrganizationDTO>();
            queryResult.List.Add(root);

            return queryResult;
        }

        #endregion

        #region 获取机构、用户树

        /// <summary>
        /// 获取机构、用户树
        /// </summary>
        /// <returns></returns>
        public async Task<QueryResult<OrgUserTreeDTO>> GetOrgUserTree()
        {
            return await Task.Run(() =>
            {
                var sqlKey = "sqls:sql:query_org_user_tree_org";
                var strSQL = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value;

                //获取用户机构编号
                var orgId = NGAdminSessionContext.GetUserOrgId(this.context);
                strSQL = String.Format(strSQL, orgId);

                int totalCount = 0;
                var root = this.organizationRepository.
                    SqlQuery<OrgUserTreeDTO>(new QueryCondition(), ref totalCount, strSQL).
                    FirstOrDefault();

                this.InitOrgUserTreeChildren(root, sqlKey);

                var queryResult = new QueryResult<OrgUserTreeDTO>();
                queryResult.HttpStatusCode = HttpStatusCode.OK;
                queryResult.Message = new String("OK");
                queryResult.List = new List<OrgUserTreeDTO>();
                queryResult.List.Add(root);

                return queryResult;
            });
        }

        #endregion

        #region 加载系统机构缓存

        /// <summary>
        /// 加载系统机构缓存
        /// </summary>
        public async Task LoadSystemOrgCache()
        {
            var sqlKey = "sqls:sql:query_org_info";
            var strSQL = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value;

            int totalCount = 0;
            var orgs = await this.organizationRepository.SqlQueryAsync<SysOrganizationDTO>(new QueryCondition(), totalCount, strSQL);
            await this.redisRepository.SetAsync(NGAdminGlobalContext.SystemCacheConfig.OrgCacheName, orgs, -1);

            NGLoggerContext.Info("系统机构缓存加载完成");
        }

        #endregion

        #region 清理系统机构缓存

        /// <summary>
        /// 清理系统机构缓存
        /// </summary>
        public async Task ClearSystemOrgCache()
        {
            await this.redisRepository.DeleteAsync(new String[] { NGAdminGlobalContext.SystemCacheConfig.OrgCacheName });

            NGLoggerContext.Info("系统机构缓存清理完成");
        }

        #endregion

        #endregion

        #region 业务层私有方法

        #region 递归机构树

        /// <summary>
        /// 递归机构树
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="orgs">机构列表</param>
        private void GetNodeChildren(SysOrganizationDTO root, List<SysOrganizationDTO> orgs)
        {
            var list = orgs.Where(t => t.ParentId == root.Id).ToList();

            if (list.Count > 0)
            {
                root.Children = new List<SysOrganizationDTO>();
                root.Children.AddRange(list.OrderBy(t => t.SerialNumber).ToList());

                foreach (var item in list)
                {
                    this.GetNodeChildren(item, orgs);
                }
            }
        }

        #endregion

        #region 递归机构、用户树

        /// <summary>
        /// 递归机构、用户树
        /// </summary>
        /// <param name="root"></param>
        /// <param name="sqlKey"></param>
        private void InitOrgUserTreeChildren(OrgUserTreeDTO root, String sqlKey)
        {
            #region 初始化节点下用户

            String strSQL = NGAdminGlobalContext.Configuration.GetSection("sqls:sql:query_org_user_tree_user").Value;

            strSQL = String.Format(strSQL, root.Id);
            int totalCount = 0;
            var users = this.organizationRepository.SqlQuery<OrgUserTreeDTO>(new QueryCondition(), ref totalCount, strSQL);

            if (users.Count > 0)
            {
                root.Children = new List<OrgUserTreeDTO>();
                root.Children.AddRange(users.OrderBy(t => t.SerialNumber).ToList());
            }

            #endregion

            #region 初始化节点下机构

            var queryCondition = new QueryCondition();
            queryCondition.QueryItems = new List<QueryItem>();
            queryCondition.QueryItems.Add(new QueryItem()
            {
                Field = "PARENT_ID",
                DataType = DataType.Guid,
                QueryMethod = QueryMethod.Equal,
                Value = root.Id
            });

            strSQL = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value.Replace("AND o.ID = '{0}'", "");

            totalCount = 0;
            var orgs = this.organizationRepository.SqlQuery<OrgUserTreeDTO>(queryCondition, ref totalCount, strSQL);

            if (orgs.Count() > 0)
            {
                if (root.Children == null)
                {
                    root.Children = new List<OrgUserTreeDTO>();
                }
                root.Children.AddRange(orgs.OrderBy(t => t.SerialNumber).ToList());
            }

            #endregion

            #region 递归初始化子集

            foreach (var item in orgs)
            {
                this.InitOrgUserTreeChildren(item, sqlKey);
            }

            #endregion
        }

        #endregion

        #endregion
    }
}
