//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace NGProjectAdmin.Entity.BusinessEntity.BusinessModule
{
    /// <summary>
    /// 模块API访问账号表
    /// </summary>
    [SugarTable("biz_account")]
    public class BizAccount : BizUser
    {
        /// <summary>
        /// 模块编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "MODULE_ID")]
        public Guid ModuleId { get; set; }
    }
}
