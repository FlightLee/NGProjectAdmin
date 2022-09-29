//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

namespace NGProjectAdmin.Entity.CoreEnum
{
    /// <summary>
    /// 查询方法
    /// </summary>
    public enum QueryMethod
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal,

        /// <summary>
        /// 模糊查询
        /// </summary>
        Like,

        /// <summary>
        /// 小于
        /// </summary>
        LessThan,

        /// <summary>
        /// 小于等于
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan,

        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// BetweenAnd
        /// </summary>
        BetweenAnd,

        /// <summary>
        /// Include
        /// </summary>
        Include,

        /// <summary>
        /// 模糊查询
        /// </summary>
        OrLike,

        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual
    }
}
