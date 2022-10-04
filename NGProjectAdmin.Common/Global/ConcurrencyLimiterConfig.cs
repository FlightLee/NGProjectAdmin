//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

namespace NGProjectAdmin.Net.Common.Global
{
    /// <summary>
    /// 并发限制配置
    /// </summary>
    public class ConcurrencyLimiterConfig
    {
        /// <summary>
        /// 最大并发请求数
        /// </summary>
        public int MaxConcurrentRequests { get; set; }

        /// <summary>
        /// 请求队列长度限制
        /// </summary>
        public int RequestQueueLimit { get; set; }
    }
}
