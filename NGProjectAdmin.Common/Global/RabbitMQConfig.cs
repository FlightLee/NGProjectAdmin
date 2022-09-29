using System;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// RabbitMQ配置
    /// </summary>
    public class RabbitMQConfig
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// ip地址，多个时以英文“,”分割
        /// </summary>
        public String HostName { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 虚拟队列名称
        /// </summary>
        public String QueueName { get; set; }

        /// <summary>
        /// 虚拟交换机名称
        /// </summary>
        public String ExchangeName { get; set; }
    }
}
