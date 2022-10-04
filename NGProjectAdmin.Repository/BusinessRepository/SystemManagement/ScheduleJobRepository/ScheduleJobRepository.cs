

using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.ScheduleJobRepository
{
    /// <summary>
    /// 计划任务数据访问层实现
    /// </summary>
    public class ScheduleJobRepository : NGAdminBaseRepository<SysScheduleJob>, IScheduleJobRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public ScheduleJobRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
