using NGProjectAdmin.Entity.BusinessDTO.NGBusiness;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.Base;
using NPOI.SS.Formula.Functions;
using SqlSugar;

namespace NGProjectAdmin.Repository.BusinessRepository.NGBusiness
{
    /// <summary>
    /// BizUser数据访问层接口
    /// </summary>   
    public interface IContract_baseinfoRepository : IBaseRepository<Contract_baseinfo>
    {

        Task<List<Contract_feeinfo>>  BuildContractFeeInfo(Contract_baseinfo contract_Baseinfo);

        Task<List<Contract_baseinfo>> GetContracts(QueryCondition queryCondition, RefAsync<int> totalCount);

        Task<Contract_baseinfoDTO> GetById(Contract_baseinfoDTO contract_Baseinfo);


        Task<List<AssetsSelect>> GetAllAssets();

    }
}
