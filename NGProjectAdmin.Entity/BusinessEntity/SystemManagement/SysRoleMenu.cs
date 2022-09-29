//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Entity.Base;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace NGProjectAdmin.Entity.BusinessEntity.SystemManagement
{
    /// <summary>
    /// 角色菜单关系模型
    /// </summary>
    [SugarTable("sys_role_menu")]
    public class SysRoleMenu : NGAdminBaseEntity
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "ROLE_ID")]
        public Guid RoleId { get; set; }

        /// <summary>
        /// 菜单编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "MENU_ID")]
        public Guid MenuId { get; set; }
    }
}
