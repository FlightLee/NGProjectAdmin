using Amib.Threading;
using NGProjectAdmin.Common.Global;
using System;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// SmartThreadPool工具类
    /// </summary>
    public class NGSmartThreadPool
    {
        /// <summary>
        /// 私有化构造函数
        /// 用于单例模式
        /// </summary>
        private NGSmartThreadPool() { }

        /// <summary>
        /// Lazy对象
        /// </summary>
        private static readonly Lazy<SmartThreadPool> LazyInstance = new Lazy<SmartThreadPool>(() =>
        {
            var stp = new SmartThreadPool(
                NGAdminGlobalContext.SmartThreadPoolConfig.IdleTimeout,
                NGAdminGlobalContext.SmartThreadPoolConfig.MaxThreads,
                NGAdminGlobalContext.SmartThreadPoolConfig.MinThreads
                );
            stp.Name = NGAdminGlobalContext.SmartThreadPoolConfig.Name;
            //stp.Concurrency = GlobalContext.SmartThreadPoolConfig.Concurrency;
            stp.CreateWorkItemsGroup(NGAdminGlobalContext.SmartThreadPoolConfig.WorkItemsGroup);
            return stp;
        });

        /// <summary>
        /// 单例对象
        /// </summary>
        public static SmartThreadPool Instance { get { return LazyInstance.Value; } }

        /// <summary>
        /// 是否已创建
        /// </summary>
        public static bool IsInstanceCreated { get { return LazyInstance.IsValueCreated; } }
    }
}
