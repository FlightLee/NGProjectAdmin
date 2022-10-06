using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.WebApi.AppCode.AuthorizationFilter;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.CoreControllers
{
    [Authorize]
    [ActionAuthorization]
    [ApiController]
    [Route(NGAdminGlobalContext.RouteTemplate)]
    public class NGAdminCapPublishController : ControllerBase
    {
        private readonly ICapPublisher capPublisher;

        public NGAdminCapPublishController(ICapPublisher capPublisher)
        {
            this.capPublisher = capPublisher;
        }

        [HttpPost]
        public IActionResult SendMessage()
        {
            var header = new Dictionary<string, string>()
            {
                ["my.header.first"] = "first",
                ["my.header.second"] = "second"
            };

            capPublisher.Publish("RuYiAdmin.CapService.ShowTime", DateTime.Now, header);

            return Ok();
        }
    }
}
