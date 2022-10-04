//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Service.Base;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.RoleUserService
{
    /// <summary>
    /// 角色用户业务层接口
    /// </summary>
    public interface IRoleUserService : INGAdminBaseService<SysRoleUser>
    {
        /// <summary>
        /// 加载角色与用户缓存
        /// </summary>
        Task LoadSystemRoleUserCache();

        /// <summary>
        /// 清理角色与用户缓存
        /// </summary>
        Task ClearSystemRoleUserCache();
    }
}
