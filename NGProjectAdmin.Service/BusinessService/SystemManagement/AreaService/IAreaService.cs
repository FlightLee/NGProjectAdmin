
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.Base;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.AreaService
{
    /// <summary>
    /// 行政区域服务层接口
    /// </summary>
    public interface IAreaService : INGAdminBaseService<SysArea>
    {
        /// <summary>
        /// 加载行政区域缓存
        /// </summary>
        Task LoadSysAreaCache();

        /// <summary>
        /// 清理行政区域缓存
        /// </summary>
        Task ClearSysAreaCache();

        /// <summary>
        /// 获取行政区域树
        /// </summary>
        /// <returns></returns>
        Task<QueryResult<SysAreaDTO>> GetAreaTreeNodes();
    }
}
