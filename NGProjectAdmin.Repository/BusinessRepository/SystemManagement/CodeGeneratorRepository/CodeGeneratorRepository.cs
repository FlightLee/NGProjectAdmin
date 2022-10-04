using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Entity.CoreDTO;
using NGProjectAdmin.Repository.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.CodeGeneratorRepository
{
    /// <summary>
    /// 代码生成器数据访问层实现
    /// </summary>
    public class CodeGeneratorRepository : NGAdminDbScope, ICodeGeneratorRepository
    {
        #region 获取表名称列表

        /// <summary>
        /// 获取表名称列表
        /// </summary>
        /// <returns>表名称列表</returns>
        public async Task<List<DbSchemaInfoDTO>> GetSchemaInfo()
        {
            var fields = new List<DbSchemaInfoDTO>();

            var sqlKey = String.Empty;
            if ((DbType)NGAdminGlobalContext.DBConfig.DBType == DbType.MySql)
            {
                sqlKey = "sqls:sql:query_mysql_schema_info";
            }
            else if ((DbType)NGAdminGlobalContext.DBConfig.DBType == DbType.SqlServer)
            {
                sqlKey = "sqls:sql:query_sqlserver_schema_info";
            }
            else if ((DbType)NGAdminGlobalContext.DBConfig.DBType == DbType.Oracle)
            {
                sqlKey = "sqls:sql:query_oracle_schema_info";
            }
            else if ((DbType)NGAdminGlobalContext.DBConfig.DBType == DbType.PostgreSQL)
            {
                sqlKey = "sqls:sql:query_postgresql_schema_info";
            }
            else
            {
                throw new NGAdminCustomException("temporarily not supported");
            }

            var strSQL = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value;
            fields = (await NGDbContext.SqlQueryable<DbSchemaInfoDTO>(strSQL).ToListAsync()).ToList();

            return fields;
        }

        #endregion

        #region 获取表的列表

        /// <summary>
        /// 获取表的列表
        /// </summary>
        /// <param name="tables">表名</param>
        /// <returns>表的列表集</returns>
        public async Task<List<DbSchemaInfoDTO>> GetSchemaFieldsInfo(String tables)
        {
            var sqlKey = String.Empty;
            if ((DbType)NGAdminGlobalContext.DBConfig.DBType == DbType.MySql)
            {
                sqlKey = "sqls:sql:query_mysql_schema_column_info";
            }
            else if ((DbType)NGAdminGlobalContext.DBConfig.DBType == DbType.SqlServer)
            {
                sqlKey = "sqls:sql:query_sqlserver_schema_column_info";
            }
            else if ((DbType)NGAdminGlobalContext.DBConfig.DBType == DbType.Oracle)
            {
                sqlKey = "sqls:sql:query_oracle_schema_column_info";
            }
            else if ((DbType)NGAdminGlobalContext.DBConfig.DBType == DbType.PostgreSQL)
            {
                sqlKey = "sqls:sql:query_postgresql_schema_column_info";
            }

            var strSQL = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value;
            strSQL = String.Format(strSQL, tables);

            var fields = (await NGDbContext.SqlQueryable<DbSchemaInfoDTO>(strSQL).ToListAsync()).ToList();

            return fields;
        }

        #endregion
    }
}
