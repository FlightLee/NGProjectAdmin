
using System;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// Apollo客户端配置
    /// </summary>
    public class ApolloConfig
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// AppId
        /// </summary>
        public String AppId { get; set; }

        /// <summary>
        /// MetaServer
        /// </summary>
        public String MetaServer { get; set; }

        /// <summary>
        /// ConfigServer
        /// </summary>
        public String[] ConfigServer { get; set; }

        /// <summary>
        /// Env
        /// </summary>
        public String Env { get; set; }
    }
}
