﻿//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using System;

namespace NGProjectAdmin.Entity.BusinessDTO.SystemManagement
{
    /// <summary>
    /// PasswordDTO
    /// </summary>
    public class PasswordDTO
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// 盐
        /// </summary>
        public String Salt { get; set; }
    }
}
