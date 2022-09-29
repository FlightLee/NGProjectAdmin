
namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// 审计日志设置
    /// </summary>
    public class LogConfig
    {
        /// <summary>
        /// 审计日志是否开启
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 审计日志分表采集年份
        /// </summary>
        public int SplitTableYearTake { get; set; }

        /// <summary>
        /// 是否支持MongoDB
        /// </summary>
        public bool SupportMongoDB { get; set; }

        /// <summary>
        /// 是否支持Elasticsearch
        /// </summary>
        public bool SupportElasticsearch { get; set; }

        /// <summary>
        /// 是否支持Meilisearch
        /// </summary>
        public bool SupportMeilisearch { get; set; }
    }
}
