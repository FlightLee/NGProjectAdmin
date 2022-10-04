using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NGProjectAdmin.WebApi.AppCode.ActionFilters
{
    /// <summary>
    /// 动作权限过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionAttribute : ActionFilterAttribute
    {
        private String permission { get; set; }

        public PermissionAttribute(String permission)
        {
            this.permission = permission;
        }

        /// <summary>
        /// 动作鉴权
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="Exception"></exception>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (String.IsNullOrEmpty(permission))
            {
                context.Result = new BadRequestObjectResult("permission can not be null");
                return;
            }

            var token = context.HttpContext.GetToken();
            //获取用户
            var user = NGRedisContext.Get<SysUserDTO>(token);
            if (user == null)
            {
                context.Result = new UnauthorizedObjectResult("token is invalid");
                return;
            }

            //放行超级用户
            if (user.IsSupperAdmin.Equals((int)YesNo.YES) && user.OrgId.Equals(Guid.Empty))
            {
                return;
            }

            var permissionArray = permission.Split(',');

            #region 获取用户角色

            var listRole = NGRedisContext.Get<List<SysRoleUser>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndUserCacheName);

            var roleIds = listRole.Where(t => t.IsDel.Equals(0) && t.UserId.Equals(user.Id)).Select(t => t.RoleId).ToArray();

            #endregion

            #region  获取角色菜单

            var roleMenuList = NGRedisContext.Get<List<SysRoleMenu>>(NGAdminGlobalContext.SystemCacheConfig.RoleAndMenuCacheName);

            var listRoleMenu = new List<SysRoleMenu>();

            foreach (var item in roleIds)
            {
                var roleMenus = roleMenuList.Where(t => t.IsDel.Equals(0) && t.RoleId.Equals(item)).ToList();
                listRoleMenu.AddRange(roleMenus);
            }

            var menuIds = listRoleMenu.Select(t => t.MenuId).Distinct().ToArray();

            #endregion

            #region 获取系统菜单

            var menus = NGRedisContext.Get<List<SysMenuDTO>>(NGAdminGlobalContext.SystemCacheConfig.MenuCacheName);

            var listMenus = menus.Where(t => t.IsDel.Equals(0)
                                          && t.MenuType.Equals(MenuType.Button)
                                          || t.MenuType.Equals(MenuType.View)).ToList();

            #endregion

            #region 操作权限判断

            var result = false;

            foreach (var item in menuIds)
            {
                var menu = listMenus.Where(t => t.Id.Equals(item)).FirstOrDefault();

                if (menu != null && !String.IsNullOrEmpty(menu.Code))
                {
                    //是否包含权限组
                    if (permissionArray.Contains(menu.Code))
                    {
                        result = true;
                        break;
                    }
                }
            }

            #endregion

            if (!result)
            {
                context.Result = new BadRequestObjectResult("access denied");
                return;
            }
        }
    }
}
