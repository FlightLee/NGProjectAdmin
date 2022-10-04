
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.BusinessRepository.RedisRepository;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.CodeTableRepository;
using NGProjectAdmin.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.CodeTableService
{
    /// <summary>
    /// 数据字典业务层实现
    /// </summary>
    class CodeTableService : NGAdminBaseService<SysCodeTable>, ICodeTableService
    {
        #region 属性及构造函数

        /// <summary>
        /// 数据字典仓储实例
        /// </summary>
        private readonly ICodeTableRepository codeTableRepository;

        /// <summary>
        /// Redis仓储实例
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="codeTableRepository"></param>
        public CodeTableService(ICodeTableRepository codeTableRepository,
                                IRedisRepository redisRepository) : base(codeTableRepository)
        {
            this.codeTableRepository = codeTableRepository;
            this.redisRepository = redisRepository;
        }

        #endregion

        #region 服务层公有方法

        #region 获取字典树

        /// <summary>
        /// 获取字典树
        /// </summary>
        /// <returns>ActionResult</returns>
        public async Task<QueryResult<SysCodeTableDTO>> GetCodeTreeNodes()
        {
            var result = new List<SysCodeTableDTO>();

            var codes = await this.redisRepository.GetAsync<List<SysCodeTableDTO>>(NGAdminGlobalContext.SystemCacheConfig.CodeTableCacheName);
            var parentCodes = codes.Where(t => t.ParentId == null).OrderBy(t => t.SerialNumber).ToList();
            foreach (var item in parentCodes)
            {
                this.GetNodeChildren(item, codes);
            }
            result.AddRange(parentCodes);

            var queryResult = new QueryResult<SysCodeTableDTO>();
            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            queryResult.List = result;

            return queryResult;
        }

        #endregion

        #region 加载数据字典缓存

        /// <summary>
        /// 加载数据字典缓存
        /// </summary>
        public async Task LoadSystemCodeTableCache()
        {
            var sqlKey = "sqls:sql:query_codetable_info";
            var strSQL = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value;

            int totalCount = 0;
            var codes = (await this.codeTableRepository.SqlQueryAsync<SysCodeTableDTO>(new QueryCondition(), totalCount, strSQL)).ToList();
            await this.redisRepository.SetAsync(NGAdminGlobalContext.SystemCacheConfig.CodeTableCacheName, codes, -1);

            NGLoggerContext.Info("系统数据字典缓存加载完成");
        }

        #endregion

        #region 清理数据字典缓存

        /// <summary>
        /// 清理数据字典缓存
        /// </summary>
        public async Task ClearSystemCodeTableCache()
        {
            await this.redisRepository.DeleteAsync(new String[] { NGAdminGlobalContext.SystemCacheConfig.CodeTableCacheName });

            NGLoggerContext.Info("系统数据字典缓存清理完成");
        }

        #endregion

        #endregion

        #region 服务层私有方法

        /// <summary>
        /// 递归树
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="codes">字典列表</param>
        private void GetNodeChildren(SysCodeTableDTO root, List<SysCodeTableDTO> codes)
        {
            var list = codes.Where(t => t.ParentId == root.Id).ToList();
            if (list.Count > 0)
            {
                root.Children = new List<SysCodeTableDTO>();
                root.Children.AddRange(list.OrderBy(t => t.SerialNumber).ToList());
                foreach (var item in list)
                {
                    this.GetNodeChildren(item, codes);
                }
            }
        }

        #endregion
    }
}
