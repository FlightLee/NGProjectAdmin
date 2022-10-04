
using Masuit.Tools.Core.Validator;
using Microsoft.AspNetCore.JsonPatch.Operations;
using NGProjectAdmin.Entity.Base;
using NGProjectAdmin.Entity.BusinessEnum;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;
using OperationType = NGProjectAdmin.Entity.BusinessEnum.OperationType;

namespace NGProjectAdmin.Entity.BusinessEntity.SystemManagement
{
    /// <summary>
    /// 审计日志模型
    /// </summary>
    [SugarTable("sys_log")]
    public class SysLog : NGAdminBaseEntity
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "USER_ID")]
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [Required, MaxLength(128)]
        [SugarColumn(ColumnName = "USER_NAME")]
        public String UserName { get; set; }

        /// <summary>
        /// 机构编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "ORG_ID")]
        public Guid OrgId { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        [Required, MaxLength(128)]
        [SugarColumn(ColumnName = "ORG_NAME")]
        public String OrgName { get; set; }

        /// <summary>
        /// 使用系统
        /// </summary>
        [SugarColumn(ColumnName = "SYSTEM")]
        public String System { get; set; }

        /// <summary>
        /// 使用浏览器
        /// </summary>
        [SugarColumn(ColumnName = "BROWSER")]
        public String Browser { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [IsIPAddress]
        [SugarColumn(ColumnName = "IP")]
        public String IP { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "OPERATION_TYPE")]
        public OperationType OperationType { get; set; }

        /// <summary>
        /// 请求路径
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "REQUEST_METHOD")]
        public String RequestMethod { get; set; }

        /// <summary>
        /// 请求路径
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "REQUEST_URL")]
        public String RequestUrl { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        [SugarColumn(ColumnName = "PARAMS")]
        public String Params { get; set; }

        /// <summary>
        /// 返回值
        /// </summary>
        [SugarColumn(ColumnName = "RESULT")]
        public String Result { get; set; }

        /// <summary>
        /// 记录旧值
        /// </summary>
        [SugarColumn(ColumnName = "OLD_VALUE")]
        public String OldValue { get; set; }

        /// <summary>
        /// 记录新值
        /// </summary>
        [SugarColumn(ColumnName = "NEW_VALUE")]
        public String NewValue { get; set; }
    }
}
