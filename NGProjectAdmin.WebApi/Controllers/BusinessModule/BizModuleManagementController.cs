using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.BusinessService.BusinessModule.BusinessModuleService;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NGProjectAdmin.WebApi.Controllers.BusinessModule
{
    /// <summary>
    /// BizModule控制器
    /// </summary>
    public class BizModuleManagementController : NGAdminBaseController<BizModule>
    {
        #region 属性及构造函数

        /// <summary>
        /// 业务模块接口实例
        /// </summary>
        private readonly IBizModuleService BizModuleService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BizModuleService"></param>
        public BizModuleManagementController(IBizModuleService BizModuleService) : base(BizModuleService)
        {
            this.BizModuleService = BizModuleService;
        }

        #endregion

        #region 查询模块列表

        /// <summary>
        /// 查询模块列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        [Permission("biz:module:list")]
        public async Task<IActionResult> Post(QueryCondition queryCondition)
        {
            var actionResult = await this.BizModuleService.SqlQueryAsync(queryCondition, "sqls:sql:query_biz_module");
            return Ok(actionResult);
        }

        #endregion

        #region 查询模块信息

        /// <summary>
        /// 查询模块信息
        /// </summary>
        /// <param name="moduleId">模块编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{moduleId}")]
        [Log(OperationType.QueryEntity)]
        [Permission("biz:module:list")]
        public async Task<IActionResult> GetById(Guid moduleId)
        {
            var actionResult = await this.BizModuleService.GetByIdAsync(moduleId);
            return Ok(actionResult);
        }

        #endregion

        #region 新增业务模块

        /// <summary>
        /// 新增业务模块
        /// </summary>
        /// <param name="bizModule">业务模块对象</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.AddEntity)]
        [Permission("module:add:entity")]
        public async Task<IActionResult> Add([FromBody] BizModule bizModule)
        {
            var actionResult = await this.BizModuleService.AddAsync(bizModule);
            return Ok(actionResult);
        }

        #endregion

        #region 编辑业务模块

        /// <summary>
        /// 编辑业务模块
        /// </summary>
        /// <param name="bizModule">业务模块对象</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Log(OperationType.EditEntity)]
        [Permission("module:edit:entity")]
        public async Task<IActionResult> Put([FromBody] BizModule bizModule)
        {
            var actionResult = await this.BizModuleService.UpdateAsync(bizModule);
            return Ok(actionResult);
        }

        #endregion

        #region 删除业务模块

        /// <summary>
        /// 删除业务模块
        /// </summary>
        /// <param name="moduleId">模块编号</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{moduleId}")]
        [Permission("module:del:entity")]
        [Log(OperationType.DeleteEntity)]
        public async Task<IActionResult> Delete(Guid moduleId)
        {
            //删除检测
            await this.DeleteCheck(moduleId.ToString());
            //数据删除
            var actionResult = await this.BizModuleService.DeleteAsync(moduleId);
            return Ok(actionResult);
        }

        #endregion

        #region 批量删除业务模块

        /// <summary>
        /// 批量删除业务模块
        /// </summary>
        /// <param name="ids">编号数组</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{ids}")]
        [Log(OperationType.DeleteEntity)]
        [Permission("module:del:entity")]
        public async Task<IActionResult> DeleteRange(String ids)
        {
            //删除检测
            await this.DeleteCheck(ids);
            //数据删除
            var array = NGStringUtil.GetGuids(ids);
            var actionResult = await this.BizModuleService.DeleteRangeAsync(array);
            return Ok(actionResult);
        }

        #endregion

        #region 查询无权限模块列表

        /// <summary>
        /// 查询无权限模块列表
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{userId}")]
        [Log(OperationType.QueryList)]
        [Permission("biz:module:list")]
        public async Task<IActionResult> GetUserNonModules(Guid userId)
        {
            var sqlKey = "sqls:sql:query_user_non_modules";
            var strSQL = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value;
            strSQL = String.Format(strSQL, userId);

            var actionResult = await this.BizModuleService.SqlQueryAsync(strSQL, new QueryCondition());
            return Ok(actionResult);
        }

        #endregion

        #region 业务模块删除检测

        /// <summary>
        /// 业务模块删除检测
        /// </summary>
        /// <param name="ids">模块编号组</param>
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
                    Field = "MODULE_ID",
                    Value = item,
                    QueryMethod = Entity.CoreEnum.QueryMethod.Equal,
                    DataType = Entity.CoreEnum.DataType.Guid
                });

                var list = await this.BizModuleService.SqlQueryAsync(queryCondition, "sqls:sql:query_biz_user_module");
                var count = list.List.Count();
                if (count > 0)
                {
                    throw new NGAdminCustomException("module has been used,can not be deleted");
                }
            }
        }

        #endregion
    }
}
