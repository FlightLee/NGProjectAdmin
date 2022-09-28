using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemConfig
    {
        /// <summary>
        /// Pem格式Rsa公钥
        /// </summary>
        public String RsaPublicKey { get; set; }=String.Empty;

        /// <summary>
        /// Pem格式Rsa私钥
        /// </summary>
        public String RsaPrivateKey { get; set; } = String.Empty;

        /// <summary>
        /// 白名单
        /// </summary>
        public String WhiteList { get; set; } = String.Empty;

        /// <summary>
        /// Header配置
        /// </summary>
        public String HeaderConfig { get; set; } = String.Empty;

        /// <summary>
        /// 机构根目录编号
        /// </summary>
        public Guid OrgRoot { get; set; }

        /// <summary>
        /// 默认密码
        /// </summary>
        public String DefaultPassword { get; set; } = String.Empty;

        /// <summary>
        /// AesKey
        /// </summary>
        public String AesKey { get; set; } = String.Empty;

        /// <summary>
        /// 登录上限
        /// </summary>
        public int LogonCountLimit { get; set; }

        /// <summary>
        /// TokenKey
        /// </summary>
        public String TokenKey { get; set; } = String.Empty;

        /// <summary>
        /// 检测Token开关
        /// </summary>
        public bool CheckToken { get; set; }

        /// <summary>
        /// token有效时间（分钟）
        /// </summary>
        public int UserTokenExpiration { get; set; }

        /// <summary>
        /// 检测JwtToken开关
        /// </summary>
        public bool CheckJwtToken { get; set; }

        /// <summary>
        /// 生产环境是否支持SwaggerUI
        /// </summary>
        public bool SupportSwaggerOnProduction { get; set; }
    }
}
