using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NGProjectAdmin.Repository.Base
{
    public class NGAdminDbScope
    {        
        /// <summary>
        /// 单例模式对象
        /// </summary>
        public static SqlSugarScope NGDbContext = new SqlSugarScope(new ConnectionConfig()
        {
            DbType = (DbType)NGAdminGlobalContext.DBConfig.DBType,
            InitKeyType = InitKeyType.Attribute,
            IsAutoCloseConnection = true,
            ConnectionString = NGAdminGlobalContext.DBConfig.ConnectionString,//主库
            ConfigureExternalServices = new ConfigureExternalServices()
            {
                EntityService = (property, column) =>
                {
                    var attributes = property.GetCustomAttributes(true);//get all attributes 

                    if (attributes.Any(it => it is KeyAttribute))// by attribute set primarykey
                    {
                        column.IsPrimarykey = true; //有哪些特性可以看 1.2 特性明细
                    }
                    //可以写多个，这边可以断点调试
                    if (attributes.Any(it => it is NotMappedAttribute))
                    {
                        column.IsIgnore = true;
                    }
                }
                
            },
                MoreSettings = new ConnMoreSettings()
            {
                //MySql禁用NVarchar
                DisableNvarchar = (DbType)NGAdminGlobalContext.DBConfig.DBType == DbType.MySql ? true : false
            },
            //读写分离从库，如果使用MyCat搭建读写分离集群，请注释以下从库配置
            SlaveConnectionConfigs = new List<SlaveConnectionConfig>() {
                     new SlaveConnectionConfig() { HitRate=10, ConnectionString=NGAdminGlobalContext.DBConfig.SlaveConnectionString } ,
                     new SlaveConnectionConfig() { HitRate=10, ConnectionString=NGAdminGlobalContext.DBConfig.SlaveConnectionString2 }
                    }
        },
         db =>
         {
             //单例参数配置，所有上下文生效
             db.Ado.CommandTimeOut = NGAdminGlobalContext.DBConfig.CommandTimeOut;//数据库超时时间，单位秒

             //SQL执行前回调函数
             db.Aop.OnLogExecuting = (sql, pars) =>
             {
                 //执行前可以输出SQL
                 Console.WriteLine(sql);

             };

             //修改SQL和参数的值
             db.Aop.OnExecutingChangeSql = (sql, pars) =>
             {
                 if (sql.Contains("CreateTime"))
                 {
                     sql = sql.Replace("CreateTime", "CREATE_TIME");
                 }

                 return new KeyValuePair<string, SugarParameter[]>(sql, pars);
             };

             //SQL执行完回调函数
             db.Aop.OnLogExecuted = (sql, pars) =>
             {
                 //执行完可以输出SQL执行时间                 
                 NGLoggerContext.Info($"SQL:{sql},\r\nTimeSpan:{db.Ado.SqlExecutionTime.TotalMilliseconds}ms\r\n");                 
             };
         });
    }
}