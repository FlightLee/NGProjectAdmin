using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.AddresseeRepository;
using NGProjectAdmin.Service.Base;
using System;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.AddresseeService
{
    /// <summary>
    /// 收件人服务层实现
    /// </summary>
    public class AddresseeService : NGAdminBaseService<SysAddressee>, IAddresseeService
    {
        #region 属性及构造函数

        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IAddresseeRepository addresseeRepository;

        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="addresseeRepository"></param>
        /// <param name="context"></param>
        public AddresseeService(IAddresseeRepository addresseeRepository,
                                IHttpContextAccessor context) : base(addresseeRepository)
        {
            this.addresseeRepository = addresseeRepository;
            this.context = context;
        }

        #endregion

        #region 更改通知收件人阅读状态

        /// <summary>
        /// 更改通知收件人阅读状态
        /// </summary>
        /// <param name="notificationId">通知编号</param>
        /// <returns>ActionResult</returns>
        public async Task<ActionResult> UpdateNotificationStatus(Guid notificationId)
        {
            //获取用户信息
            var user = NGAdminSessionContext.GetCurrentUserInfo(this.context);

            //获取通知信息
            var addressee = (await this.addresseeRepository.GetListAsync()).Find(t => t.IsDel == 0 && t.BusinessId == notificationId && t.UserId == user.Id);
            if (addressee != null && addressee.Status == ((int)ReadingStatus.Unread))
            {
                addressee.Status = (int)ReadingStatus.Read;
                await this.addresseeRepository.UpdateEntityAsync(addressee);
            }

            return ActionResult.Success("OK");
        }

        #endregion
    }
}
