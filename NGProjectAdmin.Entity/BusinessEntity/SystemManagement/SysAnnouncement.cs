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
    /// 通知公告模型
    /// </summary>
    [SugarTable("sys_announcement")]
    public class SysAnnouncement : NGAdminBaseEntity
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required, MaxLength(512)]
        [SugarColumn(ColumnName = "TITLE")]
        public String Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "CONTENT")]
        public String Content { get; set; }

        /// <summary>
        /// 类型，0：公告，1：通知
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "TYPE")]
        public int Type { get; set; }

        /// <summary>
        /// 状态，0：开放，1：关闭
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "STATUS")]
        public int Status { get; set; }
    }
}
