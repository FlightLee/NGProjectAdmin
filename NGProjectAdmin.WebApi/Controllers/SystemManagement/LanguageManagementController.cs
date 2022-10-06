//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Class.Extensions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.BusinessService.RedisService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.LanguageService;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 多语管理控制器
    /// </summary>
    public class LanguageManagementController : NGAdminBaseController<SysLanguage>
    {
        #region 属性及构造函数

        /// <summary>
        /// 多语管理业务接口实例
        /// </summary>
        private readonly ILanguageService languageService;

        /// <summary>
        /// Redis接口实例
        /// </summary>
        private readonly IRedisService redisService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="languageService"></param>
        /// <param name="redisService"></param>
        public LanguageManagementController(ILanguageService languageService,
                                            IRedisService redisService) : base(languageService)
        {
            this.languageService = languageService;
            this.redisService = redisService;
        }

        #endregion

        #region 查询多语列表

        /// <summary>
        /// 查询多语列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        //[Log(OperationType.QueryList)]
        //[Permission("language:query:list")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(QueryCondition queryCondition)
        {
            var languages = await this.redisService.GetAsync<List<SysLanguage>>(NGAdminGlobalContext.SystemCacheConfig.LanguageCacheName);
            languages = languages.AsQueryable().Where(QueryCondition.BuildExpression<SysLanguage>(queryCondition.QueryItems)).ToList();

            if (!String.IsNullOrEmpty(queryCondition.Sort))
            {
                languages = languages.Sort<SysLanguage>(queryCondition.Sort);
            }

            var actionResult = new QueryResult<SysLanguage>();
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.TotalCount = languages.Count;
            actionResult.List = languages.Skip(queryCondition.PageIndex * queryCondition.PageSize).Take(queryCondition.PageSize).ToList();

            return Ok(actionResult);
        }

        #endregion
    }
}
