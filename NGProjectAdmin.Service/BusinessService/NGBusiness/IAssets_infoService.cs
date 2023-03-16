using NGProjectAdmin.Entity.BusinessDTO.NGBusiness;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.Base;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.NGBusiness
{
    /// <summary>
    /// Assets_info 业务接口
    /// </summary>
    public interface IAssets_infoService : IBaseService<Assets_info>
    {
        Task<QueryResult<Assets_infoDTO>> GetAssetInfoListAsync(QueryCondition queryCondition);

        Task<ActionResult> GetAssetByIdAsync(Assets_infoDTO assetId);


        /// <summary>
        /// 删除资产时把对应的合同和缴费信息也停掉
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        Task<ActionResult> DeleteAssetAndContract(Assets_infoDTO assetId);

        /// <summary>
        /// 首页资产统计报表
        /// </summary>
        /// <returns></returns>
        Task<ActionResult> GetAssetsData();

        Task<ActionResult> UpdateAssetsByContractId(string contractId);
    }
}
