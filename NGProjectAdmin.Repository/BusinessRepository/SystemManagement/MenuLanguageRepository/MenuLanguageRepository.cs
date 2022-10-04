
using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.MenuLanguageRepository
{
    /// <summary>
    /// 菜单多语数据访问层实现
    /// </summary>
    public class MenuLanguageRepository : NGAdminBaseRepository<SysMenuLanguage>, IMenuLanguageRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public MenuLanguageRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
