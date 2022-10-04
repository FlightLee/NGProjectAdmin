
using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.AddresseeRepository
{
    /// <summary>
    /// 收件人数据访问层实现
    /// </summary>
    public class AddresseeRepository : NGAdminBaseRepository<SysAddressee>, IAddresseeRepository
    {
        #region 属性及构造函数

        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public AddresseeRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }

        #endregion
    }
}
