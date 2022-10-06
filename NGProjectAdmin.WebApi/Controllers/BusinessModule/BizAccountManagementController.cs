
using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.BusinessService.BusinessModule.BusinessAccountService;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System;
using System.Threading.Tasks;

namespace NGProjectAdmin.WebApi.Controllers.BusinessModule
{
    /// <summary>
    /// BizAccount控制器
    /// </summary>
    public class BizAccountManagementController : NGAdminBaseController<BizAccount>
    {
        #region 属性及构造函数

        /// <summary>
        /// 业务接口实例
        /// </summary>
        private readonly IBizAccountService BizAccountService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BizAccountService"></param>
        public BizAccountManagementController(IBizAccountService BizAccountService) : base(BizAccountService)
        {
            this.BizAccountService = BizAccountService;
        }

        #endregion

        #region 查询模块同步账号列表

        /// <summary>
        /// 查询模块同步账号列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        [Permission("account:query:list")]
        public async Task<IActionResult> Post(QueryCondition queryCondition)
        {
            var actionResult = await this.BizAccountService.SqlQueryAsync(queryCondition, "sqls:sql:query_biz_account");
            return Ok(actionResult);
        }

        #endregion

        #region 查询同步账号信息

        /// <summary>
        /// 查询同步账号信息
        /// </summary>
        /// <param name="accountId">同步账号编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{accountId}")]
        [Log(OperationType.QueryEntity)]
        [Permission("account:query:list")]
        public async Task<IActionResult> GetById(Guid accountId)
        {
            var actionResult = await this.BizAccountService.GetByIdAsync(accountId);
            return Ok(actionResult);
        }

        #endregion

        #region 新增模块同步账号

        /// <summary>
        /// 新增模块同步账号
        /// </summary>
        /// <param name="bizAccount">同步账号</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.AddEntity)]
        [Permission("account:add:entity")]
        public async Task<IActionResult> Add([FromBody] BizAccount bizAccount)
        {
            bizAccount.UserPassword = NGRsaUtil.PemDecrypt(bizAccount.UserPassword, NGAdminGlobalContext.SystemConfig.RsaPrivateKey);
            bizAccount.UserPassword = NGAesUtil.Encrypt(bizAccount.UserPassword, NGAdminGlobalContext.SystemConfig.AesKey);
            var actionResult = await this.BizAccountService.AddAsync(bizAccount);
            return Ok(actionResult);
        }

        #endregion

        #region 编辑模块同步账号

        /// <summary>
        /// 编辑模块同步账号
        /// </summary>
        /// <param name="bizAccount">同步账号</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Log(OperationType.EditEntity)]
        [Permission("account:edit:entity")]
        public async Task<IActionResult> Put([FromBody] BizAccount bizAccount)
        {
            var account = this.BizAccountService.GetById(bizAccount.Id).Object as BizAccount;
            var password = NGRsaUtil.PemDecrypt(bizAccount.UserPassword, NGAdminGlobalContext.SystemConfig.RsaPrivateKey);
            if (password != account.UserPassword)
            {
                bizAccount.UserPassword = NGAesUtil.Encrypt(password, NGAdminGlobalContext.SystemConfig.AesKey);
            }
            else
            {
                bizAccount.UserPassword = password;
            }
            var actionResult = await this.BizAccountService.UpdateAsync(bizAccount);
            return Ok(actionResult);
        }

        #endregion

        #region 批量删除模块同步账号

        /// <summary>
        /// 批量删除模块同步账号
        /// </summary>
        /// <param name="ids">编号数组</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{ids}")]
        [Log(OperationType.DeleteEntity)]
        [Permission("account:del:entities")]
        public async Task<IActionResult> DeleteRange(String ids)
        {
            var array = NGStringUtil.GetGuids(ids);
            var actionResult = await this.BizAccountService.DeleteRangeAsync(array);
            return Ok(actionResult);
        }

        #endregion
    }
}
