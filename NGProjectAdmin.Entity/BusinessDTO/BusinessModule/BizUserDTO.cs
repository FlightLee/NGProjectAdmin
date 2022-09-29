//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

namespace NGProjectAdmin.Entity.BusinessDTO.BusinessModule
{
    /// <summary>
    /// 业务用户DTO
    /// </summary>
    public class BizUserDTO
    {
        /// <summary>
        /// 模块编号
        /// </summary>
        [Required]
        public Guid ModuleId { get; set; }

        /// <summary>
        /// 用户登录账号
        /// </summary>
        [Required]
        [MaxLength(128)]
        public String UserLogonName { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [MaxLength(128)]
        public String UserDisplayName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [MaxLength(512)]
        public String UserPassword { get; set; }

        /// <summary>
        /// 座机
        /// </summary>
        [MaxLength(45)]
        public String Telephone { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [MaxLength(45)]
        public String MobilePhone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(45)]
        public String Email { get; set; }

        /// <summary>
        /// 性别
        /// 0：男
        /// 1：女
        /// 2：第三性别
        /// </summary>
        [Required]
        public int Sex { get; set; }
    }
}
