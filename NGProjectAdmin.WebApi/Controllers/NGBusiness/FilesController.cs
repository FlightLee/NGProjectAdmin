using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.BusinessService.NGBusiness;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NGProjectAdmin.WebApi.Controllers.NGBusiness
{
    /// <summary>
    /// 文件管理
    /// </summary>
    public class FilesController : BaseController<File_group>
    {
        private readonly IFile_groupService File_groupService;

        private readonly IFile_detailService File_detailService;

        private readonly IHttpContextAccessor context;
        public FilesController(IFile_groupService file_groupService, IHttpContextAccessor context, IFile_detailService file_detailService) : base(file_groupService)
        {
            this.File_groupService = file_groupService;
            this.context = context;
            this.File_detailService = file_detailService;
        }
        /// <summary>
        /// 上传文件接口
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        [HttpPost]
        [Log(OperationType.UploadFile)]
        public async Task<IActionResult> UploadFiles(int businessId)
        {
            File_group f = new File_group();
            f.optype=businessId;
            List<File_detail> list = new List<File_detail>();
            int i = 0;
            var res = await this.File_groupService.AddAsync(f, true);
            foreach (var item in this.context.HttpContext.Request.Form.Files)
            {
                i++;
                var file = new File_detail();
                file.Create(this.context);
                file.FileName= item.FileName;
                file.FileId = f.Id.ToString();
                file.FileUrl= NGFileContext.SaveBusinessAttachment(item, file.Id.ToString());
                file.insideId = i;               
                list.Add(file);
            }
            var actionResult = await this.File_detailService.AddListAsync(list, false);
            return Ok(actionResult);
        }
    }
}
