using Lazy.Captcha.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Class.CaptchaPicture;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using System;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.CoreControllers
{
    [Route(NGAdminGlobalContext.RouteTemplate)]
    [ApiController]
    public class NGAdminCaptchaController : ControllerBase
    {
        #region 属性及其构造函数

        //Lazy.Captcha接口实例
        private readonly ICaptcha lazyCaptcha;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lazyCaptcha"></param>
        public NGAdminCaptchaController(ICaptcha lazyCaptcha)
        {
            this.lazyCaptcha = lazyCaptcha;
        }

        #endregion

        #region 获取登录验证码

        /// <summary>
        /// 获取登录验证码
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetCaptcha()
        {
            var captcha = new Captcha();

            captcha.Id = Guid.NewGuid();
            CaptchaData lazyCaptchaInfo = this.lazyCaptcha.Generate(captcha.Id.ToString());
            captcha.CaptchaPicture = "data:image/gif;base64," + lazyCaptchaInfo.Base64;

            NGRedisContext.Set(captcha.Id.ToString(), lazyCaptchaInfo.Code, NGAdminGlobalContext.JwtSettings.SaltExpiration);

            var actionResult = new Entity.CoreEntity.ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = captcha;

            return Ok(actionResult);
        }

        #endregion
    }
}
