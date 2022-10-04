using System;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// SignalR配置
    /// </summary>
    public class SignalRConfig
    {
        /// <summary>
        /// 审计日志是否开启
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// pattern
        /// </summary>
        public String Pattern { get; set; }

        /// <summary>
        /// method
        /// </summary>
        public String Method { get; set; }
    }
}
