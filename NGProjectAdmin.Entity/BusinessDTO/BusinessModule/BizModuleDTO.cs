//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using System;

namespace NGProjectAdmin.Entity.BusinessDTO.BusinessModule
{
    /// <summary>
    /// BizModule DTO
    /// </summary>
    public class BizModuleDTO : BizModule
    {
        /// <summary>
        /// 用户所在模块登录账号
        /// </summary>
        public String UserModuleLogonName { get; set; }

        /// <summary>
        /// 用户所在模块登录密码
        /// </summary>
        public String UserModulePassword { get; set; }
    }
}
