//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Entity.BusinessEnum;
using System;

namespace NGProjectAdmin.Entity.BusinessEntity.System
{
    /// <summary>
    /// 系统消息
    /// </summary>
    public class SystemMessage
    {
        /// <summary>
        /// 消息
        /// </summary>
        public String Message { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// 信息载体
        /// </summary>
        public Object Object { get; set; }
    }
}
