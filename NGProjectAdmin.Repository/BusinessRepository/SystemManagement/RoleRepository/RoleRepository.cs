using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.RoleRepository
{
    /// <summary>
    /// 角色数据访问层实现
    /// </summary>
    public class RoleRepository : NGAdminBaseRepository<SysRole>, IRoleRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public RoleRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
