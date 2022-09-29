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
    /// 导入主配置模型
    /// </summary>
    [SugarTable("sys_import_config")]
    public class SysImportConfig : NGAdminBaseEntity
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        [Required, MaxLength(128)]
        [SugarColumn(ColumnName = "CONFIG_NAME")]
        public String ConfigName { get; set; }

        /// <summary>
        /// 起始行
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "START_ROW")]
        public int StartRow { get; set; }

        /// <summary>
        /// 起始列
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "START_COLUMN")]
        public int StartColumn { get; set; }

        /// <summary>
        /// 工作簿索引列表
        /// </summary>
        [SugarColumn(ColumnName = "WORKSHEET_INDEXES")]
        public String WorkSheetIndexes { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(ColumnName = "SERIAL_NUMBER")]
        public Nullable<int> SerialNumber { get; set; }
    }
}
