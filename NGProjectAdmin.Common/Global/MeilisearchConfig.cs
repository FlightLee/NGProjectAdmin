using System;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// Meilisearch配置
    /// </summary>
    public class MeilisearchConfig
    {
        /// <summary>
        /// URL
        /// </summary>
        public String URL { get; set; }

        /// <summary>
        /// ApiKey
        /// </summary>
        public String ApiKey { get; set; }

        /// <summary>
        /// Index
        /// </summary>
        public String Index { get; set; }
    }
}
