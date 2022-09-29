using System;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// Kafka配置
    /// </summary>
    public class KafkaConfig
    {
        /// <summary>
        /// 地址
        /// </summary>
        public String BootstrapServers { get; set; }

        /// <summary>
        /// GroupId
        /// </summary>
        public String GroupId { get; set; }

        /// <summary>
        /// 间隔时间
        /// </summary>
        public int StatisticsIntervalMs { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int SessionTimeoutMs { get; set; }
    }
}
