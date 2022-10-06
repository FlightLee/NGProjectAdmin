using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Global;
using System;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.CoreControllers
{
    [Route(NGAdminGlobalContext.RouteTemplate)]
    [ApiController]
    public class NGAdminSystemController : ControllerBase
    {
        #region 获取消息中间件类型

        /// <summary>
        /// 获取消息中间件类型
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetMomType()
        {
            var actionResult = new Entity.CoreEntity.ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = (int)NGAdminGlobalContext.MomConfig.MomType;

            return Ok(actionResult);
        }

        #endregion
    }
}
