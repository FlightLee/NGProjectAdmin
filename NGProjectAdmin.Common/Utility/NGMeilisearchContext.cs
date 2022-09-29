using Meilisearch;
using NGProjectAdmin.Common.Global;
using System;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// Meilisearch工具类
    /// </summary>
    public static class NGMeilisearchContext
    {
        /// <summary>
        /// Lazy对象
        /// </summary>
        private static readonly Lazy<MeilisearchClient> LazyInstance = new Lazy<MeilisearchClient>(() =>
        {
            var client = new MeilisearchClient(NGAdminGlobalContext.MeilisearchConfig.URL, NGAdminGlobalContext.MeilisearchConfig.ApiKey);
            return client;
        });

        /// <summary>
        /// 单例对象
        /// </summary>
        public static MeilisearchClient Instance { get { return LazyInstance.Value; } }

        /// <summary>
        /// 是否已创建
        /// </summary>
        public static bool IsInstanceCreated { get { return LazyInstance.IsValueCreated; } }

        /// <summary>
        /// 获取Index
        /// </summary>
        /// <param name="meilisearch">MeilisearchClient</param>
        /// <returns>Meilisearch.Index</returns>
        public static Meilisearch.Index GetIndex(this MeilisearchClient meilisearch)
        {
            return Instance.Index(NGAdminGlobalContext.MeilisearchConfig.Index);
        }
    }
}
