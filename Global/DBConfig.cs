
using System;
namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DBConfig
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public int DBType { get; set; }

        /// <summary>
        /// 数据库连接串
        /// </summary>
        public String ConnectionString { get; set; } = String.Empty;

        /// <summary>
        /// 数据库超时时间
        /// </summary>
        public int CommandTimeOut { get; set; }

        /// <summary>
        /// 数据库备份路径
        /// </summary>
        public String BackupPath { get; set; } = String.Empty;

        /// <summary>
        /// 读写分离从库连接串
        /// </summary>
        public String SlaveConnectionString { get; set; } = String.Empty;

        /// <summary>
        /// 读写分离从库2连接串
        /// </summary>
        public String SlaveConnectionString2 { get; set; } = String.Empty;

        /// <summary>
        /// 是否自动构建数据库
        /// </summary>
        public bool AutomaticallyBuildDatabase { get; set; }

        /// <summary>
        /// 脚本路径
        /// </summary>
        public String SqlScriptPath { get; set; } = String.Empty;
    }
}
