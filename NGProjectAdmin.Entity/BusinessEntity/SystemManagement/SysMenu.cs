//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Entity.Base;
using NGProjectAdmin.Entity.BusinessEnum;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace NGProjectAdmin.Entity.BusinessEntity.SystemManagement
{
    /// <summary>
    /// 菜单模型
    /// </summary>
    [SugarTable("sys_menu")]
    public class SysMenu : NGAdminBaseEntity
    {
        /// <summary>
        /// 父路径
        /// </summary>
        [MaxLength(256)]
        [SugarColumn(ColumnName = "PATH")]
        public String Path { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [Required, MaxLength(128)]
        [SugarColumn(ColumnName = "MENU_NAME")]
        public String MenuName { get; set; }

        /// <summary>
        /// 菜单路径
        /// </summary>
        [MaxLength(256)]
        [SugarColumn(ColumnName = "MENU_URL")]
        public String MenuUrl { get; set; }

        /// <summary>
        /// 父键
        /// </summary>
        [SugarColumn(ColumnName = "PARENT_ID")]
        public Nullable<Guid> ParentId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(ColumnName = "SERIAL_NUMBER")]
        public Nullable<int> SerialNumber { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "MENU_TYPE")]
        public MenuType MenuType { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [MaxLength(256)]
        [SugarColumn(ColumnName = "ICON")]
        public String Icon { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [MaxLength(125)]
        [SugarColumn(ColumnName = "CODE")]
        public String Code { get; set; }

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
