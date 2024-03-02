using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessDTO.NGBusiness;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.Base;
using SqlSugar;

namespace NGProjectAdmin.Repository.BusinessRepository.NGBusiness
{
    /// <summary>
    /// File_group数据访问层实现
    /// </summary>   
    public interface IAssets_ChecklRepository : INGAdminBaseRepository<assets_check>
    {
 
        Task<List<Assets_CheckDTO>> GetAssetCheckListAsync(QueryCondition queryCondition, RefAsync<int> totalCount);

    }
}
