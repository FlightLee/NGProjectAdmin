
using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.OrgUserRepository
{
    /// <summary>
    /// 机构用户数据访问层实现
    /// </summary>
    public class OrgUserRepository : NGAdminBaseRepository<SysOrgUser>, IOrgUserRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public OrgUserRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
