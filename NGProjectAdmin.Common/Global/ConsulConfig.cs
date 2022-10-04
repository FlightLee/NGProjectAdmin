
using System;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// Consul配置
    /// </summary>
    public class ConsulConfig
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public String ServiceName { get; set; }

        /// <summary>
        /// 服务端口
        /// </summary>
        public int ServicePort { get; set; }

        /// <summary>
        /// ConsulIP
        /// </summary>
        public String ConsulHostIP { get; set; }

        /// <summary>
        /// ConsulPort
        /// </summary>
        public int ConsulHostPort { get; set; }

        /// <summary>
        /// 服务启动至注册时间间隔
        /// </summary>
        public int DeregisterCriticalServiceAfter { get; set; }

        /// <summary>
        /// 心跳间隔
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; set; }
    }
}
