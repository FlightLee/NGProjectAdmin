//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using System;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// Jwt配置信息
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// 是否启用JWT
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 订阅者
        /// </summary>
        public String Audience { get; set; }

        /// <summary>
        /// 发起人
        /// </summary>
        public String Issuer { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public String SecurityKey { get; set; }

        /// <summary>
        /// 缺省用户
        /// </summary>
        public String DefaultUser { get; set; }

        /// <summary>
        /// 缺省密码
        /// </summary>
        public String DefaultPassword { get; set; }

        /// <summary>
        /// 盐有效时间（秒）
        /// </summary>
        public int SaltExpiration { get; set; }

        /// <summary>
        /// token有效时间（分钟）
        /// </summary>
        public int TokenExpiration { get; set; }
    }
}
