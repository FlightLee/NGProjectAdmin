using NGProjectAdmin.Common.Global;
using SqlSugar;
using System;
using System.Linq;

namespace NGProjectAdmin.Entity.CoreEntity
{
    /// <summary>
    /// 字段模型
    /// </summary>
    public class DbSchemaField
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        [SugarColumn(ColumnName = "COLUMN_NAME")]
        public String FieldName { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        [SugarColumn(ColumnName = "DATA_TYPE")]
        public String FieldDataType { get; set; }

        /// <summary>
        /// 字段注释
        /// </summary>
        [SugarColumn(ColumnName = "COLUMN_COMMENT")]
        public String FieldComment { get; set; }

        /// <summary>
        /// 是否为空
        /// </summary>
        [SugarColumn(ColumnName = "IS_NULLABLE")]
        public String IsNullable { get; set; }

        /// <summary>
        /// 最大长度
        /// </summary>
        [SugarColumn(ColumnName = "CHARACTER_MAXIMUM_LENGTH")]
        public String FieldMaxLength { get; set; }

        /// <summary>
        /// 判断是否忽略
        /// </summary>
        /// <returns></returns>
        public bool IsFieldIgnoreCase()
        {
            var arr = NGAdminGlobalContext.CodeGeneratorConfig.FieldsIgnoreCase.Split(',');

            if (arr.Contains(this.FieldName))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取数据类型
        /// </summary>
        /// <returns>转译类型</returns>
        public String GetDataType()
        {
            if (this.FieldDataType.Contains("("))
            {
                if (this.FieldDataType.Contains("number"))
                {
                    this.FieldDataType = "number(";
                }
                else
                {
                    this.FieldDataType = this.FieldDataType.Split('(')[0];
                }
            }

            if (this.FieldDataType.Equals("varchar") && this.FieldMaxLength.Equals("36"))
            {
                return "Guid";
            }

            switch (this.FieldDataType)
            {
                case "tinyint":
                case "smallint":
                case "mediumint":
                case "int":
                case "integer":
                case "number":
                case "int4":
                    return "int";

                case "double":
                case "number(":
                    return "Double";

                case "float":
                case "float8":
                    return "float";

                case "decimal":
                case "numeric":
                case "real":
                    return "decimal";

                case "bit":
                    return "bool";

                case "date":
                case "time":
                case "year":
                case "datetime":
                case "timestamp":
                case "datetime2":
                    return "DateTime";

                case "tinyblob":
                case "blob":
                case "mediumblob":
                case "longblob":
                case "binary":
                case "varbinary":
                case "bytea":
                    return "byte[]";

                case "char":
                case "varchar":
                case "nvarchar2":
                case "tinytext":
                case "text":
                case "mediumtext":
                case "longtext":
                case "clob":
                case "nvarchar":
                    return "String";

                case "uuid":
                case "uniqueidentifier":
                    return "Guid";

                case "point":
                case "linestring":
                case "polygon":
                case "geometry":
                case "multipoint":
                case "multilinestring":
                case "multipolygon":
                case "geometrycollection":
                case "enum":
                case "set":

                default:
                    return String.Empty;
            }
        }
    }
}
