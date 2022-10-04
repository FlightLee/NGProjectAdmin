
using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.RoleUserRepository
{
    /// <summary>
    /// 角色用户数据访问层实现
    /// </summary>
    public class RoleUserRepository : NGAdminBaseRepository<SysRoleUser>, IRoleUserRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public RoleUserRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
