
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Service.BusinessService.RedisService;
using NGProjectAdmin.WebApi.AppCode.JwtSecurity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.JwtSecurityAuthentication
{
    /// <summary>
    /// Jwt接口认证控制器
    /// </summary>
    [ApiController]
    [Route(NGAdminGlobalContext.RouteTemplate)]
    public class JwtSecurityAuthenticationController : ControllerBase
    {
        #region 属性及构造函数

        /// <summary>
        /// Redis接口实例
        /// </summary>
        private readonly IRedisService redisService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="redisService"></param>
        public JwtSecurityAuthenticationController(IRedisService redisService)
        {
            this.redisService = redisService;
        }

        #endregion

        #region 获取盐份

        /// <summary>
        /// 获取盐份
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>盐份</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(String userName)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new NGAdminCustomException("user name can not be null");
            }

            if (userName.Equals(NGAdminGlobalContext.JwtSettings.DefaultUser))
            {
                var salt = Guid.NewGuid();
                var encrytStr = NGHashUtil.HashToHexString512(NGAdminGlobalContext.JwtSettings.DefaultPassword + salt);
                await redisService.SetAsync(salt.ToString(), encrytStr, NGAdminGlobalContext.JwtSettings.SaltExpiration);
                var actionResult = new NGProjectAdmin.Entity.CoreEntity.ActionResult()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Object = salt,
                    Message = new String("OK")
                };
                return Ok(actionResult);
            }

            throw new NGAdminCustomException("no content");
        }

        #endregion

        #region 获取Jwt口令

        /// <summary>
        /// Jwt身份认证接口
        /// </summary>
        /// <param name="jwtAuthentication">身份信息</param>
        /// <returns>token信息</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] JwtAuthentication jwtAuthentication)
        {
            if (!ModelState.IsValid)
            {
                throw new NGAdminCustomException("Model State Is Valid");
            }

            var encrytStr = String.Empty;

            if (await redisService.ExistsAsync(new string[] { jwtAuthentication.Salt }) > 0)
            {
                encrytStr = await redisService.GetAsync(jwtAuthentication.Salt);
            }

            if (String.IsNullOrEmpty(encrytStr))
            {
                throw new NGAdminCustomException("salt is not validated");
            }

            if (jwtAuthentication.UserName.Equals(NGAdminGlobalContext.JwtSettings.DefaultUser)
                && jwtAuthentication.Password.Equals(encrytStr, StringComparison.OrdinalIgnoreCase))
            {
                await redisService.DeleteAsync(new string[] { jwtAuthentication.Salt });

                var token = JwtAuthentication.GetJwtSecurityToken(jwtAuthentication.UserName);

                //设置RefreshToken有效期
                await this.redisService.SetAsync(token.Id, token.Id, 12 * 3 * NGAdminGlobalContext.JwtSettings.TokenExpiration * 60);

                var actionResult = new NGProjectAdmin.Entity.CoreEntity.ActionResult()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Object = new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token), RefreshToken = token.Id },
                    Message = "OK,expiration:" + token.ValidTo.ToLocalTime()
                };

                return Ok(actionResult);
            }

            throw new NGAdminCustomException("not found");
        }

        #endregion

        #region 刷新Jwt口令

        /// <summary>
        /// 刷新口令
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            String accessToken = this.HttpContext.Request.Headers["Authorization"];
            String refreshToken = this.HttpContext.Request.Headers["RefreshToken"];

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken.Replace("Bearer ", ""));
            if (jwtToken.Id.Equals(refreshToken))
            {
                var rtValue = await this.redisService.GetAsync(refreshToken);
                if (!String.IsNullOrEmpty(rtValue) && rtValue.Equals(refreshToken))
                {
                    //删除旧RefreshToken
                    await this.redisService.DeleteAsync(new String[] { refreshToken });

                    var token = JwtAuthentication.GetJwtSecurityToken(NGAdminGlobalContext.JwtSettings.DefaultUser);

                    //设置RefreshToken有效期
                    await this.redisService.SetAsync(token.Id, token.Id, 12 * 3 * NGAdminGlobalContext.JwtSettings.TokenExpiration * 60);

                    var actionResult = new NGProjectAdmin.Entity.CoreEntity.ActionResult()
                    {
                        HttpStatusCode = HttpStatusCode.OK,
                        Object = new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token), RefreshToken = token.Id },
                        Message = "OK,expiration:" + token.ValidTo.ToLocalTime()
                    };

                    return Ok(actionResult);
                }
            }

            throw new NGAdminCustomException("forbid");
        }

        #endregion
    }
}
