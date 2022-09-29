//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Common.Class.Excel;
using NGProjectAdmin.Entity.Base;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace NGProjectAdmin.Entity.BusinessEntity.SystemManagement
{
    /// <summary>
    /// 用户模型
    /// </summary>
    [SugarTable("sys_user")]
    public class SysUser : NGAdminBaseEntity
    {
        /// <summary>
        /// 登录名称
        /// </summary>
        [Required, MaxLength(128)]
        [SugarColumn(ColumnName = "LOGON_NAME")]
        [ExcelExport("用户账户")]
        public String LogonName { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [Required, MaxLength(128)]
        [SugarColumn(ColumnName = "DISPLAY_NAME")]
        [ExcelExport("用户姓名")]
        public String DisplayName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [Required, MaxLength(512)]
        [SugarColumn(ColumnName = "PASSWORD")]
        public String Password { get; set; }

        /// <summary>
        /// 座机号码
        /// </summary>
        [MaxLength(45)]
        [SugarColumn(ColumnName = "TELEPHONE")]
        [ExcelExport("座机号")]
        public String Telephone { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [MaxLength(45)]//, IsPhone
        [SugarColumn(ColumnName = "MOBILEPHONE")]
        [ExcelExport("手机号")]
        public String MobilePhone { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        [MaxLength(45)]//, IsEmail
        [SugarColumn(ColumnName = "EMAIL")]
        [ExcelExport("电子邮件")]
        public String Email { get; set; }

        /// <summary>
        /// 是否超级管理员
        /// 0：否
        /// 1：是
        /// </summary>
        [SugarColumn(ColumnName = "IS_SUPPER_ADMIN")]
        public int IsSupperAdmin { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(ColumnName = "SERIAL_NUMBER")]
        [ExcelExport("排序")]
        public int SerialNumber { get; set; }

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
        [ExcelExport("性别", "0:男,1:女,2:第三性别")]
        [ExcelImport("性别", "男:0,女:1,第三性别:2")]
        public int Sex { get; set; }

        /// <summary>
        /// 盐
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "SALT")]
        public Guid Salt { get; set; }

        /// <summary>
        /// 预留字段1
        /// </summary>
        [SugarColumn(ColumnName = "EXTEND1")]
        public String Extend1 { get; set; }

        /// <summary>
        /// 预留字段2
        /// </summary>
        [SugarColumn(ColumnName = "EXTEND2")]
        public String Extend2 { get; set; }

        /// <summary>
        /// 预留字段3
        /// </summary>
        [SugarColumn(ColumnName = "EXTEND3")]
        public String Extend3 { get; set; }

        /// <summary>
        /// 预留字段4
        /// </summary>
        [SugarColumn(ColumnName = "EXTEND4")]
        public String Extend4 { get; set; }

        /// <summary>
        /// 预留字段5
        /// </summary>
        [SugarColumn(ColumnName = "EXTEND5")]
        public String Extend5 { get; set; }
    }
}
