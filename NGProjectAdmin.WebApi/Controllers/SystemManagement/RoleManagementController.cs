//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Class.Extensions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.BusinessService.RedisService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.ImportService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.RoleMenuService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.RoleOrgService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.RoleService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.RoleUserService;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 角色管理控制器
    /// </summary>
    public class RoleManagementController : NGAdminBaseController<SysRole>
    {
        #region 属性及构造函数

        /// <summary>
        /// 角色服务接口实例
        /// </summary>
        private readonly IRoleService roleService;

        /// <summary>
        /// 角色用户服务接口
        /// </summary>
        private readonly IRoleUserService roleUserService;

        /// <summary>
        /// 角色菜单服务接口
        /// </summary>
        private readonly IRoleMenuService roleMenuService;

        /// <summary>
        /// 导入配置服务接口
        /// </summary>
        private readonly IImportConfigService importConfigService;

        /// <summary>
        /// 角色机构服务接口
        /// </summary>
        private readonly IRoleOrgService roleOrgService;

        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// Redis接口实例
        /// </summary>
        private readonly IRedisService redisService;

        /// <summary>
        /// AutoMapper实例
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleService">角色服务实例</param>
        /// <param name="roleUserService">角色用户服务实例</param>
        /// <param name="roleMenuService">角色菜单服务实例</param>
        /// <param name="importConfigService">导入配置服务实例</param>
        /// <param name="roleOrgService">角色机构服务实例</param>
        ///<param name="context">Http会话实例</param>
        /// <param name="redisService">redis服务实例</param>
        /// <param name="mapper">AutoMapper实例</param>
        public RoleManagementController(IRoleService roleService,
                                        IRoleUserService roleUserService,
                                        IRoleMenuService roleMenuService,
                                        IImportConfigService importConfigService,
                                        IRoleOrgService roleOrgService,
                                        IHttpContextAccessor context,
                                        IRedisService redisService,
                                        IMapper mapper) : base(roleService)
        {
            this.roleService = roleService;
            this.roleUserService = roleUserService;
            this.roleMenuService = roleMenuService;
            this.importConfigService = importConfigService;
            this.roleOrgService = roleOrgService;
            this.context = context;
            this.redisService = redisService;
            this.mapper = mapper;
        }

        #endregion

        #region 查询角色列表

        /// <summary>
        /// 查询角色列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        [Permission("role:query:list")]
        public async Task<IActionResult> Post(QueryCondition queryCondition)
        {
            var roles = await this.redisService.GetAsync<List<SysRoleDTO>>(NGAdminGlobalContext.SystemCacheConfig.RoleCacheName);

            //获取用户机构编号
            var orgId = NGAdminSessionContext.GetUserOrgId(this.context);
            roles = roles.Where(r => r.OrgId == orgId).ToList();

            roles = roles.AsQueryable().Where(QueryCondition.BuildExpression<SysRoleDTO>(queryCondition.QueryItems)).ToList();

            if (!String.IsNullOrEmpty(queryCondition.Sort))
            {
                roles = roles.Sort<SysRoleDTO>(queryCondition.Sort);
            }

            var actionResult = new QueryResult<SysRoleDTO>();
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.TotalCount = roles.Count;
            actionResult.List = roles.Skip(queryCondition.PageIndex * queryCondition.PageSize).Take(queryCondition.PageSize).ToList();

            return Ok(actionResult);
        }

        #endregion

        #region 查询角色信息

        /// <summary>
        /// 查询角色信息
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{roleId}")]
        [Log(OperationType.QueryEntity)]
        [Permission("role:query:list")]
        public async Task<IActionResult> GetById(Guid roleId)
        {
            var actionResult = new Entity.CoreEntity.ActionResult();
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            var roles = await this.redisService.GetAsync<List<SysRoleDTO>>(NGAdminGlobalContext.SystemCacheConfig.RoleCacheName);
            actionResult.Object = roles.Where(t => t.Id == roleId).FirstOrDefault();

            return Ok(actionResult);
        }

        #endregion

        #region 新增系统角色

        /// <summary>
        /// 新增系统角色
        /// </summary>
        /// <param name="role">角色对象</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.AddEntity)]
        [Permission("role:add:entity")]
        public async Task<IActionResult> Add([FromBody] SysRole role)
        {
            var actionResult = await this.roleService.AddAsync(role);

            var roleOrg = new SysRoleOrg();
            roleOrg.RoleId = role.Id;
            //获取用户机构编号
            var orgId = NGAdminSessionContext.GetUserOrgId(this.context);
            roleOrg.OrgId = orgId;
            roleOrg.OwnerType = (int)RoleType.Own;
            await this.roleOrgService.AddAsync(roleOrg);

            #region 数据一致性维护

            var roles = await this.redisService.GetAsync<List<SysRoleDTO>>(NGAdminGlobalContext.SystemCacheConfig.RoleCacheName);
            var roleDTO = mapper.Map<SysRoleDTO>(role);
            roleDTO.OrgId = roleOrg.OrgId;
            roles.Add(roleDTO);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleCacheName, roles, -1);

            var roleOrgs = await this.redisService.GetAsync<List<SysRoleOrg>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndOrgCacheName);
            roleOrgs.Add(roleOrg);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleAndOrgCacheName, roleOrgs, -1);

            #endregion

            return Ok(actionResult);
        }

        #endregion

        #region 编辑系统角色

        /// <summary>
        /// 编辑系统角色
        /// </summary>
        /// <param name="role">角色对象</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Log(OperationType.EditEntity)]
        [Permission("role:edit:entity")]
        public async Task<IActionResult> Put([FromBody] SysRole role)
        {
            var actionResult = await this.roleService.UpdateAsync(role);

            #region 数据一致性维护

            var roles = await this.redisService.GetAsync<List<SysRoleDTO>>(NGAdminGlobalContext.SystemCacheConfig.RoleCacheName);

            var old = roles.Where(t => t.Id == role.Id).FirstOrDefault();

            var roleDTO = mapper.Map<SysRoleDTO>(role);
            roleDTO.OrgId = old.OrgId;
            roles.Add(roleDTO);

            roles.Remove(old);

            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleCacheName, roles, -1);

            #endregion

            return Ok(actionResult);
        }

        #endregion

        #region 删除系统角色

        /// <summary>
        /// 删除系统角色
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{roleId}")]
        [Permission("role:del:entities")]
        [Log(OperationType.DeleteEntity)]
        public async Task<IActionResult> Delete(Guid roleId)
        {
            //删除检测
            await this.DeleteCheck(roleId.ToString());

            //删除角色
            var actionResult = await this.roleService.DeleteAsync(roleId);
            //删除角色与机构关系
            var roleOrgs = await this.redisService.GetAsync<List<SysRoleOrg>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndOrgCacheName);
            var sysRoleOrg = roleOrgs.Where(t => t.RoleId.Equals(roleId) && t.IsDel == 0 && t.OwnerType == (int)RoleType.Own).FirstOrDefault();
            await this.roleOrgService.DeleteAsync(sysRoleOrg.Id);

            #region 数据一致性维护

            var roles = await this.redisService.GetAsync<List<SysRoleDTO>>(NGAdminGlobalContext.SystemCacheConfig.RoleCacheName);
            var old = roles.Where(t => t.Id == roleId).FirstOrDefault();
            roles.Remove(old);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleCacheName, roles, -1);

            roleOrgs.Remove(sysRoleOrg);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleAndOrgCacheName, roleOrgs, -1);

            #endregion

            return Ok(actionResult);
        }

        #endregion

        #region 批量删除角色

        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="ids">编号数组</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{ids}")]
        [Permission("role:del:entities")]
        [Log(OperationType.DeleteEntity)]
        public async Task<IActionResult> DeleteRange(String ids)
        {
            //删除检测
            await this.DeleteCheck(ids);

            //删除角色
            var array = NGStringUtil.GetGuids(ids);
            var actionResult = await this.roleService.DeleteRangeAsync(array);
            //删除角色与机构关系  
            var roleOrgs = await this.redisService.GetAsync<List<SysRoleOrg>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndOrgCacheName);
            var delList = new List<SysRoleOrg>();
            foreach (var item in array)
            {
                var sysRoleOrg = roleOrgs.Where(t => t.RoleId.Equals(item) && t.IsDel == 0 && t.OwnerType == (int)RoleType.Own).FirstOrDefault();
                delList.Add(sysRoleOrg);
            }
            await this.roleOrgService.DeleteRangeAsync(delList.Select(t => t.Id).ToArray());

            #region 数据一致性维护

            var roles = await this.redisService.GetAsync<List<SysRoleDTO>>(NGAdminGlobalContext.SystemCacheConfig.RoleCacheName);
            foreach (var item in array)
            {
                var old = roles.Where(t => t.Id == item).FirstOrDefault();
                roles.Remove(old);
            }
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleCacheName, roles, -1);

            roleOrgs.RemoveRange(delList);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleAndOrgCacheName, roleOrgs, -1);

            #endregion

            return Ok(actionResult);
        }

        #endregion

        #region  角色授权用户

        /// <summary>
        /// 角色授权用户
        /// </summary>
        /// <param name="roleUsers">用户角色</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.PermissionAuthorization)]
        [Permission("role:grant:permission")]
        public async Task<IActionResult> GrantUserRole([FromBody] List<SysRoleUser> roleUsers)
        {
            var addList = new List<SysRoleUser>();
            var list = await this.redisService.GetAsync<List<SysRoleUser>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndUserCacheName);

            if (roleUsers.Count > 0)
            {
                foreach (var item in roleUsers)
                {
                    var entity = list.Where(t => t.IsDel == 0 && t.UserId.Equals(item.UserId) && t.RoleId.Equals(item.RoleId)).FirstOrDefault();
                    if (entity == null)
                    {
                        addList.Add(item);
                    }
                }
            }

            var actionResult = await this.roleUserService.AddListAsync(addList);

            //数据一致性维护
            list.AddRange(addList);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleAndUserCacheName, list, -1);

            return Ok(actionResult);
        }

        #endregion

        #region 收回用户授权

        /// <summary>
        /// 收回用户授权
        /// </summary>
        /// <param name="roleUsers">用户角色</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.PermissionAuthorization)]
        [Permission("role:grant:permission")]
        public async Task<IActionResult> WithdrawGrantedUserRole([FromBody] List<SysRoleUser> roleUsers)
        {
            var delList = new List<SysRoleUser>();
            var list = await this.redisService.GetAsync<List<SysRoleUser>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndUserCacheName);

            if (roleUsers.Count > 0)
            {
                foreach (var item in roleUsers)
                {
                    var entity = list.Where(t => t.IsDel == 0 && t.UserId.Equals(item.UserId) && t.RoleId.Equals(item.RoleId)).FirstOrDefault();
                    if (entity != null)
                    {
                        delList.Add(entity);
                    }
                }
            }

            var actionResult = await this.roleUserService.DeleteRangeAsync(delList.Select(t => t.Id).ToArray());

            //数据一致性维护
            list.RemoveRange(delList);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleAndUserCacheName, list, -1);

            return Ok(actionResult);
        }

        #endregion

        #region 查询角色授权用户

        /// <summary>
        /// 查询角色授权用户
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{roleId}")]
        [Log(OperationType.QueryList)]
        [Permission("role:grant:permission")]
        public async Task<IActionResult> GetGrantedUserRoles(Guid roleId)
        {
            var list = await this.redisService.GetAsync<List<SysRoleUser>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndUserCacheName);
            list = list.Where(t => t.RoleId.Equals(roleId) && t.IsDel == 0).ToList();

            var actionResult = new Entity.CoreEntity.ActionResult();
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = list;

            return Ok(actionResult);
        }

        #endregion

        #region 角色关联菜单

        /// <summary>
        /// 角色关联菜单
        /// </summary>
        /// <param name="roleMenus">角色菜单</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.MenuAuthorization)]
        [Permission("role:relate:menus")]
        public async Task<IActionResult> GrantRoleMenus([FromBody] List<SysRoleMenu> roleMenus)
        {
            var roleMenuList = await this.redisService.GetAsync<List<SysRoleMenu>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndMenuCacheName);

            if (roleMenus.Count > 0)
            {
                var list = roleMenuList.Where(t => t.RoleId.Equals(roleMenus.FirstOrDefault().RoleId) && t.IsDel == 0).ToList();
                var ids = list.Select(t => t.Id).ToArray();
                await this.roleMenuService.DeleteRangeAsync(ids);

                roleMenuList.RemoveRange(list);
            }

            var actionResult = await this.roleMenuService.AddListAsync(roleMenus);

            //数据一致性维护
            roleMenuList.AddRange(roleMenus);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleAndMenuCacheName, roleMenuList, -1);

            return Ok(actionResult);
        }

        #endregion

        #region 查询角色关联菜单

        /// <summary>
        /// 查询角色关联菜单
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{roleId}")]
        [Log(OperationType.QueryList)]
        [Permission("role:relate:menus")]
        public async Task<IActionResult> GetGrantedRoleMenus(Guid roleId)
        {
            var roleMenus = await this.redisService.GetAsync<List<SysRoleMenu>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndMenuCacheName);
            roleMenus = roleMenus.Where(t => t.RoleId.Equals(roleId) && t.IsDel == 0).ToList();

            var actionResult = new Entity.CoreEntity.ActionResult();
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = roleMenus;

            return Ok(actionResult);
        }

        #endregion

        #region 查询角色继承机构

        /// <summary>
        /// 查询角色继承机构
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{roleId}")]
        [Log(OperationType.QueryList)]
        [Permission("role:delegate:permission")]
        public async Task<IActionResult> GetDelegatedRoleOrgs(Guid roleId)
        {
            var roleOrgs = await this.redisService.GetAsync<List<SysRoleOrg>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndOrgCacheName);
            roleOrgs = roleOrgs.Where(t => t.RoleId.Equals(roleId) && t.IsDel == 0 && t.OwnerType.Equals((int)RoleType.Inherited)).ToList();

            var actionResult = new Entity.CoreEntity.ActionResult();
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = roleOrgs;

            return Ok(actionResult);
        }

        #endregion

        #region 机构继承角色

        /// <summary>
        /// 机构继承角色
        /// </summary>
        /// <param name="roleOrgs">角色机构列表</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.DelegatePermission)]
        [Permission("role:delegate:permission")]
        public async Task<IActionResult> DelegateRoleOrgs([FromBody] List<SysRoleOrg> roleOrgs)
        {
            //新增列表
            var addList = new List<SysRoleOrg>();
            //查询列表
            var list = await this.redisService.GetAsync<List<SysRoleOrg>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndOrgCacheName);

            foreach (var item in roleOrgs)
            {
                if (list.Where(t => t.IsDel == 0 && t.RoleId.Equals(item.RoleId) &&
                                    t.OrgId.Equals(item.OrgId) && t.OwnerType.Equals((int)RoleType.Own)).Count() > 0)
                {
                    //自有角色，无需继承
                    continue;
                }

                var count = list.Where(t => t.IsDel == 0 && t.RoleId.Equals(item.RoleId) &&
                                            t.OrgId.Equals(item.OrgId) && t.OwnerType.Equals(item.OwnerType)).Count();
                //未继承该角色
                if (count == 0)
                {
                    addList.Add(item);
                }
            }

            //新增继承角色
            var actionResult = await this.roleOrgService.AddListAsync(addList);

            //数据一致性维护
            list.AddRange(addList);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleAndOrgCacheName, list, -1);

            return Ok(actionResult);
        }

        #endregion

        #region 收回机构继承角色

        /// <summary>
        /// 收回机构继承角色
        /// </summary>
        /// <param name="roleOrgs">角色机构列表</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.DeleteEntity)]
        [Permission("role:delegate:permission")]
        public async Task<IActionResult> WithdrawRoleOrgs([FromBody] List<SysRoleOrg> roleOrgs)
        {
            //删除列表
            var delList = new List<SysRoleOrg>();
            var delRoleUserList = new List<SysRoleUser>();

            //查询列表
            var list = await this.redisService.GetAsync<List<SysRoleOrg>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndOrgCacheName);
            var roleUsers = await this.redisService.GetAsync<List<SysRoleUser>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndUserCacheName);
            var users = await this.redisService.GetAsync<List<SysUserDTO>>(NGAdminGlobalContext.SystemCacheConfig.UserCacheName);

            foreach (var item in roleOrgs)
            {
                var entity = list.Where(t => t.IsDel == 0 && t.RoleId.Equals(item.RoleId) &&
                                             t.OrgId.Equals(item.OrgId) && t.OwnerType.Equals(item.OwnerType)).FirstOrDefault();
                if (entity != null)
                {
                    delList.Add(entity);

                    var userList = users.Where(t => t.OrgId == item.OrgId).ToList();
                    foreach (var user in userList)
                    {
                        var roleUser = roleUsers.Where(t => t.UserId == user.Id && t.RoleId == item.RoleId).FirstOrDefault();
                        if (roleUser != null)
                        {
                            delRoleUserList.Add(roleUser);
                        }
                    }
                }
            }

            //删除继承角色
            var actionResult = await this.roleOrgService.DeleteRangeAsync(delList.Select(t => t.Id).ToArray());
            await this.roleUserService.DeleteRangeAsync(delRoleUserList.Select(t => t.Id).ToArray());

            //数据一致性维护
            list.RemoveRange(delList);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleAndOrgCacheName, list, -1);

            roleUsers.RemoveRange(delRoleUserList);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleAndUserCacheName, roleUsers, -1);

            return Ok(actionResult);
        }

        #endregion

        #region 查询是否为继承角色

        /// <summary>
        /// 查询是否为继承角色
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{roleId}")]
        [Log(OperationType.QueryEntity)]
        [Permission("role:query:list")]
        public async Task<IActionResult> IsInheritedRole(Guid roleId)
        {
            var actionResult = new Entity.CoreEntity.ActionResult();
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");

            var roleOrgs = await this.redisService.GetAsync<List<SysRoleOrg>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndOrgCacheName);

            if (roleOrgs.Where(t => t.RoleId.Equals(roleId) && t.IsDel == 0 &&
                                    t.OwnerType == (int)RoleType.Inherited
                                    && t.OrgId.Equals(NGAdminSessionContext.GetUserOrgId(this.context))).Count() > 0)
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

        #region 角色导出Excel

        /// <summary>
        /// 角色导出Excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Log(OperationType.ExportData)]
        [Permission("role:list:export")]
        public async Task<IActionResult> ExportExcel()
        {
            var roles = await this.redisService.GetAsync<List<SysRoleDTO>>(NGAdminGlobalContext.SystemCacheConfig.RoleCacheName);

            //获取用户机构编号
            var orgId = NGAdminSessionContext.GetUserOrgId(this.context);
            roles = roles.Where(r => r.OrgId == orgId).ToList();
            roles = roles.OrderBy(t => t.SerialNumber).ToList();

            var stream = roles.Export().ToStream();
            stream.Position = 0;

            return File(stream, "application/octet-stream", "角色信息.xls");
        }

        #endregion

        #region Excel导入角色

        /// <summary>
        /// Excel导入角色
        /// </summary>
        /// <param name="file">excel文件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.ImportData)]
        [Permission("role:list:import")]
        public async Task<IActionResult> Import([FromForm] IFormFile file)
        {
            var actionResult = new Entity.CoreEntity.ActionResult();

            if (file != null)
            {
                #region 常规合法性校验

                //获取文件拓展名
                var extension = Path.GetExtension(file.FileName);
                //文件保存路径
                var filePath = NGFileContext.SaveFormFile(file);

                var configDTO = this.importConfigService.GetImportConfig("RoleImportConfig");
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

                    var roles = await this.redisService.GetAsync<List<SysRoleDTO>>(NGAdminGlobalContext.SystemCacheConfig.RoleCacheName);

                    //获取用户机构编号
                    var orgId = NGAdminSessionContext.GetUserOrgId(this.context);
                    var currentRoles = roles.Where(r => r.OrgId == orgId).ToList();

                    var xlxStream = new FileStream(configDTO.ExcelPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                    var workbook = new HSSFWorkbook(xlxStream);
                    var worksheet = workbook.GetSheetAt(0);

                    for (var i = configDTO.StartRow; i <= worksheet.LastRowNum; i++)
                    {
                        var value = worksheet.GetRow(i).GetCell(1).GetCellValue().Trim();
                        if (!String.IsNullOrEmpty(value) && currentRoles.Where(t => t.RoleName.Equals(value)).Count() > 0)
                        {
                            errorCount++;
                            worksheet.SetCellComment(i, 1, "角色名称重复！");
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

                        var roleList = new List<SysRole>();
                        var roleOrgList = new List<SysRoleOrg>();

                        var list = worksheet.ToList<SysRole>(configDTO.StartRow, configDTO.StartColumn);
                        foreach (var role in list)
                        {
                            //开启事务
                            await this.roleService.UseTransactionAsync(async () =>
                            {
                                await this.roleService.AddAsync(role);
                                roleList.Add(role);

                                var roleOrg = new SysRoleOrg();
                                roleOrg.RoleId = role.Id;
                                roleOrg.OrgId = orgId;
                                roleOrg.OwnerType = (int)RoleType.Own;
                                await this.roleOrgService.AddAsync(roleOrg);
                                roleOrgList.Add(roleOrg);
                            });
                        }

                        #region 数据一致性维护

                        foreach (var role in roleList)
                        {
                            var roleDTO = mapper.Map<SysRoleDTO>(role);
                            roleDTO.OrgId = orgId;
                            roles.Add(roleDTO);
                        }
                        await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleCacheName, roles, -1);

                        var roleOrgs = await this.redisService.GetAsync<List<SysRoleOrg>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndOrgCacheName);
                        roleOrgs.AddRange(roleOrgList);
                        await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.RoleAndOrgCacheName, roleOrgs, -1);

                        #endregion

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

        #region 角色删除检测

        /// <summary>
        /// 角色删除检测
        /// </summary>
        /// <param name="ids">编号数组</param>
        /// <returns>真假值</returns>
        private async Task DeleteCheck(String ids)
        {
            var roleUsers = await this.redisService.GetAsync<List<SysRoleUser>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndUserCacheName);
            var roleMenus = await this.redisService.GetAsync<List<SysRoleMenu>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndMenuCacheName);
            var roleOrgs = await this.redisService.GetAsync<List<SysRoleOrg>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndOrgCacheName);

            var array = NGStringUtil.GetGuids(ids);
            foreach (var item in array)
            {
                if (roleUsers.Where(t => t.RoleId.Equals(item) && t.IsDel == 0).Count() > 0 ||
                    roleMenus.Where(t => t.RoleId.Equals(item) && t.IsDel == 0).Count() > 0 ||
                    roleOrgs.Where(t => t.RoleId.Equals(item) && t.IsDel == 0 && t.OwnerType == (int)RoleType.Inherited).Count() > 0)
                {
                    throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException("role has been used,can not be deleted");
                }
            }
        }

        #endregion
    }
}
