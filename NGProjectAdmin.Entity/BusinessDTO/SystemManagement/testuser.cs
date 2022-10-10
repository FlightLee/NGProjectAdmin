//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Common.Class.Excel;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.User;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace NGProjectAdmin.Entity.BusinessDTO.SystemManagement
{
    /// <summary>
    /// 用户DTO
    /// </summary>
    // public class SysUserDTO : UserBaseInfo
    public class testuser 
    {
        /// <summary>
        /// 登录名称
        /// </summary>
        [Required, MaxLength(128)]
        [SugarColumn(ColumnName = "LOGON_NAME")]
        [ExcelExport("用户账户")]
        public String LogonName { get; set; } = String.Empty;


    }
}
