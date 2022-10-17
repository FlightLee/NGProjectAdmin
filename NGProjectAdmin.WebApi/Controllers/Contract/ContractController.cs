using Masuit.Tools.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NGProjectAdmin.Entity.BusinessDTO.File;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.Base;
using NGProjectAdmin.Service.Base;
using NGProjectAdmin.Service.BusinessService.File;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NGProjectAdmin.WebApi.Controllers.Contract
{
    /// <summary>
    /// 合同控制器
    /// </summary>
    public class ContractController : BaseController<File_group>
    {

        #region 属性及构造函数

        /// <summary>
        /// 业务模块接口实例
        /// </summary>
        private readonly IFile_groupService File_groupService;

        private readonly IFile_detailService File_detailService;

        /// <summary>
        /// 合同控制器
        /// </summary>
        /// <param name="File_groupService"></param>      
        /// <param name="file_detailService"></param>      
        public ContractController(IFile_groupService File_groupService, IFile_detailService file_detailService) : base(File_groupService)
        {
            this.File_groupService = File_groupService;
            File_detailService = file_detailService;
        }
        #endregion
        /// <summary>
        /// 查询文件信息
        /// </summary>
        /// <param name="FileId">查询条件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(int FileId)
        {
             var actionResult = await this.File_groupService.GetByIdAsync(FileId);

            //var actionResult2 = NGAdminDbScope.NGDbContext.Queryable<File_group>().Where(x => x.Id == FileId).ToList();
            //var actionResult = await this.userService.Logon(loginDTO);
            return Ok(actionResult);
        }
    }
}
