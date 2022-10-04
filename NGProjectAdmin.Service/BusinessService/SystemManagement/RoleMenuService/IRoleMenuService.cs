//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Service.Base;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.RoleMenuService
{
    /// <summary>
    /// 角色菜单业务层接口
    /// </summary>
    public interface IRoleMenuService : INGAdminBaseService<SysRoleMenu>
    {
        /// <summary>
        /// 加载角色与菜单缓存
        /// </summary>
        Task LoadSystemRoleMenuCache();

        /// <summary>
        /// 清理角色与菜单缓存
        /// </summary>
        Task ClearSystemRoleMenuCache();
    }
}
