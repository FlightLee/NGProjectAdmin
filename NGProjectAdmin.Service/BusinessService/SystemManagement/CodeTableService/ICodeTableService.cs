
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.Base;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.CodeTableService
{
    /// <summary>
    /// 数据字典业务层接口
    /// </summary>
    public interface ICodeTableService : INGAdminBaseService<SysCodeTable>
    {
        /// <summary>
        /// 获取字典树
        /// </summary>
        /// <returns>ActionResult</returns>
        Task<QueryResult<SysCodeTableDTO>> GetCodeTreeNodes();

        /// <summary>
        /// 加载数据字典缓存
        /// </summary>
        Task LoadSystemCodeTableCache();

        /// <summary>
        /// 清理数据字典缓存
        /// </summary>
        Task ClearSystemCodeTableCache();
    }
}
