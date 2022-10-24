//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Entity.Base;
using NGProjectAdmin.Entity.CoreEnum;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace NGProjectAdmin.Entity.BusinessEntity.SystemManagement
{
    /// <summary>
    /// 导入详细配置模型
    /// </summary>
    [SugarTable("sys_import_config_detail")]
    public class SysImportConfigDetail : NGAdminBaseEntity
    {
        /// <summary>
        /// 父键
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "PARENT_ID")]
        public Guid ParentId { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "DATA_TYPE")]
        public CellDataType DataType { get; set; }

        /// <summary>
        /// 所在列
        /// </summary>
        [Required, MaxLength(512)]
        [SugarColumn(ColumnName = "CELLS")]
        public String? Cells { get; set; }

        /// <summary>
        /// 是否必填项
        /// 0：否
        /// 1：是
        /// </summary>
        [SugarColumn(ColumnName = "REQUIRED")]
        public Nullable<int> Required { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        [SugarColumn(ColumnName = "MAX_VALUE")]
        public Nullable<Double> MaxValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        [SugarColumn(ColumnName = "MIN_VALUE")]
        public Nullable<Double> MinValue { get; set; }

        /// <summary>
        /// 小数位上限
        /// </summary>
        [SugarColumn(ColumnName = "DECIMAL_LIMIT")]
        public Nullable<int> DecimalLimit { get; set; }

        /// <summary>
        /// 枚举列表
        /// </summary>
        [SugarColumn(ColumnName = "TEXT_ENUM")]
        public String? TextEnum { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        [SugarColumn(ColumnName = "EXTEND1")]
        public String? Extend1 { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        [SugarColumn(ColumnName = "EXTEND2")]
        public String? Extend2 { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        [SugarColumn(ColumnName = "EXTEND3")]
        public String? Extend3 { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(ColumnName = "SERIAL_NUMBER")]
        public Nullable<int> SerialNumber { get; set; }
    }
}
