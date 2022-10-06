
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.Base;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.Base;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.AuthorizationFilter;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NGProjectAdmin.WebApi.AppCode.FrameworkBase
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [Authorize]
    [ActionAuthorization]
    [ApiController]
    [Route(NGAdminGlobalContext.RouteTemplate)]
    public class NGAdminBaseController<T> : ControllerBase where T : NGAdminBaseEntity
    {
    

        #region 属性与构造函数

        /// <summary>
        /// 服务层基类实例
        /// </summary>
        private readonly INGAdminBaseService<T> NGAdminBaseService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="NGAdminBaseService"></param>
        public NGAdminBaseController(INGAdminBaseService<T> NGAdminBaseService)
        {
            this.NGAdminBaseService = NGAdminBaseService;
        }

        #endregion        

        #region 通用方法

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        public async Task<IActionResult> GetPage(QueryCondition queryCondition)
        {
            var actionResult = await this.NGAdminBaseService.GetPageAsync(queryCondition);
            return Ok(actionResult);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        public async Task<IActionResult> GetList(QueryCondition queryCondition)
        {
            var actionResult = await this.NGAdminBaseService.GetListAsync(queryCondition);
            return Ok(actionResult);
        }

        /// <summary>
        /// 按编号查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Log(OperationType.QueryEntity)]
        public async Task<IActionResult> GetEntityById(Guid id)
        {
            var actionResult = await this.NGAdminBaseService.GetByIdAsync(id);
            return Ok(actionResult);
        }

        /// <summary>
        /// 新增对象
        /// </summary>
        /// <param name="t">对象</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.AddEntity)]
        public async Task<IActionResult> AddEntity([FromBody] T t)
        {
            var actionResult = await this.NGAdminBaseService.AddAsync(t);
            return Ok(actionResult);
        }

        /// <summary>
        /// 编辑对象
        /// </summary>
        /// <param name="t">对象</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Log(OperationType.EditEntity)]
        public async Task<IActionResult> EditEntity([FromBody] T t)
        {
            var actionResult = await this.NGAdminBaseService.UpdateAsync(t);
            return Ok(actionResult);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{id}")]
        [Log(OperationType.DeleteEntity)]
        public async Task<IActionResult> DeleteEntityById(Guid id)
        {
            var actionResult = await this.NGAdminBaseService.DeleteAsync(id);
            return Ok(actionResult);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">编号数组</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{ids}")]
        [Log(OperationType.DeleteEntity)]
        public async Task<IActionResult> DeleteEntitiesByIds(String ids)
        {
            var array = NGStringUtil.GetGuids(ids);
            var actionResult = await this.NGAdminBaseService.DeleteRangeAsync(array);
            return Ok(actionResult);
        }

        /// <summary>
        /// 下载Excel
        /// </summary>
        /// <param name="excelId">文件编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{excelId}")]
        [Log(OperationType.DownloadFile)]
        public async Task<IActionResult> DownloadExcel(String excelId)
        {
            return await Task.Run(() =>
            {
                //存储路径
                var path = Path.Join(NGAdminGlobalContext.DirectoryConfig.GetTempPath(), "/");
                //文件路径
                var filePath = Path.Join(path, excelId + ".xls");
                //文件读写流
                var stream = new FileStream(filePath, FileMode.Open);
                //设置流的起始位置
                stream.Position = 0;

                return File(stream, "application/octet-stream", "错误信息.xls");
            });
        }

        /// <summary>
        /// 下载Excel模板
        /// </summary>
        /// <param name="templateId">文件编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{templateId}")]
        [Log(OperationType.DownloadFile)]
        public async Task<IActionResult> DownloadTemplate(String templateId)
        {
            return await Task.Run(() =>
            {
                //存储路径
                var path = Path.Join(NGAdminGlobalContext.DirectoryConfig.GetTemplateDirectory(), "/");
                //文件路径
                var filePath = Path.Join(path, templateId + ".xls");
                //文件读写流
                var stream = new FileStream(filePath, FileMode.Open);
                //设置流的起始位置
                stream.Position = 0;

                return File(stream, "application/octet-stream", "导入模板.xls");
            });
        }

        /// <summary>
        /// 下载Zip
        /// </summary>
        /// <param name="zipId">文件编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{zipId}")]
        [Log(OperationType.DownloadFile)]
        public async Task<IActionResult> DownloadZip(String zipId)
        {
            return await Task.Run(() =>
            {
                //存储路径
                var path = Path.Join(NGAdminGlobalContext.DirectoryConfig.GetTempPath(), "/");
                //文件路径
                var filePath = Path.Join(path, zipId + ".zip");
                //文件读写流
                var stream = new FileStream(filePath, FileMode.Open);
                //设置流的起始位置
                stream.Position = 0;

                return File(stream, "application/octet-stream", "RuYiAdmin.zip");
            });
        }

        #endregion
    }
}
