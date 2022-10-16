using NGProjectAdmin.Common.Class.Excel;
using NGProjectAdmin.Entity.Base;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace NGProjectAdmin.Entity.BusinessEntity.SystemManagement
{
    /// <summary>
    /// 角色模型
    /// </summary>
    [SugarTable("sys_role")]
    public class SysRole : NGAdminBaseEntity
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required, MaxLength(128)]
        [SugarColumn(ColumnName = "ROLE_NAME")]
        [ExcelExport("角色名称")]
        [ExcelImport("角色名称")]
        public String RoleName { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [ExcelExport("排序")]
        [ExcelImport("排序")]
        [SugarColumn(ColumnName = "SERIAL_NUMBER")]
        public Nullable<int> SerialNumber { get; set; }

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
