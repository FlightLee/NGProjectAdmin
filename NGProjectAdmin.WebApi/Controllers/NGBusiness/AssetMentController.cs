using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.BusinessService.NGBusiness;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System.Net;
using System;
using System.Threading.Tasks;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.Entity.BusinessEnum;

namespace NGProjectAdmin.WebApi.Controllers.NGBusiness
{
    /// <summary>
    /// 资产评估
    /// </summary>
    public class AssetMentController : BaseController<Assetment_group>
    {
        private readonly IAssetment_groupService Assetment_groupService;


        public AssetMentController(IAssetment_groupService assetment_GroupService  ):base(assetment_GroupService)
        { 
            this.Assetment_groupService = assetment_GroupService;
        }
        [HttpPost]
        [Log(OperationType.QueryList)]
        [Permission("Asset:qurey:list")]
        public async Task<IActionResult> Post(QueryCondition queryCondition)
        {
           var assetments= await Assetment_groupService.GetListAsync(queryCondition);
     
            return Ok(assetments);

        }

    }
}
