using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.LogRepository
{
    /// <summary>
    /// 审计日志数据访问层实现
    /// </summary>
    public class LogRepository : NGAdminBaseRepository<SysLog>, ILogRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public LogRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
