using System;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// 系统缓存配置
    /// </summary>
    public class SystemCacheConfig
    {
        /// <summary>
        /// 机构缓存名称
        /// </summary>
        public String OrgCacheName { get; set; }

        /// <summary>
        /// 用户缓存名称
        /// </summary>
        public String UserCacheName { get; set; }

        /// <summary>
        /// 菜单缓存名称
        /// </summary>
        public String MenuCacheName { get; set; }

        /// <summary>
        /// 菜单与多语缓存名称
        /// </summary>
        public String MenuAndLanguageCacheName { get; set; }

        /// <summary>
        /// 角色缓存名称
        /// </summary>
        public String RoleCacheName { get; set; }

        /// <summary>
        /// 角色菜单缓存名称
        /// </summary>
        public String RoleAndMenuCacheName { get; set; }

        /// <summary>
        /// 角色机构缓存名称
        /// </summary>
        public String RoleAndOrgCacheName { get; set; }

        /// <summary>
        /// 角色用户缓存名称
        /// </summary>
        public String RoleAndUserCacheName { get; set; }

        /// <summary>
        /// 数据字典缓存名称
        /// </summary>
        public String CodeTableCacheName { get; set; }

        /// <summary>
        /// 多语缓存名称
        /// </summary>
        public String LanguageCacheName { get; set; }

        /// <summary>
        /// 计划业务缓存名称
        /// </summary>
        public String ScheduleJobCacheName { get; set; }

        /// <summary>
        /// 行政区域缓存名称
        /// </summary>
        public String AreaCacheName { get; set; }
    }
}
