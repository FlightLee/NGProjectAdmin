using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.WebApi.AppCode.AuthorizationFilter;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.CoreControllers
{
    [Authorize]
    [ActionAuthorization]
    [ApiController]
    [Route(NGAdminGlobalContext.RouteTemplate)]
    public class NGAdminApolloController : ControllerBase
    {
        /// <summary>
        /// 输出实例
        /// </summary>
        private readonly ILogger<NGAdminApolloController> logger;

        /// <summary>
        /// 全局配置
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">输出实例</param>
        /// <param name="configuration">全局配置</param>
        public NGAdminApolloController(ILogger<NGAdminApolloController> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new NGAdminCustomException("key can not be empty");
            }

            var value = configuration[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new NGAdminCustomException("value is null");
            }

            logger.LogInformation($"Apollo:{key},{value}");

            return Ok(value);
        }
    }
}
