
using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.MenuRepository
{
    /// <summary>
    /// 菜单数据访问层实现
    /// </summary>
    class MenuRepository : NGAdminBaseRepository<SysMenu>, IMenuRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public MenuRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
