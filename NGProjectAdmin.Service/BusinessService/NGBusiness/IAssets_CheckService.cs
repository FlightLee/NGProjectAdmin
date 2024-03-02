using NGProjectAdmin.Entity.BusinessDTO.NGBusiness;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.NGBusiness
{
    /// <summary>
    /// Assetment_detail 业务接口
    /// </summary>
    public interface IAssets_CheckService : INGAdminBaseService<assets_check>
    {   

        Task<QueryResult<Assets_CheckDTO>> GetAssetCheckListAsync(QueryCondition queryCondition);
    }
}
