using NGProjectAdmin.Entity.Base;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace NGProjectAdmin.Entity.BusinessEntity.SystemManagement
{
    /// <summary>
    /// 机构模型
    /// </summary>
    [SugarTable("sys_organization")]
    public class SysOrganization : NGAdminBaseEntity
    {
        /// <summary>
        /// 机构名称
        /// </summary>
        [Required, MaxLength(128)]
        [SugarColumn(ColumnName = "ORG_NAME")]
        public String? OrgName { get; set; }

        /// <summary>
        /// 父键
        /// </summary>
        [SugarColumn(ColumnName = "PARENT_ID")]
        public Nullable<Guid> ParentId { get; set; }

        /// <summary>
        /// 主管人
        /// </summary>
        [SugarColumn(ColumnName = "LEADER")]
        public Nullable<Guid> Leader { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(ColumnName = "SERIAL_NUMBER")]
        public int SerialNumber { get; set; }

        /// <summary>
        /// 预留字段1
        /// </summary>
        [SugarColumn(ColumnName = "EXTEND1")]
        public String? Extend1 { get; set; }

        /// <summary>
        /// 预留字段2
        /// </summary>
        [SugarColumn(ColumnName = "EXTEND2")]
        public String? Extend2 { get; set; }

        /// <summary>
        /// 预留字段3
        /// </summary>
        [SugarColumn(ColumnName = "EXTEND3")]
        public String? Extend3 { get; set; }

        /// <summary>
        /// 预留字段4
        /// </summary>
        [SugarColumn(ColumnName = "EXTEND4")]
        public String? Extend4 { get; set; }

        /// <summary>
        /// 预留字段5
        /// </summary>
        [SugarColumn(ColumnName = "EXTEND5")]
        public String? Extend5 { get; set; }
    }
}
