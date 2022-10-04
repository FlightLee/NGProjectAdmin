using System;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// ActiveMQ配置
    /// </summary>
    public class ActiveMQConfig
    {
        /// <summary>
        /// ActiveMQ连接串
        /// </summary>
        public String ConnectionString { get; set; }

        /// <summary>
        /// 主题名称
        /// </summary>
        public String TopicName { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public String QueueName { get; set; }
    }
}
