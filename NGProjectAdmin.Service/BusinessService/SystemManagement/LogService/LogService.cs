using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.LogRepository;
using NGProjectAdmin.Service.Base;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.LogService
{
    /// <summary>
    /// 审计日志业务层实现
    /// </summary>
    public class LogService : NGAdminBaseService<SysLog>, ILogService
    {
        /// <summary>
        /// 审计日志仓储实例
        /// </summary>
        private readonly ILogRepository logRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logRepository"></param>
        public LogService(ILogRepository logRepository) : base(logRepository)
        {
            this.logRepository = logRepository;
        }
    }
}
