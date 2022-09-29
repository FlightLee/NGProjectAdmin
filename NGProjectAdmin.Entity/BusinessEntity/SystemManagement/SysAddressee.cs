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
    /// 收件人模型
    /// </summary>
    [SugarTable("sys_addressee")]
    public class SysAddressee : NGAdminBaseEntity
    {
        /// <summary>
        /// 业务编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "BUSINESS_ID")]
        public Guid BusinessId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "USER_ID")]
        public Guid UserId { get; set; }

        /// <summary>
        /// 状态，0：未读，1：已读
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "STATUS")]
        public int Status { get; set; }
    }
}
