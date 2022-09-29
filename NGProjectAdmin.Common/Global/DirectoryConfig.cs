using NGProjectAdmin.Common.Utility;
using System;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// 目录配置
    /// </summary>
    public class DirectoryConfig
    {
        /// <summary>
        /// 模板目录
        /// </summary>
        public String TemplateDirectory { get; set; }

        /// <summary>
        /// 存储类型
        /// </summary>
        public String StorageType { get; set; }

        /// <summary>
        /// 存储路径
        /// </summary>
        public String StoragePath { get; set; }

        /// <summary>
        /// 临时目录
        /// </summary>
        public String TempPath { get; set; }

        /// <summary>
        /// 审计日志目录
        /// </summary>
        public String MonitoringLogsPath { get; set; }

        /// <summary>
        /// 业务附件目录
        /// </summary>
        public String BusinessAttachmentPath { get; set; }

        /// <summary>
        /// 获取模板路径
        /// </summary>
        /// <returns>模板路径</returns>
        public String GetTemplateDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "/" + this.TemplateDirectory;
        }

        /// <summary>
        /// 获取存储路径
        /// </summary>
        /// <returns>存储路径</returns>
        public String GetStoragePath()
        {
            if (this.StorageType.Equals("Relative"))
            {
                return AppDomain.CurrentDomain.BaseDirectory + "/" + this.StoragePath;
            }
            else if (this.StorageType.Equals("Absolute"))
            {
                return this.StoragePath;
            }

            return String.Empty;
        }

        /// <summary>
        /// 获取临时目录
        /// </summary>
        /// <returns>临时目录</returns>
        public String GetTempPath()
        {
            var path = this.GetStoragePath() + "/" + this.TempPath;
            NGFileContext.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// 清空临时目录
        /// </summary>
        public void CleanTempPath()
        {
            NGFileContext.ClearDirectory(this.GetTempPath());
        }

        /// <summary>
        /// 获取审计日志所在目录
        /// </summary>
        /// <returns>审计日志所在目录</returns>
        public String GetMonitoringLogsPath()
        {
            var path = this.GetStoragePath() + "/" + this.MonitoringLogsPath;
            NGFileContext.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// 获取业务附件目录
        /// </summary>
        /// <returns>业务附件目录</returns>
        public String GetBusinessAttachmentPath()
        {
            var path = this.GetStoragePath() + "/" + this.BusinessAttachmentPath;
            NGFileContext.CreateDirectory(path);
            return path;
        }
    }
}
