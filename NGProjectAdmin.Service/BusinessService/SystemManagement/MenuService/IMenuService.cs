//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.Base;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.MenuService
{
    /// <summary>
    /// 菜单业务层接口
    /// </summary>
    public interface IMenuService : INGAdminBaseService<SysMenu>
    {
        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns>ActionResult</returns>
        Task<QueryResult<SysMenuDTO>> GetMenuTreeNodes();

        /// <summary>
        /// 加载系统菜单缓存
        /// </summary>
        Task LoadSystemMenuCache();

        /// <summary>
        /// 清理系统菜单缓存
        /// </summary>
        Task ClearSystemMenuCache();
    }
}
