
using System;
using System.Runtime.Serialization;

namespace NGProjectAdmin.Common.Class.Exceptions
{
    /// <summary>
    /// 自定义异常
    /// </summary>
    public  class NGAdminCustomException : Exception
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public NGAdminCustomException() : base()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info">System.Runtime.Serialization.SerializationInfo</param>
        /// <param name="context">System.Runtime.Serialization.StreamingContext</param>
        protected NGAdminCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常信息</param>
        public NGAdminCustomException(String message = null) : base(message)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <param name="innerException">内部异常</param>
        public NGAdminCustomException(String message = null, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}
