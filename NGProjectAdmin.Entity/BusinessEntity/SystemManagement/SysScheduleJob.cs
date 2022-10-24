using NGProjectAdmin.Entity.Base;
using NGProjectAdmin.Entity.BusinessEnum;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace NGProjectAdmin.Entity.BusinessEntity.SystemManagement
{
    /// <summary>
    /// 计划任务实体模型
    /// </summary>
    [SugarTable("sys_schedule_job")]
    public class SysScheduleJob : NGAdminBaseEntity
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        [Required, MaxLength(128)]
        [SugarColumn(ColumnName = "JOB_NAME")]
        public String? JobName { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        [MaxLength(512)]
        [SugarColumn(ColumnName = "JOB_DESCRIPTION")]
        public String? JobDescription { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        [Required, MaxLength(512)]
        [SugarColumn(ColumnName = "NAMESPACE")]
        public String? NameSpace { get; set; }

        /// <summary>
        /// 实现类
        /// </summary>
        [Required, MaxLength(128)]
        [SugarColumn(ColumnName = "JOB_IMPLEMENT")]
        public String? JobImplement { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        [Required, MaxLength(128)]
        [SugarColumn(ColumnName = "CRON_EXPRESSION")]
        public String? CronExpression { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [SugarColumn(ColumnName = "START_TIME")]
        public Nullable<DateTime> StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [SugarColumn(ColumnName = "END_TIME")]
        public Nullable<DateTime> EndTime { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "JOB_STATUS")]
        public JobStatus JobStatus { get; set; }

        /// <summary>
        /// 集群编号
        /// </summary>
        [SugarColumn(ColumnName = "GROUP_ID")]
        public Nullable<int> GroupId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(ColumnName = "SERIAL_NUMBER")]
        public Nullable<int> SerialNumber { get; set; }
    }
}
