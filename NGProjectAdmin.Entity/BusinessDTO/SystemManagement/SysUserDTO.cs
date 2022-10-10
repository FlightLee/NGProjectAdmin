//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Common.Class.Excel;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.User;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace NGProjectAdmin.Entity.BusinessDTO.SystemManagement
{
    /// <summary>
    /// 用户DTO
    /// </summary>
   // public class SysUserDTO : UserBaseInfo
   public class SysUserDTO : UserBaseInfo
    {
        /// <summary>
        /// 机构编号
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "ORG_ID")]
        public Guid OrgId { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        [SugarColumn(ColumnName = "ORG_NAME")]
        [ExcelExport("所在机构")]
        public String OrgName { get; set; }=String.Empty;       

        /// <summary>
        /// token
        /// </summary>
        public String Token { get; set; }=String.Empty;

        /// <summary>
        /// token有效时间
        /// 单位：秒
        /// </summary>
        public int TokenExpiration { get; set; }        
    }
}
