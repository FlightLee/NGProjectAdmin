using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.WebApi.AppCode.AuthorizationFilter;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.CoreControllers
{
    [Authorize]
    [ActionAuthorization]
    [ApiController]
    [Route(NGAdminGlobalContext.RouteTemplate)]
    public class NGAdminCapConsumerController : ControllerBase
    {
        private readonly ILogger<NGAdminCapConsumerController> logger;

        public NGAdminCapConsumerController(ILogger<NGAdminCapConsumerController> logger)
        {
            this.logger = logger;
        }

        [NonAction]
        [CapSubscribe("RuYiAdmin.CapService.ShowTime")]
        public void ReceiveMessage(DateTime time, [FromCap] CapHeader header)
        {
            logger.LogInformation("message time is:" + time);
            logger.LogInformation("message firset header :" + header["my.header.first"]);
            logger.LogInformation("message second header :" + header["my.header.second"]);
        }
    }
}
