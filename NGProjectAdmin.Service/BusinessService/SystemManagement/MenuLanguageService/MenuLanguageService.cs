
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.BusinessRepository.RedisRepository;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.MenuLanguageRepository;
using NGProjectAdmin.Service.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.MenuLanguageService
{
    /// <summary>
    /// 菜单多语业务层实现
    /// </summary>
    public class MenuLanguageService : NGAdminBaseService<SysMenuLanguage>, IMenuLanguageService
    {
        #region 属性及构造函数

        /// <summary>
        /// 菜单多语仓储实例
        /// </summary>
        private readonly IMenuLanguageRepository menuLanguageRepository;

        /// <summary>
        /// Redis仓储实例
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="menuLanguageRepository"></param>
        /// <param name="redisRepository"></param>
        public MenuLanguageService(IMenuLanguageRepository menuLanguageRepository,
                                   IRedisRepository redisRepository) : base(menuLanguageRepository)
        {
            this.menuLanguageRepository = menuLanguageRepository;
            this.redisRepository = redisRepository;
        }

        #endregion

        #region 服务层公有方法

        #region 加载菜单与多语缓存

        /// <summary>
        /// 加载菜单与多语缓存
        /// </summary>
        public async Task LoadSystemMenuLanguageCache()
        {
            var sqlKey = "sqls:sql:query_menu_language_info";
            var strSQL = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value;

            int totalCount = 0;
            var menuLanguages = (await this.menuLanguageRepository.SqlQueryAsync<SysMenuLanguage>(new QueryCondition(), totalCount, strSQL)).ToList();
            await this.redisRepository.SetAsync(NGAdminGlobalContext.SystemCacheConfig.MenuAndLanguageCacheName, menuLanguages, -1);

            NGLoggerContext.Info("系统菜单与多语缓存加载完成");
        }

        #endregion

        #region 清理菜单与多语缓存

        /// <summary>
        /// 清理菜单与多语缓存
        /// </summary>
        public async Task ClearSystemMenuLanguageCache()
        {
            await this.redisRepository.DeleteAsync(new String[] { NGAdminGlobalContext.SystemCacheConfig.MenuAndLanguageCacheName });

            NGLoggerContext.Info("系统菜单与多语缓存清理完成");
        }

        #endregion

        #endregion
    }
}