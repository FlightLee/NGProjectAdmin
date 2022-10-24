using NGProjectAdmin.Entity.Base;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace NGProjectAdmin.Entity.BusinessEntity.SystemManagement
{
    /// <summary>
    /// 多语菜单关系模型
    /// </summary>
    [SugarTable("sys_menu_language")]
    public class SysMenuLanguage : NGAdminBaseEntity
    {
        /// <summary>
        /// 菜单编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "MENU_ID")]
        public Guid MenuId { get; set; }

        /// <summary>
        /// 语言编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "LANGUAGE_ID")]
        public Guid LanguageId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [Required, MaxLength(128)]
        [SugarColumn(ColumnName = "MENU_NAME")]
        public String? MenuName { get; set; }
    }
}
