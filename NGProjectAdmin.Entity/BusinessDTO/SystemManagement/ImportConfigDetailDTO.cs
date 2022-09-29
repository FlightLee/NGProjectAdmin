//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using NGProjectAdmin.Entity.CoreEnum;
using System;

namespace NGProjectAdmin.Entity.BusinessDTO.SystemManagement
{
    /// <summary>
    /// 导入配置明细DTO
    /// </summary>
    public class ImportConfigDetailDTO
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        public CellDataType DataType { get; set; }

        /// <summary>
        /// 所在列
        /// </summary>
        public String Cells { get; set; }

        /// <summary>
        /// 是否必填项
        /// 0：否
        /// 1：是
        /// </summary>
        public Nullable<int> Rquired { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public Nullable<Double> MaxValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public Nullable<Double> MinValue { get; set; }

        /// <summary>
        /// 小数位上限
        /// </summary>
        public Nullable<int> DecimalLimit { get; set; }

        /// <summary>
        /// 枚举列表
        /// </summary>
        public String TextEnum { get; set; }

        /// <summary>
        /// 扩展字段
        /// 正则占用
        /// </summary>
        public String Extend1 { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        public String Extend2 { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        public String Extend3 { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public Nullable<int> SerialNumber { get; set; }
    }
}
