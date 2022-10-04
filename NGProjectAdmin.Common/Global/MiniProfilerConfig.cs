using System;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// MiniProfiler配置
    /// </summary>
    public class MiniProfilerConfig
    {
        /// <summary>
        /// profiler URL
        /// </summary>
        public String RouteBasePath { get; set; }

        /// <summary>
        /// CacheDuration
        /// </summary>
        public int CacheDuration { get; set; }
    }
}
