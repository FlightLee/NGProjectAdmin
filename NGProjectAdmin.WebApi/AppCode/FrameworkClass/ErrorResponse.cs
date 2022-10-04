//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using System;
using System.Text.Json;

namespace NGProjectAdmin.WebApi.AppCode.FrameworkClass
{
    /// <summary>
    /// 错误应答信息
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// 标志位
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// 应答消息
        /// </summary>
        public String Message { get; set; } = "An unexpected error occurred";

        /// <summary>
        /// ToJson方法
        /// </summary>
        /// <returns></returns>
        public String ToJson() => JsonSerializer.Serialize(this);
    }
}
