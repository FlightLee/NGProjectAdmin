using System;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// Cap分布式事务配置
    /// </summary>
    public class CapConfig
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 默认组名
        /// </summary>
        public String DefaultGroupName { get; set; }

        /// <summary>
        /// 表名前缀
        /// </summary>
        public String TableNamePrefix { get; set; }

        /// <summary>
        /// MQ类型
        /// RabbitMQ=0,Kafka=1,Redis=2
        /// </summary>
        public int CapMqType { get; set; }

        /// <summary>
        /// 当前节点主机名称
        /// </summary>
        public String CurrentNodeHostName { get; set; }

        /// <summary>
        /// 节点编号
        /// </summary>
        public String NodeId { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public String NodeName { get; set; }
    }
}
