using System;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// SmartThreadPool配置
    /// </summary>
    public class SmartThreadPoolConfig
    {
        /// <summary>
        /// 线程池的名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 线程池超时时间
        /// </summary>
        public int IdleTimeout { get; set; }

        /// <summary>
        /// 线程池的最大并发数
        /// </summary>
        public int Concurrency { get; set; }

        /// <summary>
        /// 最大线程数
        /// </summary>
        public int MaxThreads { get; set; }

        /// <summary>
        /// 最小线程数
        /// </summary>
        public int MinThreads { get; set; }

        /// <summary>
        /// 工作组最大并发数
        /// </summary>
        public int WorkItemsGroup { get; set; }
    }
}
