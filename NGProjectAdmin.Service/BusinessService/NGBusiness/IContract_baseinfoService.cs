using NGProjectAdmin.Entity.BusinessDTO.NGBusiness;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.NGBusiness
{
    /// <summary>
    /// Contract_baseinfo 业务接口
    /// </summary>
    public interface IContract_baseinfoService : IBaseService<Contract_baseinfo>
    {
        Task<ActionResult> BuildContractFeeInfo(Contract_baseinfo contract_Baseinfo);

        Task<QueryResult<Contract_baseinfo>> GetContracts(QueryCondition queryCondition);

        Task<ActionResult> GetById(Contract_baseinfoDTO contract_Baseinfo);


        Task<ActionResult> GetAllAssets();
    }
}
