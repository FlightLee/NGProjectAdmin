//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Entity.CoreEnum;
using NGProjectAdmin.Service.BusinessService.SystemManagement.ImportDetailService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.ImportService;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 导入配置管理控制器
    /// </summary>
    public class ImportConfigManagementController : NGAdminBaseController<SysImportConfig>
    {
        #region 属性及构造函数

        /// <summary>
        /// 导入配置接口实例
        /// </summary>
        private readonly IImportConfigService importConfigService;

        /// <summary>
        /// 导入配置明细接口实例
        /// </summary>
        private readonly IImportConfigDetailService importConfigDetailService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="importConfigService"></param>
        /// <param name="importConfigDetailService"></param>
        public ImportConfigManagementController(IImportConfigService importConfigService,
            IImportConfigDetailService importConfigDetailService) : base(importConfigService)
        {
            this.importConfigService = importConfigService;
            this.importConfigDetailService = importConfigDetailService;
        }

        #endregion

        #region 查询导入配置列表

        /// <summary>
        /// 查询导入配置列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        [Permission("import:config:query:list")]
        public async Task<IActionResult> Post(QueryCondition queryCondition)
        {
            var actionResult = await this.importConfigService.SqlQueryAsync(queryCondition, "sqls:sql:query_sysimportconfig");
            return Ok(actionResult);
        }

        #endregion

        #region 按编号获取配置

        /// <summary>
        /// 按编号获取配置
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{id}")]
        [Log(OperationType.QueryEntity)]
        [Permission("import:config:query:list")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var actionResult = await this.importConfigService.GetByIdAsync(id);
            return Ok(actionResult);
        }

        #endregion

        #region 按父键获取配置明细

        /// <summary>
        /// 按父键获取配置明细
        /// </summary>
        /// <param name="parentId">父键</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{parentId}")]
        [Log(OperationType.QueryList)]
        [Permission("import:config:query:list")]
        public async Task<IActionResult> GetByParentId(Guid parentId)
        {
            var actionResult = new Entity.CoreEntity.ActionResult();

            var task = await this.importConfigDetailService.GetListAsync();
            var list = (List<SysImportConfigDetail>)task.Object;
            list = list.Where(t => t.IsDel == 0).OrderByDescending(t => t.CreateTime).ToList();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = list.Where(t => t.ParentId.Equals(parentId)).OrderBy(t => t.SerialNumber).ToList();

            return Ok(actionResult);
        }

        #endregion

        #region 添加导入配置

        /// <summary>
        /// 添加导入配置
        /// </summary>
        /// <param name="importConfig">导入配置</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.AddEntity)]
        [Permission("import:config:add:entity")]
        public async Task<IActionResult> AddConfig([FromBody] SysImportConfig importConfig)
        {
            var actionResult = await this.importConfigService.AddAsync(importConfig);
            return Ok(actionResult);
        }

        #endregion

        #region 添加导入配置明细

        /// <summary>
        ///添加导入配置明细
        /// </summary>
        /// <param name="configDetail">配置明细</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.AddEntity)]
        [Permission("import:config:add:entity")]
        public async Task<IActionResult> AddConfigDetail([FromBody] SysImportConfigDetail configDetail)
        {
            var actionResult = await this.importConfigDetailService.AddAsync(configDetail);
            return Ok(actionResult);
        }

        #endregion

        #region 编辑导入配置

        /// <summary>
        /// 编辑导入配置
        /// </summary>
        /// <param name="importConfig">导入配置</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Log(OperationType.EditEntity)]
        [Permission("import:config:edit:entity")]
        public async Task<IActionResult> EditConfig([FromBody] SysImportConfig importConfig)
        {
            var actionResult = await this.importConfigService.UpdateAsync(importConfig);
            return Ok(actionResult);
        }

        #endregion

        #region 编辑导入配置明细

        /// <summary>
        ///编辑导入配置明细
        /// </summary>
        /// <param name="configDetail">配置明细</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Log(OperationType.EditEntity)]
        [Permission("import:config:edit:entity")]
        public async Task<IActionResult> EditConfigDetail([FromBody] SysImportConfigDetail configDetail)
        {
            var actionResult = await this.importConfigDetailService.UpdateAsync(configDetail);
            return Ok(actionResult);
        }

        #endregion

        #region 删除导入配置

        /// <summary>
        /// 删除导入配置
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{id}")]
        [Log(OperationType.DeleteEntity)]
        [Permission("import:config:del:entities")]
        public async Task<IActionResult> DeleteConfig(Guid id)
        {
            //数据删除检测
            await this.DeleteCheck(id.ToString());
            //数据删除
            var actionResult = await this.importConfigService.DeleteAsync(id);
            return Ok(actionResult);
        }

        #endregion

        #region 批量删除导入配置

        /// <summary>
        /// 批量删除导入配置
        /// </summary>
        /// <param name="ids">数组串</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{ids}")]
        [Log(OperationType.DeleteEntity)]
        [Permission("import:config:del:entities")]
        public async Task<IActionResult> DeleteConfigs(String ids)
        {
            //数据删除检测
            await this.DeleteCheck(ids);
            //数据删除
            var array = NGStringUtil.GetGuids(ids);
            var actionResult = await this.importConfigService.DeleteRangeAsync(array);
            return Ok(actionResult);
        }

        #endregion

        #region 删除配置明细

        /// <summary>
        /// 删除配置明细
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{id}")]
        [Log(OperationType.DeleteEntity)]
        [Permission("import:config:del:entities")]
        public async Task<IActionResult> DeleteConfigDetail(Guid id)
        {
            var actionResult = await this.importConfigDetailService.DeleteAsync(id);
            return Ok(actionResult);
        }

        #endregion

        #region 批量删除配置明细

        /// <summary>
        /// 批量删除配置明细
        /// </summary>
        /// <param name="ids">数组串</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{ids}")]
        [Log(OperationType.DeleteEntity)]
        [Permission("import:config:del:entities")]
        public async Task<IActionResult> DeleteConfigDetails(String ids)
        {
            var array = NGStringUtil.GetGuids(ids);
            var actionResult = await this.importConfigDetailService.DeleteRangeAsync(array);
            return Ok(actionResult);
        }

        #endregion

        #region 删除子表检测

        /// <summary>
        /// 删除子表检测
        /// </summary>
        /// <param name="ids">数组串</param>
        /// <returns>真假值</returns>
        private async Task DeleteCheck(String ids)
        {
            var array = NGStringUtil.GetGuids(ids);
            foreach (var item in array)
            {
                var queryCondition = new QueryCondition();
                queryCondition.QueryItems = new List<QueryItem>();
                queryCondition.QueryItems.Add(new QueryItem()
                {
                    Field = "ParentId",
                    DataType = DataType.Guid,
                    QueryMethod = QueryMethod.Equal,
                    Value = item
                });

                var data = await this.importConfigDetailService.GetListAsync(queryCondition);
                if (data.List.Count > 0)
                {
                    throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException("config contains subitems,can not be deleted");
                }
            }
        }

        #endregion
    }
}
