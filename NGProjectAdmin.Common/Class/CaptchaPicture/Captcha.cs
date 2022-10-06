using System;

namespace NGProjectAdmin.Common.Class.CaptchaPicture
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class Captcha
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// base64格式验证码
        /// </summary>
        public String CaptchaPicture { get; set; }
    }
}
