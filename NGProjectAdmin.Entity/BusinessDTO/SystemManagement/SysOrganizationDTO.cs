//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using System;
using System.Collections.Generic;

namespace NGProjectAdmin.Entity.BusinessDTO.SystemManagement
{
    /// <summary>
    /// 机构DTO
    /// </summary>
    public class SysOrganizationDTO : SysOrganization
    {
        /// <summary>
        /// 主管人姓名
        /// </summary>
        public String LeaderName { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public List<SysOrganizationDTO> Children { get; set; }
    }
}
