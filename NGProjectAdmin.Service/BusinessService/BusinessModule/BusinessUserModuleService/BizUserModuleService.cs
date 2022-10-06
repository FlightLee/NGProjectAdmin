using AutoMapper;
using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.BusinessModule;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.BusinessRepository.BusinessModule.BusinessAccountRepository;
using NGProjectAdmin.Repository.BusinessRepository.BusinessModule.BusinessModuleRepository;
using NGProjectAdmin.Repository.BusinessRepository.BusinessModule.BusinessUserModuleRepository;
using NGProjectAdmin.Repository.BusinessRepository.BusinessModule.BusinessUserRepository;
using NGProjectAdmin.Repository.BusinessRepository.RedisRepository;
using NGProjectAdmin.Service.Base;
using NGProjectAdmin.Service.BusinessService.SystemManagement.LogService;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.BusinessModule.BusinessUserModuleService
{
    /// <summary>
    /// BizUserModule业务层实现
    /// </summary>
    public class BizUserModuleService : NGAdminBaseService<BizUserModule>, IBizUserModuleService
    {
        #region 属性及构造函数

        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IBizUserModuleRepository BizUserModuleRepository;

        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// Redis仓储实例
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// 模块仓储
        /// </summary>
        private readonly IBizModuleRepository BizModuleRepository;

        /// <summary>
        /// 业务用户仓储
        /// </summary>
        private readonly IBizUserRepository BizUserRepository;

        /// <summary>
        /// 同步账号仓储
        /// </summary>
        private readonly IBizAccountRepository BizAccountRepository;

        /// <summary>
        /// AutoMapper实例
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BizUserModuleRepository"></param>
        /// <param name="context"></param>
        /// <param name="redisRepository"></param>
        /// <param name="BizModuleRepository"></param>
        /// <param name="BizUserRepository"></param>
        /// <param name="BizAccountRepository"></param>
        /// <param name="mapper"></param>
        public BizUserModuleService(IBizUserModuleRepository BizUserModuleRepository,
                                    IHttpContextAccessor context,
                                    IRedisRepository redisRepository,
                                    IBizModuleRepository BizModuleRepository,
                                    IBizUserRepository BizUserRepository,
                                    IBizAccountRepository BizAccountRepository,
                                    IMapper mapper) : base(BizUserModuleRepository)
        {
            this.BizUserModuleRepository = BizUserModuleRepository;
            this.context = context;
            this.redisRepository = redisRepository;
            this.BizModuleRepository = BizModuleRepository;
            this.BizUserRepository = BizUserRepository;
            this.BizAccountRepository = BizAccountRepository;
            this.mapper = mapper;
        }

        #endregion

        #region 获取用户模块

        /// <summary>
        /// 获取用户模块
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="moduleId"></param>
        /// <returns>用户模块对象</returns>
        public async Task<BizUserModule> GetBizUserModule(Guid userId, Guid moduleId)
        {
            var queryCondition = new QueryCondition();
            queryCondition.QueryItems = new List<QueryItem>();
            queryCondition.QueryItems.Add(new QueryItem()
            {
                Field = "USER_ID",
                Value = userId,
                QueryMethod = Entity.CoreEnum.QueryMethod.Equal,
                DataType = Entity.CoreEnum.DataType.Guid
            });
            queryCondition.QueryItems.Add(new QueryItem()
            {
                Field = "MODULE_ID",
                Value = moduleId,
                QueryMethod = Entity.CoreEnum.QueryMethod.Equal,
                DataType = Entity.CoreEnum.DataType.Guid
            });

            RefAsync<int> totalCount = 0;
            var userModule = (await this.BizUserModuleRepository.SqlQueryAsync(queryCondition, "sqls:sql:query_biz_user_module", totalCount)).FirstOrDefault();

            return userModule;
        }

        #endregion

        #region 统一认证登录

        /// <summary>
        /// 统一认证登录
        /// </summary>
        /// <param name="loginDTO">登录信息</param>
        /// <returns>ActionResult</returns>
        public async Task<ActionResult> Logon(LoginDTO loginDTO)
        {
            var prikey = NGAdminGlobalContext.SystemConfig.RsaPrivateKey;

            #region 用户信息解密

            loginDTO.UserName = NGRsaUtil.PemDecrypt(loginDTO.UserName, prikey);
            loginDTO.Password = NGRsaUtil.PemDecrypt(loginDTO.Password, prikey);
            loginDTO.CaptchaId = NGRsaUtil.PemDecrypt(loginDTO.CaptchaId, prikey);
            loginDTO.Captcha = NGRsaUtil.PemDecrypt(loginDTO.Captcha, prikey);

            #endregion

            var obj = new Object();

            var resultNum = await this.redisRepository.GetAsync(loginDTO.CaptchaId);
            if (resultNum != null && resultNum.Equals(loginDTO.Captcha))
            {
                //删除验证码
                await this.redisRepository.DeleteAsync(new String[] { loginDTO.CaptchaId });

                #region 获取用户信息

                //用户名去盐
                loginDTO.UserName = loginDTO.UserName.Replace("_" + loginDTO.CaptchaId, "");
                //密码去盐
                loginDTO.Password = loginDTO.Password.Replace("_" + loginDTO.CaptchaId, "");

                var task = await this.BizUserRepository.GetListAsync();
                var tempUser = task.Where(t => t.IsDel == (int)DeletionType.Undeleted && t.UserLogonName.Equals(loginDTO.UserName)).FirstOrDefault();
                if (tempUser == null)
                {
                    
                    throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException("user is invalid");
                }

                //AES加密
                var aesKey = NGAdminGlobalContext.SystemConfig.AesKey;
                loginDTO.Password = NGAesUtil.Encrypt(loginDTO.Password + tempUser.Salt, aesKey);

                var users = task.Where(t => t.IsDel.Equals(0)).
                    Where(t => t.UserLogonName.Equals(loginDTO.UserName)).
                    Where(t => t.UserPassword.Equals(loginDTO.Password)).
                    Where(t => t.IsEnabled.Equals((int)UserStatus.Enabled));
                var list = users.ToList();

                #endregion

                //用户合法
                if (list.Count() == 1)
                {
                    var user = list.FirstOrDefault();
                    var token = Guid.NewGuid();
                    var tokenExpiration = NGAdminGlobalContext.SystemConfig.UserTokenExpiration;

                    #region 获取授权信息

                    var modules = (await this.BizModuleRepository.GetListAsync()).Where(t => t.IsDel == 0).ToList();
                    var userModules = (await this.BizUserModuleRepository.GetListAsync()).Where(t => t.IsDel == 0 && t.UserId == user.Id).ToList();

                    var permissions = new List<BizModuleDTO>();
                    foreach (var module in modules)
                    {
                        var moduleDto = new BizModuleDTO();
                        moduleDto.Id = module.Id;
                        moduleDto.ModuleName = module.ModuleName;
                        moduleDto.ModuleShortName = module.ModuleShortName;
                        moduleDto.ModuleShortNameEN = module.ModuleShortNameEN;
                        moduleDto.ModuleProtocol = module.ModuleProtocol;
                        moduleDto.ModuleAddress = module.ModuleAddress;
                        moduleDto.ModulePort = module.ModulePort;
                        moduleDto.ModuleLogoAddress = module.ModuleLogoAddress;
                        moduleDto.ModuleSsoAddress = module.ModuleSsoAddress;
                        moduleDto.ModuleTodoAddress = module.ModuleTodoAddress;
                        moduleDto.SerialNumber = module.SerialNumber;

                        var auth = userModules.Where(t => t.ModuleId == module.Id).FirstOrDefault();
                        if (auth != null)
                        {
                            moduleDto.UserModuleLogonName = auth.UserModuleLogonName;
                            moduleDto.UserModulePassword = NGHashUtil.HashToHexString512(auth.UserModulePassword + token);
                        }

                        permissions.Add(moduleDto);
                    }

                    #endregion

                    #region 写入缓存信息                    

                    var key = NGAdminGlobalContext.RedisConfig.UnifiedAuthenticationPattern + user.Id + "_";
                    var pattern = $"{key}*";
                    var keys = this.redisRepository.PatternSearch(pattern);
                    if (keys.Count > 0)
                    {
                        foreach (var item in keys)
                        {
                            await this.redisRepository.DeleteAsync(new string[] { item.ToString() });
                        }
                    }

                    var expiration = tokenExpiration * 60;
                    await this.redisRepository.SetAsync(key + token, user, expiration);

                    #endregion

                    //擦除敏感信息
                    user.UserPassword = null;

                    //登录授权
                    obj = new { user, permissions = permissions, token = token };

                    #region 记录登录日志

                    var log = new SysLog();

                    log.Id = Guid.NewGuid();
                    log.UserId = user.Id;
                    log.UserName = user.UserDisplayName + "/" + user.UserLogonName;

                    log.OrgId = Guid.Empty;
                    log.OrgName = "none";

                    log.System = this.context.HttpContext.Request.Headers["User-Agent"].FirstOrDefault().ToString().Split('(')[1].Split(')')[0];
                    log.Browser = this.context.HttpContext.Request.Headers["sec-ch-ua"];

                    var ip = this.context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                    if (string.IsNullOrEmpty(ip))
                    {
                        ip = this.context.HttpContext.Connection.RemoteIpAddress.ToString();
                    }
                    log.IP = ip;

                    log.OperationType = OperationType.UnifiedAuthentication;
                    log.RequestMethod = context.HttpContext.Request.Method;
                    log.RequestUrl = "/API/BizUserModuleManagement/Logon";
                    log.Params = String.Empty;

                    log.Result = String.Empty;
                    log.OldValue = String.Empty;
                    log.NewValue = String.Empty;
                    log.Remark = $"{user.UserDisplayName}于{DateTime.Now}访问了{log.RequestUrl}接口";

                    log.IsDel = (int)DeletionType.Undeleted;
                    log.Creator = user.Id;
                    log.CreateTime = DateTime.Now;
                    log.Modifier = user.Id;
                    log.ModifyTime = DateTime.Now;
                    log.VersionId = Guid.NewGuid();

                    //记录审计日志
                    await log.WriteAsync();

                    #endregion
                }
                else
                {
                    throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException("user is invalid");
                }
            }
            else
            {
                throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException("captcha is invalid");
            }

            var actionResult = new ActionResult();
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = obj;

            return actionResult;
        }

        #endregion

        #region 获取同步token

        /// <summary>
        /// 获取同步token
        /// </summary>
        /// <param name="loginDTO">登录信息</param>
        /// <returns>ActionResult</returns>
        public async Task<ActionResult> GetToken(LoginDTO loginDTO)
        {
            var actionResult = new ActionResult();

            var prikey = NGAdminGlobalContext.SystemConfig.RsaPrivateKey;

            #region 用户信息解密

            loginDTO.UserName = NGRsaUtil.PemDecrypt(loginDTO.UserName, prikey);
            loginDTO.Password = NGRsaUtil.PemDecrypt(loginDTO.Password, prikey);
            loginDTO.CaptchaId = NGRsaUtil.PemDecrypt(loginDTO.CaptchaId, prikey);

            #endregion

            var resultNum = await this.redisRepository.GetAsync(loginDTO.CaptchaId);
            if (resultNum != null && resultNum.Equals(loginDTO.CaptchaId))
            {
                //删除验证码
                await this.redisRepository.DeleteAsync(new String[] { loginDTO.CaptchaId });

                #region 获取用户信息

                //用户名去盐
                loginDTO.UserName = loginDTO.UserName.Replace("_" + loginDTO.CaptchaId, "");
                //密码去盐
                loginDTO.Password = loginDTO.Password.Replace("_" + loginDTO.CaptchaId, "");

                //AES加密
                var aesKey = NGAdminGlobalContext.SystemConfig.AesKey;
                loginDTO.Password = NGAesUtil.Encrypt(loginDTO.Password, aesKey);

                var accounts = (await this.BizAccountRepository.GetListAsync()).
                    Where(t => t.IsDel.Equals(0)).
                    Where(t => t.UserLogonName.Equals(loginDTO.UserName)).
                    Where(t => t.UserPassword.Equals(loginDTO.Password)).
                    Where(t => t.IsEnabled.Equals((int)UserStatus.Enabled));

                var list = accounts.ToList();

                #endregion

                //用户合法
                if (list.Count() == 1)
                {
                    var account = list.FirstOrDefault();
                    var module = this.BizModuleRepository.GetById(account.ModuleId);

                    var token = Guid.NewGuid();
                    var tokenExpiration = NGAdminGlobalContext.SystemConfig.UserTokenExpiration;
                    var expiration = tokenExpiration * 60;

                    var accountDTO = mapper.Map<BizAccountDTO>(account);
                    accountDTO.Token = token.ToString();
                    await this.redisRepository.SetAsync(NGAdminGlobalContext.RedisConfig.SynchronizationPattern + token, accountDTO, expiration);

                    actionResult.HttpStatusCode = HttpStatusCode.OK;
                    actionResult.Message = new String("OK");
                    actionResult.Object = token;

                    #region 记录登录日志

                    var log = new SysLog();

                    log.Id = Guid.NewGuid();

                    log.UserId = account.Id;
                    log.UserName = account.UserDisplayName + "/" + account.UserLogonName;
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

                    log.OperationType = OperationType.GetSyncToken;
                    log.RequestMethod = context.HttpContext.Request.Method;
                    log.RequestUrl = "/API/BizUserManagement/GetToken";
                    log.Params = String.Empty;

                    log.Result = String.Empty;
                    log.OldValue = String.Empty;
                    log.NewValue = String.Empty;
                    log.Remark = $"{account.UserDisplayName}于{DateTime.Now}访问了{log.RequestUrl}接口";

                    log.IsDel = (int)DeletionType.Undeleted;
                    log.Creator = account.Id;
                    log.CreateTime = DateTime.Now;
                    log.Modifier = account.Id;
                    log.ModifyTime = DateTime.Now;
                    log.VersionId = Guid.NewGuid();

                    //记录审计日志
                    await log.WriteAsync();

                    #endregion
                }
                else
                {
                    throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException("account is invalid");
                }
            }
            else
            {
                throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException("salt is invalid");
            }

            return actionResult;
        }

        #endregion
    }
}
