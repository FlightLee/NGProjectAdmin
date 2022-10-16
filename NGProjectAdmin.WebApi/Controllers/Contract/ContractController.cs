using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Entity.BusinessEntity.File;
using NGProjectAdmin.Service.BusinessService.File;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System;
using System.Threading.Tasks;

namespace NGProjectAdmin.WebApi.Controllers.Contract
{
    /// <summary>
    /// 合同控制器
    /// </summary>
    public class ContractController : NGAdminBaseController<File_group>
    {

        #region 属性及构造函数

        /// <summary>
        /// 业务模块接口实例
        /// </summary>
        private readonly IFile_groupService File_groupService;

        /// <summary>
        /// 合同控制器
        /// </summary>
        /// <param name="File_groupService"></param>      
        public ContractController(IFile_groupService File_groupService) : base(File_groupService)
        {
            this.File_groupService = File_groupService;
        }
        #endregion
        /// <summary>
        /// 查询文件信息
        /// </summary>
        /// <param name="FileId">查询条件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(Guid FileId)
        {
            var actionResult = await this.File_groupService.GetByIdAsync(FileId);
            return Ok(actionResult);
        }
    }
}
