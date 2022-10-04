using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.RoleOrgRepository
{
    /// <summary>
    /// 角色机构数据访问层实现
    /// </summary>   
    public class RoleOrgRepository : NGAdminBaseRepository<SysRoleOrg>, IRoleOrgRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public RoleOrgRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
