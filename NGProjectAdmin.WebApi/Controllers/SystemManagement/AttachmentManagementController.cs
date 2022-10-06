using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Entity.CoreEnum;
using NGProjectAdmin.Service.BusinessService.SystemManagement.AttachmentService;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 业务附件管理控制器
    /// </summary>
    public class AttachmentManagementController : NGAdminBaseController<SysAttachment>
    {
        #region 属性及构造函数

        /// <summary>
        /// 业务附件业务接口实例
        /// </summary>
        private readonly IAttachmentService attachmentService;

        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="attachmentService"></param>
        /// <param name="context"></param>
        public AttachmentManagementController(IAttachmentService attachmentService, IHttpContextAccessor context) : base(attachmentService)
        {
            this.attachmentService = attachmentService;
            this.context = context;
        }

        #endregion

        #region 上传业务附件

        /// <summary>
        /// 上传业务附件
        /// </summary>
        /// <param name="businessId">业务编号</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.UploadFile)]
        public async Task<IActionResult> UploadAttachments(Guid businessId)
        {
            var attachments = new List<SysAttachment>();

            foreach (var item in this.context.HttpContext.Request.Form.Files)
            {
                var attachment = new SysAttachment();

                attachment.Create(this.context);
                attachment.FileName = item.FileName;
                //处理文件大小
                String remark = String.Empty;
                attachment.FileSize = NGFileContext.GetFileSize(item, out remark);
                attachment.Remark = remark;
                //保存文件
                attachment.FilePath = NGFileContext.SaveBusinessAttachment(item, attachment.Id.ToString());
                //附件关联业务
                attachment.BusinessId = businessId;

                attachments.Add(attachment);
            }

            var actionResult = await this.attachmentService.AddListAsync(attachments, false);
            return Ok(actionResult);
        }

        #endregion

        #region 获取业务附件

        /// <summary>
        /// 获取业务附件
        /// </summary>
        /// <param name="businessId">业务编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{businessId}")]
        [Log(OperationType.QueryList)]
        public async Task<IActionResult> GetAttachments(Guid businessId)
        {
            var queryCondition = new QueryCondition();
            queryCondition.QueryItems = new List<QueryItem>();
            queryCondition.QueryItems.Add(new QueryItem()
            {
                Field = "BusinessId",
                DataType = DataType.Guid,
                QueryMethod = QueryMethod.Equal,
                Value = businessId.ToString()
            });
            //获取附件信息
            var actionResult = await this.attachmentService.GetListAsync(queryCondition);
            return Ok(actionResult);
        }

        #endregion

        #region 下载业务附件

        /// <summary>
        /// 下载业务附件
        /// </summary>
        /// <param name="attachmentId">附件编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{attachmentId}")]
        [Log(OperationType.DownloadFile)]
        public async Task<IActionResult> DownloadAttachment(Guid attachmentId)
        {
            //获取附件信息
            var task = await this.attachmentService.GetByIdAsync(attachmentId);
            var attachment = (SysAttachment)task.Object;
            //存储路径
            var filePath = attachment.FilePath;
            //文件读写流
            var stream = new FileStream(filePath, FileMode.Open);
            //设置流的起始位置
            stream.Position = 0;
            return File(stream, "application/octet-stream", attachment.FileName);
        }

        #endregion

        #region 删除业务附件

        /// <summary>
        /// 删除业务附件
        /// </summary>
        /// <param name="attachmentIds">数组串</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{attachmentIds}")]
        [Log(OperationType.DeleteEntity)]
        public async Task<IActionResult> DeleteAttachment(String attachmentIds)
        {
            var array = NGStringUtil.GetGuids(attachmentIds);
            var actionResult = await this.attachmentService.DeleteRangeAsync(array);
            //删除业务文件
            foreach (var item in array)
            {
                var attachment = this.attachmentService.GetById(item).Object as SysAttachment;
                NGFileContext.DeleteFile(attachment.FilePath);
            }
            return Ok(actionResult);
        }

        #endregion

        #region 系统文件统计

        /// <summary>
        /// 系统文件统计
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Log(OperationType.QueryEntity)]
        [Permission("attachment:query:list")]
        public async Task<IActionResult> GetSysFileStatisticalInfo()
        {
            //获取统计信息
            var actionResult = await this.attachmentService.QuerySysFileStatisticalInfo();
            return Ok(actionResult);
        }

        #endregion
    }
}
