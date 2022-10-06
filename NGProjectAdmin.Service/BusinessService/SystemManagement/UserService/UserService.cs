
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.System;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.BusinessRepository.MQRepository;
using NGProjectAdmin.Repository.BusinessRepository.RedisRepository;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.OrgUserRepository;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.UserRepository;
using NGProjectAdmin.Service.Base;
using NGProjectAdmin.Service.BusinessService.SystemManagement.LogService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.UserService
{
    /// <summary>
    /// 用户业务层实现
    /// </summary>
    public class UserService : NGAdminBaseService<SysUser>, IUserService
    {
        #region 属性及构造函数

        /// <summary>
        /// 用户仓储实例
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// 用户与机构仓储实例
        /// </summary>
        private readonly IOrgUserRepository orgUserRepository;

        /// <summary>
        /// AutoMapper实例
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Redis仓储实例
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// MQ仓储实例
        /// </summary>
        private readonly IMQRepository mqRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="orgUserRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="redisRepository"></param>
        /// <param name="context"></param>
        public UserService(IUserRepository userRepository,
            IOrgUserRepository orgUserRepository,
            IMapper mapper,
            IRedisRepository redisRepository,
            IHttpContextAccessor context,
            IMQRepository mqRepository
            ) : base(userRepository)
        {
            this.userRepository = userRepository;
            this.orgUserRepository = orgUserRepository;
            this.mapper = mapper;
            this.redisRepository = redisRepository;
            this.context = context;
            this.mqRepository = mqRepository;
        }

        #endregion

        #region 业务层公有方法

        #region 删除系统用户

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>ActionResult</returns>
        public async Task<ActionResult> DeleteEntity(Guid userId)
        {
            //删除用户
            await this.DeleteUser(userId);

            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");

            return actionResult;
        }

        #endregion

        #region 批量删除用户

        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="ids">编号组</param>
        /// <returns>ActionResult</returns>
        public async Task<ActionResult> DeleteEntities(String ids)
        {
            var array = NGStringUtil.GetGuids(ids);

            foreach (var item in array)
            {
                //删除用户
                await this.DeleteUser(item);
            }

            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");

            return actionResult;
        }

        #endregion

        #region 系统用户登录

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginDTO">登录信息</param>
        /// <returns>ActionResult</returns>
        public async Task<ActionResult> Logon(LoginDTO login)
        {
            var prikey = NGAdminGlobalContext.SystemConfig.RsaPrivateKey;

            #region 用户信息解密

            login.UserName = NGRsaUtil.PemDecrypt(login.UserName, prikey);
            login.Password = NGRsaUtil.PemDecrypt(login.Password, prikey);
            login.CaptchaId = NGRsaUtil.PemDecrypt(login.CaptchaId, prikey);
            login.Captcha = NGRsaUtil.PemDecrypt(login.Captcha, prikey);

            #endregion

            var obj = new Object();

            var resultNum = await this.redisRepository.GetAsync(login.CaptchaId);

            if (resultNum != null && resultNum.Equals(login.Captcha))
            {
                //删除验证码
             //   await this.redisRepository.DeleteAsync(new String[] { login.CaptchaId });

                #region 获取用户信息

                //用户名去盐
                login.UserName = login.UserName.Replace("_" + login.CaptchaId, "");
                //密码去盐
                login.Password = login.Password.Replace("_" + login.CaptchaId, "");

                var users = await this.redisRepository.GetAsync<List<SysUserDTO>>(NGAdminGlobalContext.SystemCacheConfig.UserCacheName);
                var tempUser = users.Where(t => t.IsDel.Equals(0)).Where(t => t.LogonName.Equals(login.UserName)).FirstOrDefault();
                if (tempUser == null)
                {
                    throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException("user is invalid");
                }

                //AesKey
                var aesKey = NGAdminGlobalContext.SystemConfig.AesKey;
                //AES加密
                login.Password = NGAesUtil.Encrypt(login.Password + tempUser.Salt, aesKey);

                var list = users.Where(t => t.IsDel.Equals(0)).
                                 Where(t => t.LogonName.Equals(login.UserName)).
                                 Where(t => t.Password.Equals(login.Password)).
                                 Where(t => t.IsEnabled.Equals((int)UserStatus.Enabled)).ToList();

                #endregion

                if (list.Count() == 1)
                {
                    #region 用户状态合法

                    var user = list.FirstOrDefault();

                    var tokenKey = NGAdminGlobalContext.SystemConfig.TokenKey;
                    var tokenExpiration = NGAdminGlobalContext.SystemConfig.UserTokenExpiration;
                    var limitCount = NGAdminGlobalContext.SystemConfig.LogonCountLimit;

                    if (limitCount > 0 && limitCount.Equals(1))
                    {
                        #region 仅允许一次登录

                        var pattern = $"{NGAdminGlobalContext.RedisConfig.Pattern + user.Id + "_"}*";
                        var keys = this.redisRepository.PatternSearch(pattern);
                        if (keys.Count() > 0)
                        {
                            foreach (var item in keys)
                            {
                                //await this.redisRepository.DeleteAsync(new string[] { item.ToString() });

                                #region 强制用户下线

                                //注销用户
                                await this.Logout(item, OperationType.ForceLogout, $"{user.DisplayName + "/" + user.LogonName}由于仅允许一次登录被挤下线");

                                var msg = new SystemMessage();
                                msg.Message = "ForceLogout";
                                msg.MessageType = MessageType.ForceLogout;
                                msg.Object = user;

                                this.mqRepository.SendTopic(JsonConvert.SerializeObject(msg));

                                #endregion
                            }
                        }

                        var token = String.Format(tokenKey, user.Id, Guid.NewGuid());
                        user.Token = token;
                        user.TokenExpiration = tokenExpiration * 60;
                        //登录授权
                        obj = await this.GetPermissions(user);

                        #endregion
                    }
                    else if (limitCount > 1)
                    {
                        #region 允许多次登录

                        var pattern = $"{NGAdminGlobalContext.RedisConfig.Pattern + user.Id + "_"}*";
                        var keys = this.redisRepository.PatternSearch(pattern);
                        var count = keys.Count();

                        //达到登录上限
                        if (count.Equals(limitCount))
                        {
                            throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException("logon count limited");
                        }
                        //未达上限
                        else if (count < limitCount)
                        {
                            var token = String.Format(tokenKey, user.Id, Guid.NewGuid());
                            user.Token = token;
                            user.TokenExpiration = tokenExpiration * 60;
                            //登录授权
                            obj = await this.GetPermissions(user);
                        }

                        #endregion
                    }

                    #region 记录登录日志

                    var log = new SysLog();

                    log.Id = Guid.NewGuid();
                    log.UserId = user.Id;
                    log.UserName = user.DisplayName + "/" + user.LogonName;

                    log.OrgId = user.OrgId;
                    if (log.OrgId.Equals(Guid.Empty))
                    {
                        log.OrgName = "none";
                    }
                    else
                    {
                        log.OrgName = user.OrgName;
                    }

                    log.System = this.context.HttpContext.Request.Headers["User-Agent"].FirstOrDefault().ToString().Split('(')[1].Split(')')[0];
                    log.Browser = this.context.HttpContext.Request.Headers["sec-ch-ua"];

                    var ip = context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                    if (string.IsNullOrEmpty(ip))
                    {
                        ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
                    }
                    log.IP = ip;

                    log.OperationType = OperationType.Logon;
                    log.RequestMethod = context.HttpContext.Request.Method;
                    log.RequestUrl = "/API/UserManagement/Logon";
                    log.Params = String.Empty;

                    log.Result = String.Empty;
                    log.OldValue = String.Empty;
                    log.NewValue = String.Empty;
                    log.Remark = $"{user.DisplayName}于{DateTime.Now}访问了{log.RequestUrl}接口";

                    log.IsDel = (int)DeletionType.Undeleted;
                    log.Creator = user.Id;
                    log.CreateTime = DateTime.Now;
                    log.Modifier = user.Id;
                    log.ModifyTime = DateTime.Now;
                    log.VersionId = Guid.NewGuid();

                    //记录审计日志
                    await log.WriteAsync();

                    #endregion

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

        #region 用户退出登录

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="operationType">退出类型</param>
        /// <param name="remark">说明信息</param>
        /// <returns>ActionResult</returns>
        public async Task<ActionResult> Logout(String token, OperationType operationType = OperationType.Logout, String remark = null)
        {
            #region 记录登出日志

            //获取用户
            var user = await this.redisRepository.GetAsync<SysUserDTO>(token);
            if (user == null)
            {
                throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException("token is invalid");
            }

            var log = new SysLog();

            log.Id = Guid.NewGuid();
            log.UserId = user.Id;
            log.UserName = user.DisplayName + "/" + user.LogonName;

            log.OrgId = user.OrgId;
            if (log.OrgId.Equals(Guid.Empty))
            {
                log.OrgName = "none";
            }
            else
            {
                log.OrgName = user.OrgName;
            }

            log.System = this.context.HttpContext.Request.Headers["User-Agent"].FirstOrDefault().ToString().Split('(')[1].Split(')')[0];
            log.Browser = this.context.HttpContext.Request.Headers["sec-ch-ua"];

            var ip = context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            log.IP = ip;

            log.OperationType = operationType;
            log.RequestMethod = context.HttpContext.Request.Method;
            log.RequestUrl = "/API/UserManagement/Logout";
            log.Params = String.Empty;

            log.Result = String.Empty;
            log.OldValue = String.Empty;
            log.NewValue = String.Empty;
            log.Remark = String.IsNullOrEmpty(remark) ? $"{user.DisplayName}于{DateTime.Now}访问了{log.RequestUrl}接口" : remark;

            log.IsDel = (int)DeletionType.Undeleted;
            log.Creator = user.Id;
            log.CreateTime = DateTime.Now;
            log.Modifier = user.Id;
            log.ModifyTime = DateTime.Now;
            log.VersionId = Guid.NewGuid();

            //记录审计日志
            await log.WriteAsync();

            #endregion

            if (!String.IsNullOrEmpty(token))
            {
                await this.redisRepository.DeleteAsync(new String[] { token });
            }

            var actionResult = new ActionResult();
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");

            return actionResult;
        }

        #endregion

        #region 更新用户密码

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="data">参数</param>
        /// <returns>ActionResult</returns>
        public async Task<ActionResult> UpdatePassword(PasswordDTO data)
        {
            var prikey = NGAdminGlobalContext.SystemConfig.RsaPrivateKey;

            var userId = NGRsaUtil.PemDecrypt(data.UserId, prikey);
            var password = NGRsaUtil.PemDecrypt(data.Password, prikey);
            var salt = NGRsaUtil.PemDecrypt(data.Salt, prikey);

            //密码去盐
            password = password.Replace("_" + salt, "");
            var aesKey = NGAdminGlobalContext.SystemConfig.AesKey;
            //AES加密
            password = NGAesUtil.Encrypt(password, aesKey);

            var user = await this.userRepository.GetByIdAsync(Guid.Parse(userId));
            user.Password = password;
            await this.userRepository.UpdateEntityAsync(user);

            #region 数据一致性维护

            var users = await this.redisRepository.GetAsync<List<SysUserDTO>>(NGAdminGlobalContext.SystemCacheConfig.UserCacheName);
            var entity = users.Where(t => t.Id == user.Id).FirstOrDefault();

            if (entity != null)
            {
                users.Remove(entity);
                entity.Password = user.Password;
                users.Add(entity);
            }

            await this.redisRepository.SetAsync(NGAdminGlobalContext.SystemCacheConfig.UserCacheName, users, -1);

            #endregion

            var actionResult = new ActionResult();
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");

            return actionResult;
        }

        #endregion

        #region 获取在线用户

        /// <summary>
        /// 获取在线用户
        /// </summary>
        /// <returns>QueryResult</returns>
        public async Task<QueryResult<SysUserDTO>> GetOnlineUsers()
        {
            var list = new List<SysUserDTO>();

            var pattern = $"{NGAdminGlobalContext.RedisConfig.Pattern}*";
            var keys = this.redisRepository.PatternSearch(pattern);
            foreach (var key in keys)
            {
                list.Add(await this.redisRepository.GetAsync<SysUserDTO>(key));
            }

            var queryResult = new QueryResult<SysUserDTO>();
            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            queryResult.TotalCount = list.Count;
            queryResult.List = list;

            return queryResult;
        }

        #endregion

        #region 加载系统用户缓存

        /// <summary>
        /// 加载系统用户缓存
        /// </summary>
        public async Task LoadSystemUserCache()
        {
            var sqlKey = "sqls:sql:query_sysuser";
            var strSQL = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value;

            int totalCount = 0;
            var users = (await this.userRepository.SqlQueryAsync<SysUserDTO>(new QueryCondition(), totalCount, strSQL)).ToList();
            await this.redisRepository.SetAsync(NGAdminGlobalContext.SystemCacheConfig.UserCacheName, users, -1);

            NGLoggerContext.Info("系统用户缓存加载完成");
        }

        #endregion

        #region 清理系统用户缓存

        /// <summary>
        /// 清理系统用户缓存
        /// </summary>
        public async Task ClearSystemUserCache()
        {
            await this.redisRepository.DeleteAsync(new String[] { NGAdminGlobalContext.SystemCacheConfig.UserCacheName });

            NGLoggerContext.Info("系统用户缓存清理完成");
        }

        #endregion

        #endregion

        #region 业务层私有方法

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户编号</param>
        private async Task DeleteUser(Guid userId)
        {
            //解除用户与机构关系
            var task = await this.orgUserRepository.GetListAsync();
            var orgUserList = task.Where(t => t.IsDel.Equals(0) && t.UserId.Equals(userId)).ToList();

            foreach (var item in orgUserList)
            {
                await this.orgUserRepository.DeleteEntityAsync(item.Id);
            }

            //删除用户
            await this.userRepository.DeleteEntityAsync(userId);
        }

        /// <summary>
        /// 用户登录授权
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>Object</returns>
        private async Task<Object> GetPermissions(SysUserDTO user)
        {
            var menuList = new List<SysMenuDTO>();

            //超级用户
            if (user.IsSupperAdmin.Equals((int)YesNo.YES) && user.OrgId.Equals(Guid.Empty))
            {
                menuList = await this.redisRepository.GetAsync<List<SysMenuDTO>>(NGAdminGlobalContext.SystemCacheConfig.MenuCacheName);
            }
            //普通用户
            else
            {
                var roleUsers = await this.redisRepository.GetAsync<List<SysRoleUser>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndUserCacheName);
                var roleIds = roleUsers.Where(t => t.IsDel.Equals(0) && t.UserId.Equals(user.Id)).Select(t => t.RoleId).ToArray();

                if (roleIds.Length > 0)
                {
                    var roleMenus = await this.redisRepository.GetAsync<List<SysRoleMenu>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndMenuCacheName);
                    var menus = await this.redisRepository.GetAsync<List<SysMenuDTO>>(NGAdminGlobalContext.SystemCacheConfig.MenuCacheName);

                    foreach (var roleId in roleIds)
                    {
                        var menuIds = roleMenus.Where(t => t.IsDel.Equals(0) && t.RoleId.Equals(roleId)).Select(t => t.MenuId);
                        foreach (var menuId in menuIds)
                        {
                            var menu = menus.Where(t => t.Id == menuId).FirstOrDefault();
                            if (menu != null && menuList.Where(t => t.Id.Equals(menu.Id)).Count().Equals(0))
                            {
                                menuList.Add(menu);
                            }
                        }
                    }
                }
            }

            var permissions = new List<SysMenuDTO>();

            //一级菜单
            var parentNodes = menuList.Where(t => t.MenuType.Equals(MenuType.Menu) && t.ParentId == null).OrderBy(t => t.SerialNumber).ToList();
            foreach (var node in parentNodes)
            {
                var parent = mapper.Map<SysMenuDTO>(node);
                parent.Children = new List<SysMenuDTO>();

                //二级菜单
                var menuSons = menuList.Where(t => t.ParentId.Equals(node.Id) && t.MenuType == MenuType.Menu).OrderBy(t => t.SerialNumber).ToList();
                foreach (var subNode in menuSons)
                {
                    //三级菜单：按钮、视图
                    var subItems = menuList.Where(t => t.ParentId.Equals(subNode.Id) && (t.MenuType == MenuType.Button || t.MenuType == MenuType.View)).
                                        OrderBy(t => t.SerialNumber).ToList();

                    if (subItems.Count > 0)
                    {
                        var sonNode = mapper.Map<SysMenuDTO>(subNode);
                        sonNode.Children = new List<SysMenuDTO>();

                        var children = mapper.Map<List<SysMenuDTO>>(subItems);
                        sonNode.Children.AddRange(children);

                        parent.Children.Add(sonNode);
                    }
                }

                permissions.Add(parent);
            }

            //初始化菜单多语
            await this.InitLanguages(permissions);
            var codeGeneratorConfig = NGAdminGlobalContext.CodeGeneratorConfig;

            //初始化用户口令
            await this.redisRepository.SetAsync(user.Token, user, user.TokenExpiration);

            //装填返回结果
            var obj = new { user, permissions, codeGeneratorConfig };

            return obj;
        }

        /// <summary>
        /// 初始化多语
        /// </summary>
        /// <param name="list">菜单列表</param>
        private async Task InitLanguages(List<SysMenuDTO> list)
        {
            if (list.Count > 0)
            {
                var languages = await this.redisRepository.GetAsync<List<SysLanguage>>(NGAdminGlobalContext.SystemCacheConfig.LanguageCacheName);
                var lanEn = languages.Where(t => t.LanguageName.Equals("en-US")).FirstOrDefault();
                var lanRu = languages.Where(t => t.LanguageName.Equals("ru-RU")).FirstOrDefault();

                var menuLanguages = await this.redisRepository.GetAsync<List<SysMenuLanguage>>(NGAdminGlobalContext.SystemCacheConfig.MenuAndLanguageCacheName);

                foreach (var item in list)
                {
                    var enMenu = menuLanguages.Where(t => t.MenuId.Equals(item.Id) && t.LanguageId.Equals(lanEn.Id) && t.IsDel.Equals(0)).FirstOrDefault();
                    var ruMenu = menuLanguages.Where(t => t.MenuId.Equals(item.Id) && t.LanguageId.Equals(lanRu.Id) && t.IsDel.Equals(0)).FirstOrDefault();

                    if (enMenu != null)
                    {
                        item.MenuNameEn = enMenu.MenuName;
                    }

                    if (ruMenu != null)
                    {
                        item.MenuNameRu = ruMenu.MenuName;
                    }

                    if (item.Children != null && item.Children.Count > 0)
                    {
                        await this.InitLanguages(item.Children);
                    }
                }
            }
        }

        #endregion
    }
}
