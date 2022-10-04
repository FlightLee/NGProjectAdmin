//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.BusinessRepository.RedisRepository;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.MenuRepository;
using NGProjectAdmin.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.MenuService
{
    /// <summary>
    /// 菜单业务层实现
    /// </summary>
    class MenuService : NGAdminBaseService<SysMenu>, IMenuService
    {
        #region 属性及构造函数

        /// <summary>
        /// 菜单仓储实例
        /// </summary>
        private readonly IMenuRepository menuRepository;

        /// <summary>
        /// Redis仓储实例
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="menuRepository"></param>
        /// <param name="redisRepository"></param>
        public MenuService(IMenuRepository menuRepository,
                           IRedisRepository redisRepository) : base(menuRepository)
        {
            this.menuRepository = menuRepository;
            this.redisRepository = redisRepository;
        }

        #endregion

        #region 服务层公有方法

        #region 获取菜单树

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns>ActionResult</returns>
        public async Task<QueryResult<SysMenuDTO>> GetMenuTreeNodes()
        {
            var result = new List<SysMenuDTO>();

            var menus = await this.redisRepository.GetAsync<List<SysMenuDTO>>(NGAdminGlobalContext.SystemCacheConfig.MenuCacheName);
            var parentMenus = menus.Where(t => t.ParentId == null).OrderBy(t => t.SerialNumber).ToList();
            foreach (var item in parentMenus)
            {
                this.GetNodeChildren(item, menus);
            }
            result.AddRange(parentMenus);

            var queryResult = new QueryResult<SysMenuDTO>();
            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            queryResult.List = result;

            return queryResult;
        }

        #endregion

        #region 加载系统菜单缓存

        /// <summary>
        /// 加载系统菜单缓存
        /// </summary>
        public async Task LoadSystemMenuCache()
        {
            var sqlKey = "sqls:sql:query_menu_info";
            var strSQL = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value;

            int totalCount = 0;
            var rootList = (await this.menuRepository.SqlQueryAsync<SysMenuDTO>(new QueryCondition(), totalCount, strSQL)).ToList();
            await this.redisRepository.SetAsync(NGAdminGlobalContext.SystemCacheConfig.MenuCacheName, rootList, -1);

            NGLoggerContext.Info("系统菜单缓存加载完成");
        }

        #endregion

        #region 清理系统菜单缓存

        /// <summary>
        /// 清理系统菜单缓存
        /// </summary>
        public async Task ClearSystemMenuCache()
        {
            await this.redisRepository.DeleteAsync(new String[] { NGAdminGlobalContext.SystemCacheConfig.MenuCacheName });

            NGLoggerContext.Info("系统菜单缓存清理完成");
        }

        #endregion

        #endregion

        #region 服务层私有方法

        /// <summary>
        /// 递归树
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="menus">菜单列表</param>
        private void GetNodeChildren(SysMenuDTO root, List<SysMenuDTO> menus)
        {
            var languages = this.redisRepository.Get<List<SysLanguage>>(NGAdminGlobalContext.SystemCacheConfig.LanguageCacheName);

            var lanEn = languages.Where(t => t.LanguageName.Equals("en-US")).FirstOrDefault();
            var lanRu = languages.Where(t => t.LanguageName.Equals("ru-RU")).FirstOrDefault();

            var menuLanguages = this.redisRepository.Get<List<SysMenuLanguage>>(NGAdminGlobalContext.SystemCacheConfig.MenuAndLanguageCacheName);

            #region 初始化多语

            var enMenu = menuLanguages.Where(t => t.MenuId.Equals(root.Id) && t.LanguageId.Equals(lanEn.Id)).FirstOrDefault();
            var ruMenu = menuLanguages.Where(t => t.MenuId.Equals(root.Id) && t.LanguageId.Equals(lanRu.Id)).FirstOrDefault();

            if (enMenu != null)
            {
                root.MenuNameEn = enMenu.MenuName;
            }

            if (ruMenu != null)
            {
                root.MenuNameRu = ruMenu.MenuName;
            }

            #endregion

            var list = menus.Where(t => t.ParentId == root.Id).ToList();
            if (list.Count > 0)
            {
                root.Children = new List<SysMenuDTO>();
                root.Children.AddRange(list.OrderBy(t => t.SerialNumber).ToList());

                foreach (var item in list)
                {
                    this.GetNodeChildren(item, menus);
                }
            }
        }

        #endregion
    }
}
