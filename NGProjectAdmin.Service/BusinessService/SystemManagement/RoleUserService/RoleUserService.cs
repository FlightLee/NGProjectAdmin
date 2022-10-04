//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.BusinessRepository.RedisRepository;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.RoleUserRepository;
using NGProjectAdmin.Service.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.RoleUserService
{
    /// <summary>
    /// 角色用户业务层实现
    /// </summary>
    class RoleUserService : NGAdminBaseService<SysRoleUser>, IRoleUserService
    {
        #region 属性及构造函数

        /// <summary>
        /// 角色与用户仓储实例
        /// </summary>
        private readonly IRoleUserRepository roleUserRepository;

        /// <summary>
        /// Redis仓储实例
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleUserRepository"></param>
        /// <param name="redisRepository"></param>
        public RoleUserService(IRoleUserRepository roleUserRepository,
                               IRedisRepository redisRepository) : base(roleUserRepository)
        {
            this.roleUserRepository = roleUserRepository;
            this.redisRepository = redisRepository;
        }

        #endregion

        #region 服务层公有方法

        #region 加载角色与用户缓存

        /// <summary>
        /// 加载角色与用户缓存
        /// </summary>
        public async Task LoadSystemRoleUserCache()
        {
            var sqlKey = "sqls:sql:query_role_user_info";
            var strSQL = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value;

            int totalCount = 0;
            var roleUsers = (await this.roleUserRepository.SqlQueryAsync<SysRoleUser>(new QueryCondition(), totalCount, strSQL)).ToList();
            await this.redisRepository.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleAndUserCacheName, roleUsers, -1);

            NGLoggerContext.Info("系统角色与用户缓存加载完成");
        }

        #endregion

        #region 清理角色与用户缓存

        /// <summary>
        /// 清理角色与用户缓存
        /// </summary>
        public async Task ClearSystemRoleUserCache()
        {
            await this.redisRepository.DeleteAsync(new String[] { NGAdminGlobalContext.SystemCacheConfig.RoleAndUserCacheName });

            NGLoggerContext.Info("系统角色与用户缓存清理完成");
        }

        #endregion

        #endregion
    }
}
