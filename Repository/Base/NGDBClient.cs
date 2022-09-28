using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Repository.Base
{
	public class NGDBClient
	{
		/// <summary>
		/// 单例模式对象
		/// </summary>
		public static SqlSugarScope RuYiDbContext = new SqlSugarScope(new ConnectionConfig()
		{
			DbType = (DbType)RuYiAdminGlobalContext.DBConfig.DBType,
			InitKeyType = InitKeyType.Attribute,
			IsAutoCloseConnection = true,
			ConnectionString = RuYiAdminGlobalContext.DBConfig.ConnectionString,//主库
			MoreSettings = new ConnMoreSettings()
			{
				//MySql禁用NVarchar
				DisableNvarchar = (DbType)RuYiAdminGlobalContext.DBConfig.DBType == DbType.MySql ? true : false
			},
			//读写分离从库，如果使用MyCat搭建读写分离集群，请注释以下从库配置
			SlaveConnectionConfigs = new List<SlaveConnectionConfig>() {
					 new SlaveConnectionConfig() { HitRate=10, ConnectionString=RuYiAdminGlobalContext.DBConfig.SlaveConnectionString } ,
					 new SlaveConnectionConfig() { HitRate=10, ConnectionString=RuYiAdminGlobalContext.DBConfig.SlaveConnectionString2 }
					}
		},
		 db =>
		 {
			 //单例参数配置，所有上下文生效
			 db.Ado.CommandTimeOut = RuYiAdminGlobalContext.DBConfig.CommandTimeOut;//数据库超时时间，单位秒

			 //SQL执行前回调函数
			 db.Aop.OnLogExecuting = (sql, pars) =>
			 {
				 //执行前可以输出SQL
				 //Console.WriteLine(sql);
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
				 Console.Write($"SQL:{sql},\r\nTimeSpan:{db.Ado.SqlExecutionTime.TotalMilliseconds}ms\r\n");
			 };
		 });
	}
}
