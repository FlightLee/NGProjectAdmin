using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.BusinessModule;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.Base;
using NGProjectAdmin.Service.BusinessService.BusinessModule.BusinessModuleService;
using NGProjectAdmin.Service.BusinessService.BusinessModule.BusinessUserModuleService;
using NGProjectAdmin.Service.BusinessService.BusinessModule.BusinessUserService;
using NGProjectAdmin.Service.BusinessService.RedisService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.LogService;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NGProjectAdmin.WebApi.Controllers.BusinessModule
{
    /// <summary>
    /// BizUser控制器
    /// </summary>
    public class BizUserManagementController : NGAdminBaseController<BizUser>
    {
        #region 属性及构造函数

        /// <summary>
        /// 业务模块接口实例
        /// </summary>
        private readonly IBizModuleService BizModuleService;

        /// <summary>
        /// 模块用户接口实例
        /// </summary>
        private readonly IBizUserModuleService BizUserModuleService;

        /// <summary>
        /// 业务接口实例
        /// </summary>
        private readonly IBizUserService BizUserService;

        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// Redis接口实例
        /// </summary>
        private readonly IRedisService redisService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BizUserService"></param>
        /// <param name="BizUserModuleService"></param>
        /// <param name="BizModuleService"></param>
        /// <param name="context"></param>
        /// <param name="redisService"></param>
        public BizUserManagementController(IBizUserService BizUserService,
                                           IBizUserModuleService BizUserModuleService,
                                           IBizModuleService BizModuleService,
                                           IHttpContextAccessor context,
                                           IRedisService redisService) : base(BizUserService)
        {
            this.BizUserService = BizUserService;
            this.BizUserModuleService = BizUserModuleService;
            this.BizModuleService = BizModuleService;
            this.context = context;
            this.redisService = redisService;
        }

        #endregion

        #region 同步新增模块用户

        /// <summary>
        /// 同步新增模块用户
        /// </summary>
        /// <param name="bizUser">模块用户对象</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Add([FromBody] BizUserDTO bizUser)
        {
            var arrary = NGRsaUtil.PemDecrypt(bizUser.UserPassword, NGAdminGlobalContext.SystemConfig.RsaPrivateKey);
            var password = arrary.Split('_')[0];
            var token = arrary.Split('_')[1];

            var accountDTO = await this.redisService.GetAsync<BizAccountDTO>(NGAdminGlobalContext.RedisConfig.SynchronizationPattern + token);
            if (accountDTO != null && accountDTO.Token.Equals(token))
            {
                var task = await this.BizModuleService.GetByIdAsync(bizUser.ModuleId);
                var module = (BizModule)task.Object;

                bizUser.UserLogonName = NGRsaUtil.PemDecrypt(bizUser.UserLogonName, NGAdminGlobalContext.SystemConfig.RsaPrivateKey);
                bizUser.UserLogonName = bizUser.UserLogonName.Split('_')[0];

                var user = await this.BizUserService.GetBizUser(module.ModuleShortNameEN, bizUser.UserLogonName);
                if (user != null)
                {
                    throw new NGAdminCustomException("logon name has existed");
                }

                #region 新增业务模块用户

                var defaultPassword = NGAdminGlobalContext.SystemConfig.DefaultPassword;
                var aesKey = NGAdminGlobalContext.SystemConfig.AesKey;

                user = new BizUser();
                user.UserLogonName = String.Join('_', module.ModuleShortNameEN, bizUser.UserLogonName);
                user.UserDisplayName = bizUser.UserDisplayName;
                user.Salt = Guid.NewGuid();
                user.UserPassword = NGAesUtil.Encrypt(defaultPassword + user.Salt, aesKey);
                user.Telephone = bizUser.Telephone;
                user.MobilePhone = bizUser.MobilePhone;
                user.Email = bizUser.Email;
                user.Sex = bizUser.Sex;
                user.IsEnabled = (int)YesNo.YES;
                user.IsDel = (int)DeletionType.Undeleted;
                user.Creator = accountDTO.Id;
                user.CreateTime = DateTime.Now;
                user.Modifier = accountDTO.Id;
                user.ModifyTime = DateTime.Now;
                user.VersionId = Guid.NewGuid();
                await NGAdminDbScope.NGDbContext.Insertable<BizUser>(user).ExecuteCommandAsync();

                #endregion

                #region 新增模块与用户关系

                var userModule = new BizUserModule();
                userModule.UserId = user.Id;
                userModule.ModuleId = bizUser.ModuleId;
                userModule.UserModuleLogonName = bizUser.UserLogonName;
                userModule.UserModulePassword = password;
                userModule.IsDel = (int)DeletionType.Undeleted;
                userModule.Creator = accountDTO.Id;
                userModule.CreateTime = DateTime.Now;
                userModule.Modifier = accountDTO.Id;
                userModule.ModifyTime = DateTime.Now;
                userModule.VersionId = Guid.NewGuid();
                await NGAdminDbScope.NGDbContext.Insertable<BizUserModule>(userModule).ExecuteCommandAsync();

                #endregion

                #region 记录同步新增用户日志

                var log = new SysLog();

                log.Id = Guid.NewGuid();

                log.UserId = accountDTO.Id;
                log.UserName = accountDTO.UserDisplayName + "/" + accountDTO.UserLogonName;
                log.OrgId = module.Id;
                log.OrgName = module.ModuleShortName;

                log.System = this.context.HttpContext.Request.Headers["User-Agent"].FirstOrDefault().ToString().Split('(')[1].Split(')')[0];
                log.Browser = this.context.HttpContext.Request.Headers["sec-ch-ua"];

                var ip = this.context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (String.IsNullOrEmpty(ip))
                {
                    ip = this.context.HttpContext.Connection.RemoteIpAddress.ToString();
                }
                log.IP = ip;

                log.OperationType = OperationType.SyncAddUser;
                log.RequestMethod = context.HttpContext.Request.Method;
                log.RequestUrl = "/API/BizUserManagement/Add";
                log.Params = Newtonsoft.Json.JsonConvert.SerializeObject(bizUser);
                log.Result = String.Empty;

                log.OldValue = String.Empty;
                log.NewValue = String.Empty;

                log.Remark = $"{module.ModuleShortName}于{DateTime.Now}访问了{log.RequestUrl}接口";
                log.IsDel = (int)DeletionType.Undeleted;
                log.Creator = accountDTO.Id;
                log.CreateTime = DateTime.Now;
                log.Modifier = accountDTO.Id;
                log.ModifyTime = DateTime.Now;
                log.VersionId = Guid.NewGuid();

                //记录审计日志
                await log.WriteAsync();

                #endregion

                return Ok(Entity.CoreEntity.ActionResult.OK());
            }

            throw new NGAdminCustomException("token is invalid");
        }

        #endregion

        #region 同步编辑模块用户

        /// <summary>
        /// 同步编辑模块用户
        /// </summary>
        /// <param name="bizUser">模块用户对象</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> Put([FromBody] BizUserDTO bizUser)
        {
            var arrary = NGRsaUtil.PemDecrypt(bizUser.UserPassword, NGAdminGlobalContext.SystemConfig.RsaPrivateKey);
            var password = arrary.Split('_')[0];
            var token = arrary.Split('_')[1];

            var accountDTO = await this.redisService.GetAsync<BizAccountDTO>(NGAdminGlobalContext.RedisConfig.SynchronizationPattern + token);
            if (accountDTO != null && accountDTO.Token.Equals(token))
            {
                var task = await this.BizModuleService.GetByIdAsync(bizUser.ModuleId);
                var module = (BizModule)task.Object;

                bizUser.UserLogonName = NGRsaUtil.PemDecrypt(bizUser.UserLogonName, NGAdminGlobalContext.SystemConfig.RsaPrivateKey);
                bizUser.UserLogonName = bizUser.UserLogonName.Split('_')[0];

                var user = await this.BizUserService.GetBizUser(module.ModuleShortNameEN, bizUser.UserLogonName);
                if (user != null)
                {
                    user.UserDisplayName = bizUser.UserDisplayName;
                    user.Telephone = bizUser.Telephone;
                    user.MobilePhone = bizUser.MobilePhone;
                    user.Email = bizUser.Email;
                    user.Sex = bizUser.Sex;
                    user.Modifier = accountDTO.Id;
                    user.ModifyTime = DateTime.Now;
                    user.VersionId = Guid.NewGuid();
                    await NGAdminDbScope.NGDbContext.Updateable<BizUser>(user).ExecuteCommandAsync();

                    var userModule = await this.BizUserModuleService.GetBizUserModule(user.Id, module.Id);
                    if (userModule != null)
                    {
                        userModule.UserModulePassword = password;
                        userModule.Modifier = accountDTO.Id;
                        userModule.ModifyTime = DateTime.Now;
                        userModule.VersionId = Guid.NewGuid();
                        await NGAdminDbScope.NGDbContext.Updateable<BizUserModule>(userModule).ExecuteCommandAsync();
                    }

                    #region 记录同步编辑用户日志

                    var log = new SysLog();

                    log.Id = Guid.NewGuid();

                    log.UserId = accountDTO.Id;
                    log.UserName = accountDTO.UserDisplayName + "/" + accountDTO.UserLogonName;
                    log.OrgId = module.Id;
                    log.OrgName = module.ModuleShortName;

                    log.System = this.context.HttpContext.Request.Headers["User-Agent"].FirstOrDefault().ToString().Split('(')[1].Split(')')[0];
                    log.Browser = this.context.HttpContext.Request.Headers["sec-ch-ua"];

                    var ip = this.context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                    if (String.IsNullOrEmpty(ip))
                    {
                        ip = this.context.HttpContext.Connection.RemoteIpAddress.ToString();
                    }
                    log.IP = ip;

                    log.OperationType = OperationType.SyncEditUser;
                    log.RequestMethod = context.HttpContext.Request.Method;
                    log.RequestUrl = "/API/BizUserManagement/Put";
                    log.Params = Newtonsoft.Json.JsonConvert.SerializeObject(bizUser);
                    log.Result = String.Empty;

                    log.OldValue = String.Empty;
                    log.NewValue = String.Empty;

                    log.Remark = $"{module.ModuleShortName}于{DateTime.Now}访问了{log.RequestUrl}接口";
                    log.IsDel = (int)DeletionType.Undeleted;
                    log.Creator = accountDTO.Id;
                    log.CreateTime = DateTime.Now;
                    log.Modifier = accountDTO.Id;
                    log.ModifyTime = DateTime.Now;
                    log.VersionId = Guid.NewGuid();

                    //记录审计日志
                    await log.WriteAsync();

                    #endregion

                    return Ok(Entity.CoreEntity.ActionResult.OK());
                }
                else
                {
                    throw new NGAdminCustomException("not found");
                }
            }

            throw new NGAdminCustomException("token is invalid");
        }

        #endregion

        #region 同步删除模块用户

        /// <summary>
        /// 同步删除模块用户
        /// </summary>
        /// <param name="bizUser">模块用户对象</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteRange(BizUserDTO bizUser)
        {
            var logonNames = NGRsaUtil.PemDecrypt(bizUser.UserLogonName, NGAdminGlobalContext.SystemConfig.RsaPrivateKey);
            var token = logonNames.Split('_')[1];

            var accountDTO = await this.redisService.GetAsync<BizAccountDTO>(NGAdminGlobalContext.RedisConfig.SynchronizationPattern + token);
            if (accountDTO != null && accountDTO.Token.Equals(token))
            {
                var module = (BizModule)(await this.BizModuleService.GetByIdAsync(bizUser.ModuleId)).Object;
                var array = logonNames.Split('_')[0].ToString().Split(',');

                //var range = new List<Guid>();
                var userModuleRange = new List<BizUserModule>();
                foreach (var item in array)
                {
                    var user = await this.BizUserService.GetBizUser(module.ModuleShortNameEN, item);
                    if (user != null)
                    {
                        //range.Add(user.Id);
                        var userModule = await this.BizUserModuleService.GetBizUserModule(user.Id, module.Id);
                        userModuleRange.Add(userModule);
                    }
                    else
                    {
                        throw new NGAdminCustomException(bizUser.UserLogonName + " is not existed");
                    }
                }

                //删除业务用户
                //var actionResult = this.BizUserService.DeleteRange(range.ToArray());
                foreach (var item in userModuleRange)
                {
                    item.IsDel = (int)DeletionType.Deleted;
                    item.Modifier = accountDTO.Id;
                    item.ModifyTime = DateTime.Now;
                    item.VersionId = Guid.NewGuid();
                    await NGAdminDbScope.NGDbContext.Updateable<BizUserModule>(item).ExecuteCommandAsync();//删除授权关系
                }

                #region 记录同步删除用户日志

                var log = new SysLog();

                log.Id = Guid.NewGuid();

                log.UserId = accountDTO.Id;
                log.UserName = accountDTO.UserDisplayName + "/" + accountDTO.UserLogonName;
                log.OrgId = module.Id;
                log.OrgName = module.ModuleShortName;

                log.System = this.context.HttpContext.Request.Headers["User-Agent"].FirstOrDefault().ToString().Split('(')[1].Split(')')[0];
                log.Browser = this.context.HttpContext.Request.Headers["sec-ch-ua"];

                var ip = this.context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (String.IsNullOrEmpty(ip))
                {
                    ip = this.context.HttpContext.Connection.RemoteIpAddress.ToString();
                }
                log.IP = ip;

                log.OperationType = OperationType.SyncDeleteUser;
                log.RequestMethod = context.HttpContext.Request.Method;
                log.RequestUrl = "/API/BizUserManagement/DeleteRange";
                log.Params = Newtonsoft.Json.JsonConvert.SerializeObject(bizUser);
                log.Result = String.Empty;

                log.OldValue = String.Empty;
                log.NewValue = String.Empty;

                log.Remark = $"{module.ModuleShortName}于{DateTime.Now}访问了{log.RequestUrl}接口";
                log.IsDel = (int)DeletionType.Undeleted;
                log.Creator = accountDTO.Id;
                log.CreateTime = DateTime.Now;
                log.Modifier = accountDTO.Id;
                log.ModifyTime = DateTime.Now;
                log.VersionId = Guid.NewGuid();

                //记录审计日志
                await log.WriteAsync();

                #endregion

                return Ok(Entity.CoreEntity.ActionResult.OK());
            }

            throw new NGAdminCustomException("token is invalid");
        }

        #endregion

        #region 查询离态用户列表

        /// <summary>
        /// 查询离态用户列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        [Permission("user:nonmodule:list")]
        public async Task<IActionResult> Post(QueryCondition queryCondition)
        {
            var actionResult = await this.BizUserModuleService.SqlQueryAsync<BizUserModuleDTO>(queryCondition, "sqls:sql:query_biz_user_nonmodule");
            return Ok(actionResult);
        }

        #endregion

        #region 匿名获取盐份服务

        /// <summary>
        /// 匿名获取盐份服务
        /// </summary>
        /// <returns>盐份</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSalt()
        {
            var salt = Guid.NewGuid();
            await redisService.SetAsync(salt.ToString(), salt, NGAdminGlobalContext.JwtSettings.SaltExpiration);
            var actionResult = new NGProjectAdmin.Entity.CoreEntity.ActionResult()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Object = salt,
                Message = new String("OK")
            };
            return Ok(actionResult);
        }

        #endregion

        #region 匿名获取同步口令

        /// <summary>
        /// 匿名获取同步口令
        /// </summary>
        /// <param name="loginDTO">登录信息</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetToken([FromBody] LoginDTO loginDTO)
        {
            var actionResult = await this.BizUserModuleService.GetToken(loginDTO);
            return Ok(actionResult);
        }

        #endregion
    }
}
