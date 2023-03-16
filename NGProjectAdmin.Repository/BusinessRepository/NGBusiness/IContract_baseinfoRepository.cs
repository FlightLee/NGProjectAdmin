using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Repository.Base;
using NPOI.SS.Formula.Functions;

namespace NGProjectAdmin.Repository.BusinessRepository.NGBusiness
{
    /// <summary>
    /// BizUser数据访问层接口
    /// </summary>   
    public interface IContract_baseinfoRepository : IBaseRepository<Contract_baseinfo>
    {

        Task<List<Contract_feeinfo>>  BuildContractFeeInfo(Contract_baseinfo contract_Baseinfo);

    }
}
