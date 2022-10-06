using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreDTO;
using NGProjectAdmin.Service.BusinessService.SystemManagement.CodeGeneratorService;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.AuthorizationFilter;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.SystemManagement
{
    [Authorize]
    [ActionAuthorization]
    [ApiController]
    [Route(NGAdminGlobalContext.RouteTemplate)]
    public class CodeGeneratorController : ControllerBase
    {
        #region 属性及其构造函数

        /// <summary>
        /// 代码生成器接口实例
        /// </summary>
        private readonly ICodeGeneratorService CodeGeneratorService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="CodeGeneratorService"></param>
        public CodeGeneratorController(ICodeGeneratorService CodeGeneratorService)
        {
            this.CodeGeneratorService = CodeGeneratorService;
        }

        #endregion

        #region 获取表空间信息

        /// <summary>
        /// 获取表空间信息
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        public async Task<IActionResult> Post()
        {
            var list = await this.CodeGeneratorService.GetSchemaInfo();
            var actionResult = Entity.CoreEntity.QueryResult<DbSchemaInfoDTO>.Success(list.Count, list);
            return Ok(actionResult);
        }

        #endregion

        #region 生成代码

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="codeGenerator">生成器对象</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.GenerateCode)]
        public async Task<IActionResult> CodeGenerate([FromBody] CodeGeneratorDTO codeGenerator)
        {
            var result = await this.CodeGeneratorService.CodeGenerate(codeGenerator);
            var actionResult = Entity.CoreEntity.ActionResult.Success(result);
            return Ok(actionResult);
        }

        #endregion
    }
}
