
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.Base;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.AnnouncementService
{
    /// <summary>
    /// 通知公告服务层接口
    /// </summary>   
    public interface IAnnouncementService : INGAdminBaseService<SysAnnouncement>
    {
        /// <summary>
        /// 查询通知列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql key</param>
        /// <returns>QueryResult</returns>
        Task<QueryResult<SysNotificationDTO>> QueryNotifications(QueryCondition queryCondition, string sqlKey);
    }
}
