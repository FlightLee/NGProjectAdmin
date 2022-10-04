using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace NGProjectAdmin.Entity.CoreEntity
{
    /// <summary>
    /// 查询结果
    /// </summary>
    public class QueryResult<T> where T : class
    {
        /// <summary>
        /// HttpStatusCode
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// 查询消息
        /// </summary>
        public String Message { get; set; }

        /// <summary>
        /// 查询总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 查询记录
        /// </summary>
        public List<T> List { get; set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public QueryResult()
        {

        }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="httpStatusCode">Http状态码</param>
        /// <param name="message">消息内容</param>
        /// <param name="totalCount">总条数</param>
        /// <param name="list">记录列表</param>
        public QueryResult(HttpStatusCode httpStatusCode, String message, int totalCount, List<T> list)
        {
            HttpStatusCode = httpStatusCode;
            Message = message;
            TotalCount = totalCount;
            List = list;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns>json字符串</returns>
        public String ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Success标志
        /// </summary>
        /// <param name="totalCount">查询总记录数</param>
        /// <param name="list">查询记录</param>
        /// <returns>QueryResult</returns>
        public static QueryResult<T> Success(int totalCount, List<T> list)
        {
            return new QueryResult<T>(HttpStatusCode.OK, new String("OK"), totalCount, list);
        }

        /// <summary>
        /// BadRequest标志
        /// </summary>
        /// <returns>QueryResult</returns>
        public static QueryResult<T> BadRequest()
        {
            return new QueryResult<T>(HttpStatusCode.BadRequest, new String("BadRequest"), 0, null);
        }

        /// <summary>
        /// Unauthorized标志
        /// </summary>
        /// <returns>QueryResult</returns>
        public static QueryResult<T> Unauthorized()
        {
            return new QueryResult<T>(HttpStatusCode.Unauthorized, new String("Unauthorized"), 0, null);
        }

        /// <summary>
        /// Forbidden标志
        /// </summary>
        /// <returns>QueryResult</returns>
        public static QueryResult<T> Forbidden()
        {
            return new QueryResult<T>(HttpStatusCode.Forbidden, new String("Forbidden"), 0, null);
        }

        /// <summary>
        /// NotFound标志
        /// </summary>
        /// <returns>QueryResult</returns>
        public static QueryResult<T> NotFound()
        {
            return new QueryResult<T>(HttpStatusCode.NotFound, new String("NotFound"), 0, null);
        }

        /// <summary>
        /// NoContent标志
        /// </summary>
        /// <returns>QueryResult</returns>
        public static QueryResult<T> NoContent()
        {
            return new QueryResult<T>(HttpStatusCode.NoContent, new String("NoContent"), 0, null);
        }
    }
}
