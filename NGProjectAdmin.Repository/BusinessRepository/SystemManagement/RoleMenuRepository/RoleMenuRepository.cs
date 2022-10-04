
using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.RoleMenuRepository
{
    /// <summary>
    /// 角色菜单数据访问层实现
    /// </summary>
    public class RoleMenuRepository : NGAdminBaseRepository<SysRoleMenu>, IRoleMenuRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public RoleMenuRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
