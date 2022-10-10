//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Class.Extensions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.System;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Entity.CoreEnum;
using NGProjectAdmin.Service.BusinessService.MQService;
using NGProjectAdmin.Service.BusinessService.RedisService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.ImportService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.OrganizationService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.OrgUserService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.UserService;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ActionResult = NGProjectAdmin.Entity.CoreEntity.ActionResult;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 用户管理控制器
    /// </summary>
    public class UserManagementController : NGAdminBaseController<SysUser>
    {
        #region 属性及构造函数

        /// <summary>
        /// 用户接口实例
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// 机构与用户接口实例
        /// </summary>
        private readonly IOrgUserService orgUserService;

        /// <summary>
        /// AutoMapper实例
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Redis接口实例
        /// </summary>
        private readonly IRedisService redisService;

        /// <summary>
        /// ActiveMQ接口实例
        /// </summary>
        private readonly IMQService mqService;

        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 导入配置服务接口
        /// </summary>
        private readonly IImportConfigService ImportConfigService;

        /// <summary>
        /// 机构服务接口
        /// </summary>
        private readonly IOrganizationService OrganizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="orgUserService"></param>
        /// <param name="mapper"></param>
        /// <param name="redisService"></param>
        /// <param name="mqService"></param>
        /// <param name="context"></param>
        /// <param name="ImportConfigService"></param>
        /// <param name="OrganizationService"></param>
        public UserManagementController(IUserService userService,
                                        IOrgUserService orgUserService,
                                        IMapper mapper,
                                        IRedisService redisService,
                                        IMQService mqService,
                                        IHttpContextAccessor context,
                                        IImportConfigService ImportConfigService,
                                        IOrganizationService OrganizationService) : base(userService)
        {
            this.userService = userService;
            this.orgUserService = orgUserService;
            this.mapper = mapper;
            this.redisService = redisService;
            this.mqService = mqService;
            this.context = context;
            this.ImportConfigService = ImportConfigService;
            this.OrganizationService = OrganizationService;
        }

        #endregion

        #region 查询用户列表

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        [Permission("user:query:list")]
        public async Task<IActionResult> Post(QueryCondition queryCondition)
        {
            if (queryCondition.QueryItems.Count.Equals(0))
            {
                queryCondition.QueryItems = new List<QueryItem>();
                queryCondition.QueryItems.Add(new QueryItem()
                {
                    Field = "OrgId",
                    DataType = DataType.Guid,
                    QueryMethod = QueryMethod.Equal,
                    Value = NGAdminSessionContext.GetUserOrgId(this.context)
                });
            }

            var users = await this.redisService.GetAsync<List<SysUserDTO>>(NGAdminGlobalContext.SystemCacheConfig.UserCacheName);
            users = users.AsQueryable().Where(QueryCondition.BuildExpression<SysUserDTO>(queryCondition.QueryItems)).ToList();

            if (!String.IsNullOrEmpty(queryCondition.Sort))
            {
                users = users.Sort<SysUserDTO>(queryCondition.Sort);
            }

            var actionResult = new QueryResult<SysUserDTO>();
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.TotalCount = users.Count;
            actionResult.List = users.Skip(queryCondition.PageIndex * queryCondition.PageSize).Take(queryCondition.PageSize).ToList();

            return Ok(actionResult);
        }

        #endregion

        #region 查询用户信息

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{userId}")]
        [Log(OperationType.QueryEntity)]
        [Permission("user:query:list")]
        public async Task<IActionResult> GetById(Guid userId)
        {
            var actionResult = new Entity.CoreEntity.ActionResult();
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            var users = await this.redisService.GetAsync<List<SysUserDTO>>(NGAdminGlobalContext.SystemCacheConfig.UserCacheName);
            actionResult.Object = users.Where(t => t.Id == userId).FirstOrDefault();

            return Ok(actionResult);
        }

        #endregion

        #region 用户登录名检测

        /// <summary>
        /// 用户登录名检测
        /// </summary>
        /// <param name="logonName">用户登录名</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{logonName}")]
        [Log(OperationType.QueryEntity)]
        [Permission("user:add:entity")]
        public async Task<IActionResult> IsExistedLogonName(String logonName)
        {
            var actionResult = new Entity.CoreEntity.ActionResult();
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");

            var users = await this.redisService.GetAsync<List<SysUserDTO>>(NGAdminGlobalContext.SystemCacheConfig.UserCacheName);
            var user = users.Where(t => t.LogonName.Equals(logonName)).FirstOrDefault();
            if (user != null)
            {
                actionResult.Object = true;
            }
            else
            {
                actionResult.Object = false;
            }

            return Ok(actionResult);
        }

        #endregion

        #region 新增用户信息

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="userDTO">用户对象</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.AddEntity)]
        [Permission("user:add:entity")]
        public async Task<IActionResult> Add([FromBody] SysUserDTO userDTO)
        {
            var orgId = userDTO.OrgId;
            var orgName = userDTO.OrgName;

            var defaultPassword = NGAdminGlobalContext.SystemConfig.DefaultPassword;
            var aesKey = NGAdminGlobalContext.SystemConfig.AesKey;

            userDTO.Salt = Guid.NewGuid();
            userDTO.Password = NGAesUtil.Encrypt(defaultPassword + userDTO.Salt, aesKey);
            
            //DTO TO POCO
            var user = mapper.Map<SysUser>(userDTO);
            //新增用户
            await this.userService.AddAsync(user);

            //用户与机构关联
            var orgUser = new SysOrgUser();
            orgUser.OrgId = orgId;
            orgUser.UserId = user.Id;
            //新增机构与用户关系
            await this.orgUserService.AddAsync(orgUser);

            #region 数据一致性维护

            userDTO = mapper.Map<SysUserDTO>(user);
            userDTO.OrgId = orgId;
            userDTO.OrgName = orgName;

            var users = await this.redisService.GetAsync<List<SysUserDTO>>(NGAdminGlobalContext.SystemCacheConfig.UserCacheName);
            users.Add(userDTO);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.UserCacheName, users, -1);

            #endregion

            var actionResult = new ActionResult();
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = userDTO;

            return Ok(actionResult);
        }

        #endregion

        #region 编辑用户信息

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="userDTO">用户对象</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Log(OperationType.EditEntity)]
        [Permission("user:edit:entity")]
        public async Task<IActionResult> Put([FromBody] SysUserDTO userDTO)
        {
            var task = await this.userService.GetByIdAsync(userDTO.Id);
            var origin = (SysUser)task.Object;

            userDTO.Password = NGRsaUtil.PemDecrypt(userDTO.Password, NGAdminGlobalContext.SystemConfig.RsaPrivateKey);
            if (!origin.Password.Equals(userDTO.Password))
            {
                userDTO.Salt = Guid.NewGuid();
                userDTO.Password = NGAesUtil.Encrypt(userDTO.Password + userDTO.Salt, NGAdminGlobalContext.SystemConfig.AesKey);
            }

            var orgId = userDTO.OrgId;
            var orgName = userDTO.OrgName;

            //DTO TO POCO
            var user = mapper.Map<SysUser>(userDTO);
            var actionResult = await this.userService.UpdateAsync(user);

            if (user.IsEnabled.Equals(0))
            {
                #region 强制禁用用户下线

                var pattern = $"{NGAdminGlobalContext.RedisConfig.Pattern + user.Id + "_"}*";
                var keys = this.redisService.PatternSearch(pattern);
                if (keys.Count > 0)
                {
                    foreach (var item in keys)
                    {
                        await this.redisService.DeleteAsync(new string[] { item.ToString() });
                    }
                }

                var msg = new SystemMessage();
                msg.Message = "ForceLogout";
                msg.MessageType = MessageType.ForceLogout;
                msg.Object = user;

                this.mqService.SendTopic(JsonConvert.SerializeObject(msg));

                #endregion
            }

            #region 数据一致性维护

            userDTO = mapper.Map<SysUserDTO>(user);
            userDTO.OrgId = orgId;
            userDTO.OrgName = orgName;

            //获取用户缓存
            var users = await this.redisService.GetAsync<List<SysUserDTO>>(NGAdminGlobalContext.SystemCacheConfig.UserCacheName);

            //删除就数据
            var old = users.Where(t => t.Id == userDTO.Id).FirstOrDefault();
            users.Remove(old);

            //添加新数据
            users.Add(userDTO);

            //更新用户缓存
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.UserCacheName, users, -1);

            //更新机构的管理人姓名
            if (origin.DisplayName != userDTO.DisplayName)
            {
                //获取机构缓存
                var orgs = await this.redisService.GetAsync<List<SysOrganizationDTO>>(NGAdminGlobalContext.SystemCacheConfig.OrgCacheName);
                var subs = orgs.Where(t => t.Leader != null && t.Leader == userDTO.Id).ToList();

                foreach (var item in subs)
                {
                    //删除旧数据
                    var oldOrg = item;
                    orgs.Remove(oldOrg);

                    //更新机构管理人姓名
                    item.LeaderName = userDTO.DisplayName;
                    orgs.Add(item);
                }

                //更新机构缓存
                await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.OrgCacheName, orgs, -1);
            }

            #endregion

            return Ok(actionResult);
        }

        #endregion

        #region 删除用户信息

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{userId}")]
        [Permission("user:del:entities")]
        [Log(OperationType.DeleteEntity)]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var actionResult = await this.userService.DeleteEntity(userId);

            #region 数据一致性维护

            var users = await this.redisService.GetAsync<List<SysUserDTO>>(NGAdminGlobalContext.SystemCacheConfig.UserCacheName);
            var user = users.Where(t => t.Id == userId).FirstOrDefault();
            users.Remove(user);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.UserCacheName, users, -1);

            #endregion

            #region 强制用户下线

            var pattern = $"{NGAdminGlobalContext.RedisConfig.Pattern + user.Id + "_"}*";
            var keys = this.redisService.PatternSearch(pattern);
            if (keys.Count > 0)
            {
                foreach (var item in keys)
                {
                    await this.redisService.DeleteAsync(new string[] { item.ToString() });
                }
            }

            var msg = new SystemMessage();
            msg.Message = "ForceLogout";
            msg.MessageType = MessageType.ForceLogout;
            msg.Object = user;

            this.mqService.SendTopic(JsonConvert.SerializeObject(msg));

            #endregion

            return Ok(actionResult);
        }

        #endregion

        #region 批量删除用户

        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="ids">编号数组</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{ids}")]
        [Log(OperationType.DeleteEntity)]
        [Permission("user:del:entities")]
        public async Task<IActionResult> DeleteRange(String ids)
        {
            var actionResult = await this.userService.DeleteEntities(ids);

            #region 数据一致性维护

            var users = await this.redisService.GetAsync<List<SysUserDTO>>(NGAdminGlobalContext.SystemCacheConfig.UserCacheName);
            var array = NGStringUtil.GetGuids(ids);

            foreach (var item in array)
            {
                var user = users.Where(t => t.Id == item).FirstOrDefault();
                users.Remove(user);

                #region 强制用户下线

                var pattern = $"{NGAdminGlobalContext.RedisConfig.Pattern + user.Id + "_"}*";
                var keys = this.redisService.PatternSearch(pattern);
                if (keys.Count > 0)
                {
                    foreach (var key in keys)
                    {
                        await this.redisService.DeleteAsync(new string[] { key.ToString() });
                    }
                }

                var msg = new SystemMessage();
                msg.Message = "ForceLogout";
                msg.MessageType = MessageType.ForceLogout;
                msg.Object = user;

                this.mqService.SendTopic(JsonConvert.SerializeObject(msg));

                #endregion
            }

            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.UserCacheName, users, -1);

            #endregion

            return Ok(actionResult);
        }

        #endregion

        #region 用户登录系统

        /// <summary>
        /// 用户登录系统
        /// </summary>
        /// <param name="loginDTO">登录信息</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [AllowAnonymous]
        //[Log(OperationType.Logon)]
        public async Task<IActionResult> Logon([FromBody] LoginDTO loginDTO)
        {
            var actionResult = await this.userService.Logon(loginDTO);
            return Ok(actionResult);
        }

        #endregion

        #region 用户退出登录

        /// <summary>
        /// 用户退出登录
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{token}")]
        //[Log(OperationType.Logout)]
        public async Task<IActionResult> Logout(String token)
        {
            var actionResult = await this.userService.Logout(token);
            return Ok(actionResult);
        }

        #endregion

        #region 强制用户退出

        /// <summary>
        /// 强制用户退出
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{token}")]
        [Log(OperationType.ForceLogout)]
        public async Task<IActionResult> ForceLogout(String token)
        {
            //获取用户
            var user = await this.redisService.GetAsync<SysUserDTO>(token);

            //注销用户
            var actionResult = await this.userService.Logout(token, OperationType.ForceLogout, $"{user.DisplayName + "/" + user.LogonName}被超级管理员强制下线");

            #region 强制用户下线

            var msg = new SystemMessage();
            msg.Message = "ForceLogout";
            msg.MessageType = MessageType.ForceLogout;
            msg.Object = user;

            this.mqService.SendTopic(JsonConvert.SerializeObject(msg));

            #endregion

            return Ok(actionResult);
        }

        #endregion

        #region 按机构获取用户

        /// <summary>
        /// 按机构获取用户
        /// </summary>
        /// <param name="orgId">机构编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{orgId}")]
        [Log(OperationType.QueryList)]
        public async Task<IActionResult> GetUserByOrgId(Guid orgId)
        {
            var users = await this.redisService.GetAsync<List<SysUserDTO>>(NGAdminGlobalContext.SystemCacheConfig.UserCacheName);
            users = users.Where(t => t.OrgId == orgId).ToList();

            var actionResult = new QueryResult<SysUserDTO>();
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.TotalCount = users.Count;
            actionResult.List = users;

            return Ok(actionResult);
        }

        #endregion

        #region 更新用户密码

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="data">参数</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.UpdatePassword)]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordDTO data)
        {
            var actionResult = await this.userService.UpdatePassword(data);
            return Ok(actionResult);
        }

        #endregion

        #region 获取在线用户

        /// <summary>
        /// 获取在线用户
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Log(OperationType.QueryList)]
        [Permission("user:query:onlineUsers")]
        public async Task<IActionResult> GetOnlineUsers()
        {
            var actionResult = await this.userService.GetOnlineUsers();
            return Ok(actionResult);
        }

        #endregion

        #region 导入用户信息

        /// <summary>
        /// 导入用户信息
        /// </summary>
        /// <param name="file">excel文件</param>
        /// <param name="orgId">moduleId</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.ImportData)]
        [Permission("user:import:entities")]
        public async Task<IActionResult> Import([FromForm] IFormFile file, Guid orgId)
        {
            var actionResult = new Entity.CoreEntity.ActionResult();

            if (file != null)
            {
                #region 常规合法性校验

                //获取文件拓展名
                var extension = Path.GetExtension(file.FileName);
                //文件保存路径
                var filePath = NGFileContext.SaveFormFile(file);

                var configDTO = this.ImportConfigService.GetImportConfig("UserImportConfig");
                if (configDTO == null)
                {
                    throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException("can not find import config");
                }

                configDTO.ExcelPath = filePath;
                //常规合法性校验
                var errorCount = configDTO.ValidationDetecting();

                #endregion

                if (errorCount > 0)
                {
                    #region 常规校验出不合规项

                    actionResult.Object = errorCount;
                    actionResult.Message = configDTO.ExcelPath.Replace(extension, "").Split('/')[1];

                    #endregion
                }
                else
                {
                    #region 常规业务性校验

                    var users = (List<SysUser>)(await this.userService.GetListAsync()).Object;
                    //users = users.Where(t => t.IsDel == 0).ToList();

                    var xlxStream = new FileStream(configDTO.ExcelPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    var workbook = new HSSFWorkbook(xlxStream);
                    var worksheet = workbook.GetSheetAt(0);

                    for (var i = configDTO.StartRow; i <= worksheet.LastRowNum; i++)
                    {
                        var value = worksheet.GetRow(i).GetCell(2).GetCellValue().Trim();
                        if (!String.IsNullOrEmpty(value))
                        {
                            var user = users.Where(t => t.LogonName.Equals(value)).FirstOrDefault();
                            if (user != null)
                            {
                                errorCount++;
                                worksheet.SetCellComment(i, 2, "用户已存在！");
                            }
                        }
                    }

                    #endregion

                    if (errorCount > 0)
                    {
                        #region 业务校验出不合规项

                        var xlxPath = configDTO.ExcelPath;
                        configDTO.ExcelPath = workbook.SaveAsXlx(xlxPath);

                        xlxStream.Close();
                        System.IO.File.Delete(xlxPath);

                        actionResult.Object = errorCount;
                        actionResult.Message = configDTO.ExcelPath.Replace(extension, "").Split('/')[1];

                        #endregion
                    }
                    else
                    {
                        #region 执行业务导入

                        var userList = await this.redisService.GetAsync<List<SysUserDTO>>(NGAdminGlobalContext.SystemCacheConfig.UserCacheName);

                        var org = (SysOrganization)(await this.OrganizationService.GetByIdAsync(orgId)).Object;
                        var defaultPassword = NGAdminGlobalContext.SystemConfig.DefaultPassword;
                        var aesKey = NGAdminGlobalContext.SystemConfig.AesKey;

                        //映射工作簿列与对象属性之间的关系
                        Dictionary<String, String> dictionary = new Dictionary<string, string>();
                        dictionary.Add("用户姓名", "DisplayName");
                        dictionary.Add("登录账号", "LogonName");
                        dictionary.Add("性别", "Sex");
                        dictionary.Add("手机号", "MobilePhone");
                        dictionary.Add("座机号", "Telephone");
                        dictionary.Add("电子邮件", "Email");

                        var list = worksheet.ToList<SysUser>(configDTO.StartRow, configDTO.StartColumn, dictionary);
                        foreach (var user in list)
                        {
                            //开始事务
                            var result = await this.userService.UseTransactionAsync(async () =>
                            {
                                user.Salt = Guid.NewGuid();
                                user.Password = NGAesUtil.Encrypt(defaultPassword + user.Salt, aesKey);
                                user.IsEnabled = (int)YesNo.YES;
                                //新增用户
                                await this.userService.AddAsync(user);

                                //用户与机构关联
                                var orgUser = new SysOrgUser();
                                orgUser.OrgId = org.Id;
                                orgUser.UserId = user.Id;
                                //新增机构与用户关系
                                await this.orgUserService.AddAsync(orgUser);
                            });

                            if (result.IsSuccess)
                            {
                                #region 数据一致性维护

                                var userDTO = mapper.Map<SysUserDTO>(user);
                                userDTO.OrgId = org.Id;
                                userDTO.OrgName = org.OrgName;

                                userList.Add(userDTO);

                                #endregion
                            }
                        }

                        await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.UserCacheName, userList, -1);

                        workbook.Close();
                        xlxStream.Close();

                        #endregion
                    }
                }

                actionResult.HttpStatusCode = HttpStatusCode.OK;
            }
            else
            {
                actionResult.HttpStatusCode = HttpStatusCode.NoContent;
                actionResult.Message = new String("NoContent");
            }

            return Ok(actionResult);
        }

        #endregion

        #region 导出用户信息

        /// <summary>
        /// 导出用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Log(OperationType.ExportData)]
        [Permission("user:import:entities")]
        public async Task<IActionResult> ExportExcel()
        {
            var users = await this.redisService.GetAsync<List<SysUserDTO>>(NGAdminGlobalContext.SystemCacheConfig.UserCacheName);

            //剔除超级用户
            var super = users.Where(t => t.IsSupperAdmin == 1 && t.OrgId == Guid.Empty).ToList();
            users.RemoveRange(super);

            var currentUser = NGAdminSessionContext.GetCurrentUserInfo(this.context);
            if (currentUser.IsSupperAdmin == 0)
            {
                //获取用户机构编号
                var orgId = NGAdminSessionContext.GetUserOrgId(this.context);
                users = users.Where(r => r.OrgId == orgId).ToList();
                users = users.OrderBy(t => t.SerialNumber).ToList();
            }

            var stream = users.Export().ToStream();
            stream.Position = 0;

            return File(stream, "application/octet-stream", "用户信息.xls");
        }

        #endregion
    }
}
