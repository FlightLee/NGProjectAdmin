
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.AttachmentRepository;
using NGProjectAdmin.Service.Base;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.AttachmentService
{
    /// <summary>
    /// 业务附件服务层实现
    /// </summary>
    public class AttachmentService : NGAdminBaseService<SysAttachment>, IAttachmentService
    {
        #region 属性及构造函数

        /// <summary>
        /// 业务附件仓储实例
        /// </summary>
        private readonly IAttachmentRepository attachmentRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="attachmentRepository"></param>
        public AttachmentService(IAttachmentRepository attachmentRepository) : base(attachmentRepository)
        {
            this.attachmentRepository = attachmentRepository;
        }

        #endregion

        #region 系统文件统计

        /// <summary>
        /// 系统文件统计（MB）
        /// </summary>
        /// <returns>ActionResult</returns>
        public Task<ActionResult> QuerySysFileStatisticalInfo()
        {
            return Task.Run(() =>
            {
                //临时文件
                var tempFileLength = NGFileContext.GetFileNames(NGAdminGlobalContext.DirectoryConfig.GetTempPath()).Length;
                var tempFileSize = NGFileContext.GetDirectoryFileSizeByMB(NGAdminGlobalContext.DirectoryConfig.GetTempPath());

                //审计日志
                var monitoringLogLength = NGFileContext.GetFileNames(NGAdminGlobalContext.DirectoryConfig.GetMonitoringLogsPath()).Length;
                var monitoringLogSize = NGFileContext.GetDirectoryFileSizeByMB(NGAdminGlobalContext.DirectoryConfig.GetMonitoringLogsPath());

                //业务附件
                var businessAttachmentLength = NGFileContext.GetFileNames(NGAdminGlobalContext.DirectoryConfig.GetBusinessAttachmentPath()).Length;
                var businessAttachmentSize = NGFileContext.GetDirectoryFileSizeByMB(NGAdminGlobalContext.DirectoryConfig.GetBusinessAttachmentPath());

                var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RuYiAdminLogs/");

                //Debug文件
                var debugLogLength = NGFileContext.GetFileNames(logPath, "Debug*.log", true).Length;
                var debugLogSize = NGFileContext.GetDirectoryFileSizeByMB(logPath, "Debug*.log", true);

                //Error文件
                var errorLogLength = NGFileContext.GetFileNames(logPath, "Error*.log", true).Length;
                var errorLogSize = NGFileContext.GetDirectoryFileSizeByMB(logPath, "Error*.log", true);

                //Info文件
                var infoLogLength = NGFileContext.GetFileNames(logPath, "Info*.log", true).Length;
                var infoLogSize = NGFileContext.GetDirectoryFileSizeByMB(logPath, "Info*.log", true);

                //Warn文件
                var warnLogLength = NGFileContext.GetFileNames(logPath, "Warn*.log", true).Length;
                var warnLogSize = NGFileContext.GetDirectoryFileSizeByMB(logPath, "Warn*.log", true);

                var obj = new
                {
                    TempFilesLength = tempFileLength,
                    TempFileSize = tempFileSize,
                    MonitoringLogLength = monitoringLogLength,
                    MonitoringLogSize = monitoringLogSize,
                    BusinessAttachmentLength = businessAttachmentLength,
                    BusinessAttachmentSize = businessAttachmentSize,
                    DebugLogLength = debugLogLength,
                    DebugLogSize = debugLogSize,
                    ErrorLogLength = errorLogLength,
                    ErrorLogSize = errorLogSize,
                    InfoLogLength = infoLogLength,
                    InfoLogSize = infoLogSize,
                    WarnLogLength = warnLogLength,
                    WarnLogSize = warnLogSize
                };

                return ActionResult.Success(obj);
            });
        }

        #endregion
    }
}
