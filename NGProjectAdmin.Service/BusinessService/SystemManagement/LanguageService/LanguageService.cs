using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.BusinessRepository.RedisRepository;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.LanguageRepository;
using NGProjectAdmin.Service.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.LanguageService
{
    /// <summary>
    /// 多语业务层实现
    /// </summary>
    public class LanguageService : NGAdminBaseService<SysLanguage>, ILanguageService
    {
        #region 属性及构造函数

        /// <summary>
        /// 多语仓储实例
        /// </summary>
        private readonly ILanguageRepository languageRepository;

        /// <summary>
        /// Redis仓储实例
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="languageRepository"></param>
        /// <param name="redisRepository"></param>
        public LanguageService(ILanguageRepository languageRepository,
                               IRedisRepository redisRepository) : base(languageRepository)
        {
            this.languageRepository = languageRepository;
            this.redisRepository = redisRepository;
        }

        #endregion

        #region 加载系统多语缓存

        /// <summary>
        /// 加载系统多语缓存
        /// </summary>
        public async Task LoadSystemLanguageCache()
        {
            var sqlKey = "sqls:sql:query_syslanguage";
            var strSQL = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value;

            int totalCount = 0;
            
            var languages = (await this.languageRepository.SqlQueryAsync<SysLanguage>(new QueryCondition(), totalCount, strSQL)).
                                                    OrderBy(t => t.OrderNumber).ToList();
            await this.redisRepository.SetAsync(NGAdminGlobalContext.SystemCacheConfig.LanguageCacheName, languages, -1);

            NGLoggerContext.Info("系统多语缓存加载完成");
        }

        #endregion

        #region 清理系统多语缓存

        /// <summary>
        /// 清理系统多语缓存
        /// </summary>
        public async Task ClearSystemLanguageCache()
        {
            await this.redisRepository.DeleteAsync(new String[] { NGAdminGlobalContext.SystemCacheConfig.LanguageCacheName });

            NGLoggerContext.Info("系统多语缓存清理完成");
        }

        #endregion
    }
}