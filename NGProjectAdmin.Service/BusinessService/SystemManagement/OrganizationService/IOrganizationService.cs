
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.Base;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.OrganizationService
{
    /// <summary>
    /// 机构业务层接口
    /// </summary>
    public interface IOrganizationService : INGAdminBaseService<SysOrganization>
    {
        /// <summary>
        /// 获取机构树
        /// </summary>
        /// <returns>ActionResult</returns>
        Task<QueryResult<SysOrganizationDTO>> GetOrgTreeNodes();

        /// <summary>
        /// 获取机构、用户树
        /// </summary>
        /// <returns></returns>
        Task<QueryResult<OrgUserTreeDTO>> GetOrgUserTree();

        /// <summary>
        /// 加载系统机构缓存
        /// </summary>
        Task LoadSystemOrgCache();

        /// <summary>
        /// 清理系统机构缓存
        /// </summary>
        Task ClearSystemOrgCache();
    }
}
