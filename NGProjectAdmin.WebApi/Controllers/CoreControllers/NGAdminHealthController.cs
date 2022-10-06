//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Global;
using System.Threading.Tasks;

namespace NGProjectAdmin.WebApi.Controllers.CoreControllers
{
    /// <summary>
    /// 健康检查控制器
    /// </summary>
    [AllowAnonymous]
    [Route(NGAdminGlobalContext.RouteTemplate)]
    [Produces("application/json")]
    [ApiController]
    public class NGAdminHealthController : ControllerBase
    {
        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns>OkObjectResult</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Task.Run(() =>
            {
                return Ok("ok");
            });
        }
    }
}
