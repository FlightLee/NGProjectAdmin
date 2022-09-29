//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using System;

namespace NGProjectAdmin.Entity.BusinessDTO.SystemManagement
{
    /// <summary>
    /// 用户登录DTO
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// 验证码编号
        /// </summary>
        public String CaptchaId { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public String Captcha { get; set; }
    }
}
