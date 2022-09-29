
using System.ComponentModel;

namespace NGProjectAdmin.Entity.BusinessEnum
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum JobStatus
    {
        [Description("已启用")]
        Started,

        [Description("运行中")]
        Running,

        [Description("执行中")]
        Executing,

        [Description("执行完成")]
        Completed,

        [Description("任务计划中")]
        Planning,

        [Description("已停止")]
        Stopped
    }
}
