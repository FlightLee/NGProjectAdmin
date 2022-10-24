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
    /// 菜单DTO
    /// </summary>
    public class SysMenuDTO : SysMenu
    {
        /// <summary>
        /// 子集
        /// </summary>
        public List<SysMenuDTO> Children { get; set; }=new List<SysMenuDTO>();  

        /// <summary>
        /// 英文菜单名称
        /// </summary>
        public String? MenuNameEn { get; set; }

        /// <summary>
        /// 俄文菜单名称
        /// </summary>
        public String? MenuNameRu { get; set; }
    }
}
