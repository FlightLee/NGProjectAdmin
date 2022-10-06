using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.Base;
using System;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.AddresseeService
{
    /// <summary>
    /// 收件人服务层接口
    /// </summary>
    public interface IAddresseeService : INGAdminBaseService<SysAddressee>
    {
        /// <summary>
        /// 更改通知收件人阅读状态
        /// </summary>
        /// <param name="notificationId">通知编号</param>
        /// <returns>ActionResult</returns>
        Task<ActionResult> UpdateNotificationStatus(Guid notificationId);
    }
}
