//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using System.Collections.Generic;

namespace NGProjectAdmin.Entity.BusinessDTO.SystemManagement
{
    /// <summary>
    /// 数据字典DTO
    /// </summary>
    public class SysCodeTableDTO : SysCodeTable
    {
        /// <summary>
        /// 子集
        /// </summary>
        public List<SysCodeTableDTO> Children { get; set; }
    }
}
