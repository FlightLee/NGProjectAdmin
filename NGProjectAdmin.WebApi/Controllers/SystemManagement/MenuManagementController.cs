//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Class.Extensions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Service.BusinessService.RedisService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.MenuLanguageService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.MenuService;
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
    /// 菜单管理控制器
    /// </summary>
    public class MenuManagementController : NGAdminBaseController<SysMenu>
    {
        #region 属性及构造函数

        /// <summary>
        /// 菜单服务实例
        /// </summary>
        private readonly IMenuService menuService;

        /// <summary>
        /// 菜单多语实例
        /// </summary>
        private readonly IMenuLanguageService menuLanguageService;

        /// <summary>
        /// Redis接口实例
        /// </summary>
        private readonly IRedisService redisService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="menuService"></param>
        /// <param name="menuLanguageService"></param>
        /// <param name="redisService"></param>
        public MenuManagementController(IMenuService menuService,
                                        IMenuLanguageService menuLanguageService,
                                        IRedisService redisService) : base(menuService)
        {
            this.menuService = menuService;
            this.menuLanguageService = menuLanguageService;
            this.redisService = redisService;
        }

        #endregion

        #region 查询菜单列表

        /// <summary>
        /// 查询菜单列表
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        [Permission("menu:query:list")]
        public async Task<IActionResult> Post()
        {
            var actionResult = await this.menuService.GetMenuTreeNodes();
            return Ok(actionResult);
        }

        #endregion

        #region 获取菜单信息

        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{id}")]
        [Log(OperationType.QueryEntity)]
        [Permission("menu:query:list")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var actionResult = new Entity.CoreEntity.ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");

            var menus = await this.redisService.GetAsync<List<SysMenuDTO>>(NGAdminGlobalContext.SystemCacheConfig.MenuCacheName);
            var menu = menus.Where(t => t.Id == id).FirstOrDefault();

            #region 初始化菜单多语

            var languages = this.redisService.Get<List<SysLanguage>>(NGAdminGlobalContext.SystemCacheConfig.LanguageCacheName);
            var lanEn = languages.Where(t => t.LanguageName.Equals("en-US")).FirstOrDefault();
            var lanRu = languages.Where(t => t.LanguageName.Equals("ru-RU")).FirstOrDefault();

            var menuLanguages = this.redisService.Get<List<SysMenuLanguage>>(NGAdminGlobalContext.SystemCacheConfig.MenuAndLanguageCacheName);

            #region 初始化多语

            var enMenu = menuLanguages.Where(t => t.MenuId.Equals(menu.Id) && t.LanguageId.Equals(lanEn.Id)).FirstOrDefault();
            var ruMenu = menuLanguages.Where(t => t.MenuId.Equals(menu.Id) && t.LanguageId.Equals(lanRu.Id)).FirstOrDefault();

            if (enMenu != null)
            {
                menu.MenuNameEn = enMenu.MenuName;
            }

            if (ruMenu != null)
            {
                menu.MenuNameRu = ruMenu.MenuName;
            }

            #endregion

            #endregion

            actionResult.Object = menu;

            return Ok(actionResult);
        }

        #endregion

        #region 新增菜单信息

        /// <summary>
        /// 新增菜单信息
        /// </summary>
        /// <param name="sysMenu">菜单对象</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.AddEntity)]
        [Permission("menu:add:entity")]
        public async Task<IActionResult> Add([FromBody] SysMenuDTO sysMenu)
        {
            var actionResult = await this.menuService.AddAsync(sysMenu);

            #region 维护菜单多语

            var languages = await this.redisService.GetAsync<List<SysLanguage>>(NGAdminGlobalContext.SystemCacheConfig.LanguageCacheName);
            var menuLanguages = await this.redisService.GetAsync<List<SysMenuLanguage>>(NGAdminGlobalContext.SystemCacheConfig.MenuAndLanguageCacheName);

            if (!String.IsNullOrEmpty(sysMenu.MenuNameEn))
            {
                var language = languages.Where(t => t.LanguageName.Equals("en-US")).FirstOrDefault();

                var menuLan = new SysMenuLanguage();
                menuLan.LanguageId = language.Id;
                menuLan.MenuId = sysMenu.Id;
                menuLan.MenuName = sysMenu.MenuNameEn;
                await this.menuLanguageService.AddAsync(menuLan);

                menuLanguages.Add(menuLan);
            }

            if (!String.IsNullOrEmpty(sysMenu.MenuNameRu))
            {
                var language = languages.Where(t => t.LanguageName.Equals("ru-RU")).FirstOrDefault();

                var menuLan = new SysMenuLanguage();
                menuLan.LanguageId = language.Id;
                menuLan.MenuId = sysMenu.Id;
                menuLan.MenuName = sysMenu.MenuNameRu;
                await this.menuLanguageService.AddAsync(menuLan);

                menuLanguages.Add(menuLan);
            }

            #endregion

            #region 数据一致性维护

            var menus = await this.redisService.GetAsync<List<SysMenuDTO>>(NGAdminGlobalContext.SystemCacheConfig.MenuCacheName);
            var menu = sysMenu;
            if (menu.Children != null)
            {
                menu.Children.Clear();
            }
            menus.Add(menu);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.MenuCacheName, menus, -1);

            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.MenuAndLanguageCacheName, menuLanguages, -1);

            #endregion

            return Ok(actionResult);
        }

        #endregion

        #region 修改菜单信息

        /// <summary>
        /// 修改菜单信息
        /// </summary>
        /// <param name="sysMenu">菜单对象</param>
        /// <returns>IActionResult</returns>
        [HttpPut]
        [Log(OperationType.EditEntity)]
        [Permission("menu:edit:entity")]
        public async Task<IActionResult> Put([FromBody] SysMenuDTO sysMenu)
        {
            var actionResult = await this.menuService.UpdateAsync(sysMenu);

            #region 维护菜单多语

            var languages = await this.redisService.GetAsync<List<SysLanguage>>(NGAdminGlobalContext.SystemCacheConfig.LanguageCacheName);
            var menuLanguages = await this.redisService.GetAsync<List<SysMenuLanguage>>(NGAdminGlobalContext.SystemCacheConfig.MenuAndLanguageCacheName);

            if (!String.IsNullOrEmpty(sysMenu.MenuNameEn))
            {
                var language = languages.Where(t => t.LanguageName.Equals("en-US")).FirstOrDefault();

                var oldMenuLan = menuLanguages.Where(t => t.LanguageId.Equals(language.Id) && t.MenuId.Equals(sysMenu.Id) && t.IsDel.Equals(0)).
                                               FirstOrDefault();

                if (oldMenuLan != null && oldMenuLan.MenuName != sysMenu.MenuNameEn)
                {
                    await this.menuLanguageService.DeleteAsync(oldMenuLan.Id);

                    menuLanguages.Remove(oldMenuLan);

                    var menuLan = new SysMenuLanguage();
                    menuLan.LanguageId = language.Id;
                    menuLan.MenuId = sysMenu.Id;
                    menuLan.MenuName = sysMenu.MenuNameEn;
                    await this.menuLanguageService.AddAsync(menuLan);

                    menuLanguages.Add(menuLan);
                }
            }
            if (!String.IsNullOrEmpty(sysMenu.MenuNameRu))
            {
                var language = languages.Where(t => t.LanguageName.Equals("ru-RU")).FirstOrDefault();

                var oldMenuLan = menuLanguages.Where(t => t.LanguageId.Equals(language.Id) && t.MenuId.Equals(sysMenu.Id) && t.IsDel.Equals(0)).
                                               FirstOrDefault();

                if (oldMenuLan != null && oldMenuLan.MenuName != sysMenu.MenuNameRu)
                {
                    await this.menuLanguageService.DeleteAsync(oldMenuLan.Id);

                    menuLanguages.Remove(oldMenuLan);

                    var menuLan = new SysMenuLanguage();
                    menuLan.LanguageId = language.Id;
                    menuLan.MenuId = sysMenu.Id;
                    menuLan.MenuName = sysMenu.MenuNameRu;
                    await this.menuLanguageService.AddAsync(menuLan);

                    menuLanguages.Add(menuLan);
                }
            }

            #endregion

            #region 数据一致性维护

            var menus = await this.redisService.GetAsync<List<SysMenuDTO>>(NGAdminGlobalContext.SystemCacheConfig.MenuCacheName);
            var menu = menus.Where(t => t.Id == sysMenu.Id).FirstOrDefault();
            menus.Remove(menu);
            menu = sysMenu;
            if (menu.Children != null)
            {
                menu.Children.Clear();
            }
            menus.Add(menu);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.MenuCacheName, menus, -1);

            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.MenuAndLanguageCacheName, menuLanguages, -1);

            #endregion

            return Ok(actionResult);
        }

        #endregion

        #region 删除菜单信息

        /// <summary>
        /// 删除菜单信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{id}")]
        [Log(OperationType.DeleteEntity)]
        [Permission("menu:del:entities")]
        public async Task<IActionResult> Delete(Guid id)
        {
            //数据删除检测
            await this.DeleteCheck(id.ToString());

            //数据删除
            var actionResult = await this.menuService.DeleteAsync(id);

            var menuLanguages = await this.redisService.GetAsync<List<SysMenuLanguage>>(NGAdminGlobalContext.SystemCacheConfig.MenuAndLanguageCacheName);
            var oldMenuLans = menuLanguages.Where(t => t.MenuId.Equals(id) && t.IsDel.Equals(0)).ToList();
            if (oldMenuLans.Count() > 0)
            {
                await this.menuLanguageService.DeleteRangeAsync(oldMenuLans.Select(t => t.Id).ToArray());

                menuLanguages.RemoveRange(oldMenuLans);
            }

            #region 数据一致性维护

            var menus = await this.redisService.GetAsync<List<SysMenuDTO>>(NGAdminGlobalContext.SystemCacheConfig.MenuCacheName);
            var menu = menus.Where(t => t.Id == id).FirstOrDefault();
            menus.Remove(menu);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.MenuCacheName, menus, -1);

            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.MenuAndLanguageCacheName, menuLanguages, -1);

            #endregion

            return Ok(actionResult);
        }

        #endregion

        #region 批量删除菜单信息

        /// <summary>
        /// 批量删除菜单信息
        /// </summary>
        /// <param name="ids">数组串</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{ids}")]
        [Log(OperationType.DeleteEntity)]
        [Permission("menu:del:entities")]
        public async Task<IActionResult> DeleteRange(String ids)
        {
            //数据删除检测
            await this.DeleteCheck(ids);

            //数据删除
            var array = NGStringUtil.GetGuids(ids);
            var actionResult = await this.menuService.DeleteRangeAsync(array);

            var menuLanguages = await this.redisService.GetAsync<List<SysMenuLanguage>>(NGAdminGlobalContext.SystemCacheConfig.MenuAndLanguageCacheName);
            foreach (var item in array)
            {
                var oldMenuLans = menuLanguages.Where(t => t.MenuId.Equals(item) && t.IsDel.Equals(0)).ToList();
                if (oldMenuLans.Count > 0)
                {
                    await this.menuLanguageService.DeleteRangeAsync(oldMenuLans.Select(t => t.Id).ToArray());

                    menuLanguages.RemoveRange(oldMenuLans);
                }
            }

            #region 数据一致性维护

            var menus = await this.redisService.GetAsync<List<SysMenuDTO>>(NGAdminGlobalContext.SystemCacheConfig.MenuCacheName);
            foreach (var item in array)
            {
                var menu = menus.Where(t => t.Id == item).FirstOrDefault();
                menus.Remove(menu);
            }
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.MenuCacheName, menus, -1);

            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.MenuAndLanguageCacheName, menuLanguages, -1);

            #endregion

            return Ok(actionResult);
        }

        #endregion

        #region 数据删除检测

        /// <summary>
        /// 数据删除检测
        /// </summary>
        /// <param name="ids">数组串</param>
        /// <returns>真假值</returns>
        private async Task DeleteCheck(String ids)
        {
            var menus = await this.redisService.GetAsync<List<SysMenuDTO>>(NGAdminGlobalContext.SystemCacheConfig.MenuCacheName);

            var array = NGStringUtil.GetGuids(ids);
            foreach (var item in array)
            {
                var subMenus = menus.Where(m => m.ParentId.Equals(item)).ToList();
                if (subMenus.Count > 0)
                {
                    throw new NGAdminCustomException("menu contains subitems,can not be deleted");
                }
            }
        }

        #endregion
    }
}
