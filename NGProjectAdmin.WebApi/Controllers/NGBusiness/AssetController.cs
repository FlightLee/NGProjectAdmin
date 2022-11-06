using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.BusinessService.NGBusiness;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System.Threading.Tasks;

namespace NGProjectAdmin.WebApi.Controllers.NGBusiness
{
    /// <summary>
    /// 资产档案
    /// </summary>
    public class AssetController : BaseController<Assets_info>
    {
        private readonly IAssets_infoService Assets_infoService;
        public AssetController(IAssets_infoService assets_infoService) : base(assets_infoService)
        {
            this.Assets_infoService = assets_infoService;
        }

        /// <summary>
        /// 查询资产档案列表
        /// </summary>
        /// <param name="queryCondition"></param>
        /// <returns></returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        [AllowAnonymous]
        //[Permission("Asset:qurey:list")]
        public async Task<IActionResult> Post(QueryCondition queryCondition)
        {
            var actionResult = await this.Assets_infoService.GetAssetInfoListAsync(queryCondition);
            return Ok(actionResult);

        }
    }
}
