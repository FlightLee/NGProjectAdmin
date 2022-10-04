
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;

using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.LogRepository
{
    /// <summary>
    /// 审计日志数据访问层接口
    /// </summary>
    public interface ILogRepository : INGAdminBaseRepository<SysLog>
    {
    }
}
