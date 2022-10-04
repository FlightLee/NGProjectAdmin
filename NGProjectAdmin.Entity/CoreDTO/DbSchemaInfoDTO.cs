using NGProjectAdmin.Entity.CoreEntity;
using SqlSugar;
using System;

namespace NGProjectAdmin.Entity.CoreDTO
{
    /// <summary>
    /// 数据库模型信息
    /// </summary>
    public class DbSchemaInfoDTO : DbSchemaField
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        [SugarColumn(ColumnName = "TABLE_SCHEMA")]
        public String SchemaName { get; set; }

        /// <summary>
        /// 表名称
        /// </summary>
        [SugarColumn(ColumnName = "TABLE_NAME")]
        public String TableName { get; set; }

        /// <summary>
        /// 表注释
        /// </summary>
        [SugarColumn(ColumnName = "TABLE_COMMENT")]
        public String TableComment { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_TIME")]
        public DateTime CreateTime { get; set; }
    }
}
