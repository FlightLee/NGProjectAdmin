//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Service.BusinessService.RedisService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.AreaService;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 行政区域管理控制器
    /// </summary>
    public class AreaManagementController : NGAdminBaseController<SysArea>
    {
        #region 属性及构造函数

        /// <summary>
        /// 行政区域业务接口实例
        /// </summary>
        private readonly IAreaService areaService;

        /// <summary>
        /// Redis接口实例
        /// </summary>
        private readonly IRedisService redisService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="areaService"></param>
        /// <param name="redisService"></param>
        public AreaManagementController(IAreaService areaService, IRedisService redisService) : base(areaService)
        {
            this.areaService = areaService;
            this.redisService = redisService;
        }

        #endregion

        #region 查询行政区域列表

        /// <summary>
        /// 查询行政区域列表
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        [Permission("area:query:list")]
        public async Task<IActionResult> Post()
        {
            var actionResult = await this.areaService.GetAreaTreeNodes();
            return Ok(actionResult);
        }

        #endregion
    }
}
