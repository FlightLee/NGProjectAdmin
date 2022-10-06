//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.AuthorizationFilter;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NGProjectAdmin.WebApi.Controllers.SystemManagement
{
    [Authorize]
    [ActionAuthorization]
    [ApiController]
    [Route(NGAdminGlobalContext.RouteTemplate)]
    public class ServerHardwareMonitorController : ControllerBase
    {
        #region 获取服务器信息

        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Log(OperationType.QueryEntity)]
        [Permission("query:hardware:info")]
        public async Task<IActionResult> Get()
        {
            return await Task.Run(() =>
            {
                var actionResult = new NGProjectAdmin.Entity.CoreEntity.ActionResult();
                actionResult.HttpStatusCode = HttpStatusCode.OK;
                actionResult.Message = new String("OK");
                var work = NGSmartThreadPool.Instance.QueueWorkItem(NGHardwareMonitorUtil.StartMonitoring);
                actionResult.Object = work.Result;
                return Ok(actionResult);
            });
        }

        #endregion
    }
}
