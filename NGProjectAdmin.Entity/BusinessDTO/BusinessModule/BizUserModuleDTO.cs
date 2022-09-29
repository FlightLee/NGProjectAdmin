//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Entity.Base;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace NGProjectAdmin.Entity.BusinessDTO.BusinessModule
{
    /// <summary>
    /// BizUserModuleDTO
    /// </summary>
    public class BizUserModuleDTO : NGAdminBaseEntity
    {
        /// <summary>
        /// 模块编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "MODULE_ID")]
        public Guid ModuleId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [SugarColumn(ColumnName = "USER_ID")]
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户所在模块登录账号
        /// </summary>
        [Required]
        [MaxLength(128)]
        [SugarColumn(ColumnName = "USER_MODULE_LOGON_NAME")]
        public String UserModuleLogonName { get; set; }

        /// <summary>
        /// 用户所在模块登录密码
        /// </summary>
        [Required]
        [MaxLength(512)]
        [SugarColumn(ColumnName = "USER_MODULE_PASSWORD")]
        public String UserModulePassword { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [SugarColumn(ColumnName = "USER_LOGON_NAME")]
        public String UserLogonName { get; set; }

        /// <summary>
        /// 用户展示名
        /// </summary>
        [SugarColumn(ColumnName = "USER_DISPLAY_NAME")]
        public String UserDisplayName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(ColumnName = "USER_PASSWORD")]
        public String UserPassword { get; set; }

        /// <summary>
        /// 座机
        /// </summary>
        [MaxLength(45)]
        [SugarColumn(ColumnName = "TELEPHONE")]
        public String Telephone { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [MaxLength(45)]
        [SugarColumn(ColumnName = "MOBILEPHONE")]
        public String MobilePhone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(45)]
        [SugarColumn(ColumnName = "EMAIL")]
        public String Email { get; set; }

        /// <summary>
        /// 是否启用
        /// 0：禁用；
        /// 1：启用
        /// </summary>
        [SugarColumn(ColumnName = "IS_ENABLED")]
        public int IsEnabled { get; set; }

        /// <summary>
        /// 性别
        /// 0：男
        /// 1：女
        /// 2：第三性别
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "SEX")]
        public int Sex { get; set; }
    }
}
