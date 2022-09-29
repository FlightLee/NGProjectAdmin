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
    /// 机构用户关系模型
    /// </summary>
    [SugarTable("sys_org_user")]
    public class SysOrgUser : NGAdminBaseEntity
    {
        /// <summary>
        /// 机构编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "ORG_ID")]
        public Guid OrgId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "USER_ID")]
        public Guid UserId { get; set; }
    }
}
