//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using SqlSugar;
using System;
using System.Collections.Generic;

namespace NGProjectAdmin.Entity.BusinessDTO.SystemManagement
{
    /// <summary>
    /// 机构、用户树
    /// </summary>
    public class OrgUserTreeDTO
    {
        /// <summary>
        /// 编号
        /// </summary>
        [SugarColumn(ColumnName = "ID")]
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 类型
        /// 1，机构
        /// 2，用户
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(ColumnName = "SERIAL_NUMBER")]
        public Nullable<int> SerialNumber { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public List<OrgUserTreeDTO> Children { get; set; }
    }
}
