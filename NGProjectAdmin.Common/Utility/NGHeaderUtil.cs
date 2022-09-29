
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// Request Header工具类
    /// </summary>
    public static class NGHeaderUtil
    {
        /// <summary>
        /// 获取Header中指定key的值
        /// </summary>
        /// <param name="context">HttpContext对象</param>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static String GetHeaderValue(this HttpContext context, String key)
        {
            var token = String.Empty;

            token = context.Request.Headers[key].FirstOrDefault();

            return token;
        }
    }
}
