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
    /// 系统附件模型
    /// </summary>
    [SugarTable("sys_attachment")]

    public class SysAttachment : NGAdminBaseEntity
    {
        /// <summary>
        /// 附件名称
        /// </summary>
        [Required, MaxLength(1024)]
        [SugarColumn(ColumnName = "FILE_NAME")]
        public String FileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "FILE_SIZE")]
        public Double FileSize { get; set; }

        /// <summary>
        /// 存储路径
        /// </summary>
        [Required, MaxLength(512)]
        [SugarColumn(ColumnName = "FILE_PATH")]
        public String FilePath { get; set; }

        /// <summary>
        /// 业务编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "BUSINESS_ID")]
        public Guid BusinessId { get; set; }
    }
}
