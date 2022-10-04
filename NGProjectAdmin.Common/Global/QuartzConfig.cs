
using System;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// Quartz配置类
    /// </summary>
    public class QuartzConfig
    {
        /// <summary>
        /// 计划任务组
        /// </summary>
        public String ScheduleJobGroup { get; set; }

        /// <summary>
        /// 计划任务触发器
        /// </summary>
        public String ScheduleJobTrigger { get; set; }

        /// <summary>
        /// 是否支持集群
        /// </summary>
        public bool SupportGroup { get; set; }

        /// <summary>
        /// 集群编号
        /// </summary>
        public Nullable<int> GroupId { get; set; }

        /// <summary>
        /// 通道名称
        /// </summary>
        public string ChanelName { get; set; }
    }
}
