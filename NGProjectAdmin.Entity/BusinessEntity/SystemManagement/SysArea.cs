using NGProjectAdmin.Entity.Base;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace NGProjectAdmin.Entity.BusinessEntity.SystemManagement
{
    /// <summary>
    /// 行政区域模型
    /// </summary>
    [SugarTable("sys_area")]
    public class SysArea : NGAdminBaseEntity
    {
        /// <summary>
        /// 地区编码
        /// </summary>
        [Required, MaxLength(6)]
        [SugarColumn(ColumnName = "AREA_CODE")]
        public String AreaCode { get; set; }

        /// <summary>
        /// 父地区编码
        /// </summary>
        [Required, MaxLength(6)]
        [SugarColumn(ColumnName = "PARENT_AREA_CODE")]
        public String ParentAreaCode { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        [Required, MaxLength(50)]
        [SugarColumn(ColumnName = "AREA_NAME")]
        public String AreaName { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        [Required, MaxLength(50)]
        [SugarColumn(ColumnName = "ZIP_CODE")]
        public String ZipCode { get; set; }

        /// <summary>
        /// 地区层级(1省份 2城市 3区县)
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "AREA_LEVEL")]
        public int AreaLevel { get; set; }
    }
}
