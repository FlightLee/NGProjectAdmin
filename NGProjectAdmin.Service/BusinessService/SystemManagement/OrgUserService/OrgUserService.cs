//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.OrgUserRepository;
using NGProjectAdmin.Service.Base;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.OrgUserService
{
    /// <summary>
    /// 机构用户业务层实现
    /// </summary>
    public class OrgUserService : NGAdminBaseService<SysOrgUser>, IOrgUserService
    {
        /// <summary>
        /// 机构与用户仓储实例
        /// </summary>
        private readonly IOrgUserRepository orgUserRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orgUserRepository"></param>
        public OrgUserService(IOrgUserRepository orgUserRepository) : base(orgUserRepository)
        {
            this.orgUserRepository = orgUserRepository;
        }
    }
}
