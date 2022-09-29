using NGProjectAdmin.Entity.BusinessEnum;
using System;

namespace NGProjectAdmin.Entity.BusinessEntity.System
{
    /// <summary>
    /// 广播消息
    /// </summary>
    public class BroadcastMessage
    {
        /// <summary>
        /// 标题
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public String Message { get; set; }

        /// <summary>
        /// 消息级别
        /// </summary>
        public MessageLevel MessageLevel { get; set; }
    }
}
