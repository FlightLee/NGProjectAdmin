
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Service.Base;
using System;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.ScheduleJobService
{
    /// <summary>
    /// 计划任务业务层接口
    /// </summary>
    public interface IScheduleJobService : INGAdminBaseService<SysScheduleJob>
    {
        /// <summary>
        /// 启动计划任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <returns></returns>
        Task StartScheduleJobAsync(Guid jobId);

        /// <summary>
        /// 暂停计划任务
        /// </summary>
        /// <param name="jobId">作业编号</param>
        /// <returns></returns>
        Task PauseScheduleJobAsync(Guid jobId);

        /// <summary>
        /// 恢复计划任务
        /// </summary>
        /// <param name="jobId">作业编号</param>
        /// <returns></returns>
        Task ResumeScheduleJobAsync(Guid jobId);

        /// <summary>
        /// 删除计划任务
        /// </summary>
        /// <param name="jobId">作业编号</param>
        /// <returns></returns>
        Task DeleteScheduleJobAsync(Guid jobId);

        /// <summary>
        /// 启动业务作业
        /// </summary>
        /// <returns></returns>
        Task StartScheduleJobAsync();

        /// <summary>
        /// 加载计划任务缓存
        /// </summary>
        Task LoadBusinessScheduleJobCache();

        /// <summary>
        /// 清理计划任务缓存
        /// </summary>
        Task ClearBusinessScheduleJobCache();
    }
}
