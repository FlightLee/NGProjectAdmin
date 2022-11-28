using NGProjectAdmin.Entity.BusinessDTO.NGBusiness;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.Base;
using NPOI.SS.Formula.Functions;

namespace NGProjectAdmin.Repository.BusinessRepository.NGBusiness
{
    /// <summary>
    /// BizUser数据访问层接口
    /// </summary>   
    public interface IAssets_infoRepository : IBaseRepository<Assets_info>
    {
        Task<List<Assets_infoDTO>> GetAssetInfoListAsync(QueryCondition queryCondition);

        Task<Assets_infoDTO> GetAssetByIdAsync(Assets_infoDTO assetId);
    }
}
