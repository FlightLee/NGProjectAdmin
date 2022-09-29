using Elasticsearch.Net;
using Nest;
using NGProjectAdmin.Common.Global;
using System;
using System.Collections.Generic;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// ES NEST工具类
    /// </summary>
    public static class NGEsNestContext
    {
        /// <summary>
        /// Lazy对象
        /// </summary>
        private static readonly Lazy<ElasticClient> LazyInstance = new Lazy<ElasticClient>(() =>
        {
            if (NGAdminGlobalContext.ElasticsearchConfig.Uri.Contains(','))
            {
                var urls = new List<Uri>();
                foreach (var item in NGAdminGlobalContext.ElasticsearchConfig.Uri.Split(','))
                {
                    urls.Add(new Uri(item));
                }
                //创建连接池
                var connectionPool = new SniffingConnectionPool(urls);

                //创建连接设置
                var settings = new ConnectionSettings(connectionPool);
                settings.DefaultIndex(NGAdminGlobalContext.ElasticsearchConfig.DefaultIndex);

                if (!String.IsNullOrEmpty(NGAdminGlobalContext.ElasticsearchConfig.UserName)
                    &&
                    !String.IsNullOrEmpty(NGAdminGlobalContext.ElasticsearchConfig.Password))
                {
                    settings.BasicAuthentication(NGAdminGlobalContext.ElasticsearchConfig.UserName,
                                                 NGAdminGlobalContext.ElasticsearchConfig.Password);
                }

                var elasticClient = new ElasticClient(settings);
                return elasticClient;
            }
            else
            {
                //创建连接设置
                var settings = new ConnectionSettings(new Uri(NGAdminGlobalContext.ElasticsearchConfig.Uri));
                settings.DefaultIndex(NGAdminGlobalContext.ElasticsearchConfig.DefaultIndex);

                if (!String.IsNullOrEmpty(NGAdminGlobalContext.ElasticsearchConfig.UserName)
                    &&
                    !String.IsNullOrEmpty(NGAdminGlobalContext.ElasticsearchConfig.Password))
                {
                    settings.BasicAuthentication(NGAdminGlobalContext.ElasticsearchConfig.UserName,
                                                 NGAdminGlobalContext.ElasticsearchConfig.Password);
                }

                var elasticClient = new ElasticClient(settings);
                return elasticClient;
            }
        });

        /// <summary>
        /// 单例对象
        /// </summary>
        public static ElasticClient Instance { get { return LazyInstance.Value; } }

        /// <summary>
        /// 是否已创建
        /// </summary>
        public static bool IsInstanceCreated { get { return LazyInstance.IsValueCreated; } }
    }
}
