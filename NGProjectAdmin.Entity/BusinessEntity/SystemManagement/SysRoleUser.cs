using NGProjectAdmin.Entity.Base;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace NGProjectAdmin.Entity.BusinessEntity.SystemManagement
{
    /// <summary>
    /// 角色用户关系模型
    /// </summary>
    [SugarTable("sys_role_user")]
    public class SysRoleUser : NGAdminBaseEntity
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "ROLE_ID")]
        public Guid RoleId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "USER_ID")]
        public Guid UserId { get; set; }
    }
}
