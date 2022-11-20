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

        Task<ActionResult> GetAssetByIdAsync(string assetId);
    }
}
