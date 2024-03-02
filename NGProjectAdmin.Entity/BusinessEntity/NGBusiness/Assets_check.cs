//-----------------------------------------------------------------------
// <Copyright file="BizModule.cs" company="RuYiAdmin">
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// * Version : 4.0.30319.42000
// * Author  : auto generated by RuYiAdmin T4 Template
// * FileName: BizModule.cs
// * History : Created by RuYiAdmin 01/19/2022 09:01:54
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Entity.Base;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace NGProjectAdmin.Entity.BusinessEntity.NGBusiness
{
    /// <summary>
    /// BizModule Entity Model
    /// </summary>   
    //[Serializable]
    [SugarTable("assets_check")]
    public class assets_check : NGAdminBaseEntity
    {
        /// <summary>
        /// 巡检人姓名
        /// </summary>
        [Required]
        [MaxLength(512)]
        [SugarColumn(ColumnName = "USER_NAME")]
        public String checkName { get; set; }

        /// <summary>
        /// 巡检人号码
        /// </summary>
        [Required]
        [MaxLength(256)]
        [SugarColumn(ColumnName = "USER_Phone")]
        public String checkPhone { get; set; }

        /// <summary>
        /// 巡检类型
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "OPERATION_TYPE")]
        public int checkType { get; set; }

        /// <summary>
        /// 资产Id
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "assetsId")]
        public string assetsId { get; set; }
        
        /// <summary>
        /// 巡检时间
        /// </summary>
        [SugarColumn(ColumnName = "checkTime")]
        public Nullable<DateTime> checkTime { get; set; }

        /// <summary>
        /// 巡检图片
        /// </summary>
        //[Required]
        //[MaxLength(512)]
        [SugarColumn(ColumnName = "fileGroupsId")]
        public String fileGroupsId { get; set; }

        /// <summary>
        /// 巡检问题
        /// </summary>
        //[Required]
        [MaxLength(512)]
        [SugarColumn(ColumnName = "checkProblem")]
        public String checkProblem { get; set; }

        /// <summary>
        /// 问题整改
        /// </summary>
        //[Required]
        [MaxLength(512)]
        [SugarColumn(ColumnName = "problemFix")]
        public String problemFix { get; set; }


    }
}