//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;
using NGProjectAdmin.Service.BusinessService.SystemManagement.AreaService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.CodeTableService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.LanguageService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.MenuLanguageService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.MenuService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.OrganizationService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.RoleMenuService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.RoleOrgService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.RoleService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.RoleUserService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.ScheduleJobService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.UserService;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NGProjectAdmin.WebApi.AppCode.FrameworkClass
{
    public class NGAdminApplication
    {
        #region 启动业务作业

        /// <summary>
        /// 启动业务作业
        /// </summary>
        /// <returns></returns>
        public static async Task StartScheduleJobAsync(IApplicationBuilder app)
        {
            await app.ApplicationServices.GetService<IScheduleJobService>().StartScheduleJobAsync();
        }

        #endregion

        #region 加载系统缓存

        /// <summary>
        /// 加载系统缓存
        /// </summary>
        /// <returns></returns>
        public static async Task LoadSystemCache(IApplicationBuilder app)
        {
            //加载机构缓存
            await app.ApplicationServices.GetService<IOrganizationService>().LoadSystemOrgCache();

            //加载用户缓存
            await app.ApplicationServices.GetService<IUserService>().LoadSystemUserCache();

            //加载菜单缓存
            await app.ApplicationServices.GetService<IMenuService>().LoadSystemMenuCache();

            //加载菜单与多语缓存
            await app.ApplicationServices.GetService<IMenuLanguageService>().LoadSystemMenuLanguageCache();

            //加载角色缓存
            await app.ApplicationServices.GetService<IRoleService>().LoadSystemRoleCache();

            //加载角色与菜单缓存
            await app.ApplicationServices.GetService<IRoleMenuService>().LoadSystemRoleMenuCache();

            //加载角色与机构缓存
            await app.ApplicationServices.GetService<IRoleOrgService>().LoadSystemRoleOrgCache();

            //加载角色与用户缓存
            await app.ApplicationServices.GetService<IRoleUserService>().LoadSystemRoleUserCache();

            //加载数据字典缓存
            await app.ApplicationServices.GetService<ICodeTableService>().LoadSystemCodeTableCache();

            //加载多语缓存
            await app.ApplicationServices.GetService<ILanguageService>().LoadSystemLanguageCache();

            //加载计划任务缓存
            await app.ApplicationServices.GetService<IScheduleJobService>().LoadBusinessScheduleJobCache();

            //加载行政区域缓存
            await app.ApplicationServices.GetService<IAreaService>().LoadSysAreaCache();
        }

        #endregion

        #region 清理系统缓存

        /// <summary>
        /// 清理系统缓存
        /// </summary>
        /// <returns></returns>
        public static async Task ClearSystemCache(IApplicationBuilder app)
        {
            //清理机构缓存
            await app.ApplicationServices.GetService<IOrganizationService>().ClearSystemOrgCache();

            //清理用户缓存
            await app.ApplicationServices.GetService<IUserService>().ClearSystemUserCache();

            //清理菜单缓存
            await app.ApplicationServices.GetService<IMenuService>().ClearSystemMenuCache();

            //清理菜单与多语缓存
            await app.ApplicationServices.GetService<IMenuLanguageService>().ClearSystemMenuLanguageCache();

            //清理角色缓存
            await app.ApplicationServices.GetService<IRoleService>().ClearSystemRoleCache();

            //清理角色与菜单缓存
            await app.ApplicationServices.GetService<IRoleMenuService>().ClearSystemRoleMenuCache();

            //清理角色与机构缓存
            await app.ApplicationServices.GetService<IRoleOrgService>().ClearSystemRoleOrgCache();

            //清理角色与用户缓存
            await app.ApplicationServices.GetService<IRoleUserService>().ClearSystemRoleUserCache();

            //清理数据字典缓存
            await app.ApplicationServices.GetService<ICodeTableService>().ClearSystemCodeTableCache();

            //清理多语缓存
            await app.ApplicationServices.GetService<ILanguageService>().ClearSystemLanguageCache();

            //清理计划任务缓存
            await app.ApplicationServices.GetService<IScheduleJobService>().ClearBusinessScheduleJobCache();

            //清理行政区域缓存
            await app.ApplicationServices.GetService<IAreaService>().ClearSysAreaCache();
        }

        #endregion

        #region 自动构建数据库

        /// <summary>
        /// 自动构建数据库
        /// </summary>
        /// <returns></returns>
        public static async Task BuildDatabase()
        {
            await Task.Run(async () =>
            {
                if (NGAdminGlobalContext.DBConfig.AutomaticallyBuildDatabase)
                {
                    try
                    {
                        NGAdminDbScope.NGDbContext.DbMaintenance.GetTableInfoList();
                        Console.WriteLine("Database is existed");
                    }
                    catch
                    {
                        Console.WriteLine("Database is not existed");

                        //Sql脚本路径
                        var sqlScriptPath = Path.Join(Directory.GetCurrentDirectory(), "/", NGAdminGlobalContext.DBConfig.SqlScriptPath);

                        //读取脚本内容
                        var content = File.ReadAllText(sqlScriptPath);

                        //创建数据库
                        NGAdminDbScope.NGDbContext.DbMaintenance.CreateDatabase();
                        Console.WriteLine("Database created");

                        //构建表结构
                        await NGAdminDbScope.NGDbContext.Ado.ExecuteCommandAsync(content);
                        Console.WriteLine("Tables created");
                    }

                }
            });
        }

        #endregion
    }
}
