using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Entity.BusinessDTO.NGBusiness;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.BusinessService.NGBusiness;
using NGProjectAdmin.Service.BusinessService.SystemManagement.UserService;
using NGProjectAdmin.WebApi.AppCode.AuthorizationFilter;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.NGBusiness
{

    /// <summary>
    /// 巡检控制器
    /// </summary>
    [Authorize]
    [ActionAuthorization]
    [ApiController]
    [Route(NGAdminGlobalContext.RouteTemplate)]
    public class AssetsCheckController : NGAdminBaseController<assets_check>
    {
        #region 属性及构造函数

        private readonly IAssets_CheckService assets_CheckService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Assets_CheckService"></param>
        public AssetsCheckController(IAssets_CheckService Assets_CheckService) : base(Assets_CheckService)
        {

            this.assets_CheckService = Assets_CheckService;
        }
        #endregion

        /// <summary>
        /// 查询巡检列表
        /// </summary>
        /// <param name="queryCondition"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetCheckList(QueryCondition queryCondition)
        {
            var actionResult = await assets_CheckService.GetAssetCheckListAsync(queryCondition);


            return Ok(actionResult);

        }

        /// <summary>
        /// 删除巡检
        /// </summary>
        /// <param name="assets_infoDTO">资产编号</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteById([FromBody] Assets_CheckDTO assets_infoDTO)
        {
            var actionResult = await assets_CheckService.DeleteAsync(assets_infoDTO.Id);
            return Ok(actionResult);



        }
    }
}