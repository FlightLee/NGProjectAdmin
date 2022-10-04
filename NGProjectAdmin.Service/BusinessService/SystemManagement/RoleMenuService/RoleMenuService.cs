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
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.RoleMenuRepository;
using NGProjectAdmin.Service.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.RoleMenuService
{
    /// <summary>
    /// 角色菜单业务层实现
    /// </summary>
    class RoleMenuService : NGAdminBaseService<SysRoleMenu>, IRoleMenuService
    {
        #region 属性及构造函数

        /// <summary>
        /// 角色与菜单仓储实例
        /// </summary>
        private readonly IRoleMenuRepository roleMenuRepository;

        /// <summary>
        /// Redis仓储实例
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleMenuRepository"></param>
        public RoleMenuService(IRoleMenuRepository roleMenuRepository,
                               IRedisRepository redisRepository) : base(roleMenuRepository)
        {
            this.roleMenuRepository = roleMenuRepository;
            this.redisRepository = redisRepository;
        }

        #endregion

        #region 服务层公有方法

        #region 加载角色与菜单缓存

        /// <summary>
        /// 加载角色与菜单缓存
        /// </summary>
        public async Task LoadSystemRoleMenuCache()
        {
            var sqlKey = "sqls:sql:query_role_menu_info";
            var strSQL = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value;

            int totalCount = 0;
            var roleMenus = (await this.roleMenuRepository.SqlQueryAsync<SysRoleMenu>(new QueryCondition(), totalCount, strSQL)).ToList();
            await this.redisRepository.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleAndMenuCacheName, roleMenus, -1);

            NGLoggerContext.Info("系统角色与菜单缓存加载完成");
        }

        #endregion

        #region 清理角色与菜单缓存

        /// <summary>
        /// 清理角色与菜单缓存
        /// </summary>
        public async Task ClearSystemRoleMenuCache()
        {
            await this.redisRepository.DeleteAsync(new String[] { NGAdminGlobalContext.SystemCacheConfig.RoleAndMenuCacheName });

            NGLoggerContext.Info("系统角色与菜单缓存清理完成");
        }

        #endregion

        #endregion
    }
}
