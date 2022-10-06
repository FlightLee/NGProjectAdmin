using System;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// 邮件配置
    /// </summary>
    public class MailConfig
    {
        /// <summary>
        /// 主机地址
        /// </summary>
        public String Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// 发件人地址
        /// </summary>
        public String SenderAddress { get; set; }
    }
}
