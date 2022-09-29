//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using System;

namespace NGProjectAdmin.Common.Class.Excel
{
    /// <summary>
    /// 数据列表导出标签
    /// </summary>
    public class ExcelExportAttribute : Attribute
    {
        /// <summary>
        /// 列名
        /// </summary>
        public String FieldName { get; }

        /// <summary>
        /// 类型枚举
        /// </summary>
        public String TextEnum { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">列名</param>
        /// <param name="textEnum">类型枚举,键值以英文冒号隔开，组之间用英文逗号隔开，如0:男,1:女,2:第三性别</param>
        public ExcelExportAttribute(String fieldName, String textEnum = null)
        {
            this.FieldName = fieldName;
            if (!String.IsNullOrEmpty(textEnum))
            {
                this.TextEnum = textEnum;
            }
        }
    }
}
