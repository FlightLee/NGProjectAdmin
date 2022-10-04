
using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.OrganizationRepository
{
    /// <summary>
    /// 机构数据访问层实现
    /// </summary>
    public class OrganizationRepository : NGAdminBaseRepository<SysOrganization>, IOrganizationRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public OrganizationRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
