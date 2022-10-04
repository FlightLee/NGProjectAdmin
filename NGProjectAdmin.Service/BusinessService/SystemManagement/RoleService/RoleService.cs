//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.BusinessRepository.RedisRepository;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.RoleRepository;
using NGProjectAdmin.Service.Base;
using System;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.RoleService
{
    /// <summary>
    /// 角色业务层实现
    /// </summary>
    class RoleService : NGAdminBaseService<SysRole>, IRoleService
    {
        #region 属性及构造函数

        /// <summary>
        /// 角色仓储实例
        /// </summary>
        private readonly IRoleRepository roleRepository;

        /// <summary>
        /// Redis仓储实例
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleRepository"></param>
        /// <param name="redisRepository"></param>
        public RoleService(IRoleRepository roleRepository,
                           IRedisRepository redisRepository) : base(roleRepository)
        {
            this.roleRepository = roleRepository;
            this.redisRepository = redisRepository;
        }

        #endregion

        #region 服务层公有方法

        #region 加载系统角色缓存

        /// <summary>
        /// 加载系统角色缓存
        /// </summary>
        public async Task LoadSystemRoleCache()
        {
            var sqlKey = "sqls:sql:query_role_info";
            var strSQL = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value;

            int totalCount = 0;
            var roles = await this.roleRepository.SqlQueryAsync<SysRoleDTO>(new QueryCondition(), totalCount, strSQL);
            await this.redisRepository.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleCacheName, roles, -1);

            NGLoggerContext.Info("系统角色缓存加载完成");
        }

        #endregion

        #region 清理系统角色缓存

        /// <summary>
        /// 清理系统角色缓存
        /// </summary>
        public async Task ClearSystemRoleCache()
        {
            await this.redisRepository.DeleteAsync(new String[] { NGAdminGlobalContext.SystemCacheConfig.RoleCacheName });

            NGLoggerContext.Info("系统菜单缓存清理完成");
        }

        #endregion

        #endregion
    }
}
