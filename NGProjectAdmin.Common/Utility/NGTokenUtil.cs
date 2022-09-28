using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Common.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Common.Utility
{
    public static class NGTokenUtil
    {
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="context">Http会话</param>
        /// <returns>token</returns>
        public static String GetToken(this HttpContext context)
        {
            var token = String.Empty;
            //从头部获取token
            token = context.Request.Headers["token"];
            //从头部获取salt
            var tokenSalt = context.Request.Headers["ts"];
            //token解密
            token = NGRsaUtil.PemDecrypt(token, NGAdminGlobalContext.SystemConfig.RsaPrivateKey);
            //移除salt
            token = token.ToString().Replace("^" + tokenSalt, "");
            //返回token
            return token;
        }

        /// <summary>
        /// 获取token salt
        /// </summary>
        /// <param name="context">Http会话</param>
        /// <returns>salt</returns>
        public static String GetTokenSalt(this HttpContext context)
        {
            //从头部获取salt
            return context.Request.Headers["ts"];
        }
    }
}
