

using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Service.Base;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.LanguageService
{
    /// <summary>
    /// 多语业务层接口
    /// </summary>
    public interface ILanguageService : INGAdminBaseService<SysLanguage>
    {
        /// <summary>
        /// 加载系统多语缓存
        /// </summary>
        Task LoadSystemLanguageCache();

        /// <summary>
        /// 清理系统多语缓存
        /// </summary>
        Task ClearSystemLanguageCache();
    }
}
