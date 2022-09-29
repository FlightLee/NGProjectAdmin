//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using SqlSugar;
using System;

namespace NGProjectAdmin.Entity.BusinessDTO.SystemManagement
{
    /// <summary>
    /// 系统角色DTO
    /// </summary>
    public class SysRoleDTO : SysRole
    {
        /// <summary>
        /// 机构编号
        /// </summary>
        [SugarColumn(ColumnName = "ORG_ID")]
        public Guid OrgId { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        public String OrgName { get; set; }
    }
}
