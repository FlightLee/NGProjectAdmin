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
    /// 多语模型
    /// </summary>
    [SugarTable("sys_language")]
    public class SysLanguage : NGAdminBaseEntity
    {
        /// <summary>
        /// 语言名称
        /// </summary>
        [Required, MaxLength(45)]
        [SugarColumn(ColumnName = "LANGUAGE_NAME")]
        public String LanguageName { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(ColumnName = "ORDER_NUMBER")]
        public Nullable<int> OrderNumber { get; set; }

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
