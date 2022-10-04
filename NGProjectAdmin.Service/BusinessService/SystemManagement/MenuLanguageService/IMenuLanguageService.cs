using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Service.Base;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.MenuLanguageService
{
    /// <summary>
    /// 菜单多语业务层接口
    /// </summary>
    public interface IMenuLanguageService : INGAdminBaseService<SysMenuLanguage>
    {
        /// <summary>
        /// 加载菜单与多语缓存
        /// </summary>
        Task LoadSystemMenuLanguageCache();

        /// <summary>
        /// 清理菜单与多语缓存
        /// </summary>
        Task ClearSystemMenuLanguageCache();
    }
}
