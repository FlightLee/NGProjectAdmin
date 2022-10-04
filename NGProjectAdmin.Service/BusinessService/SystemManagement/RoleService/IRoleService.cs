//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Service.Base;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.RoleService
{
    /// <summary>
    /// 角色业务层接口
    /// </summary>
    public interface IRoleService : INGAdminBaseService<SysRole>
    {
        /// <summary>
        /// 加载系统角色缓存
        /// </summary>
        Task LoadSystemRoleCache();

        /// <summary>
        /// 清理系统角色缓存
        /// </summary>
        Task ClearSystemRoleCache();
    }
}
