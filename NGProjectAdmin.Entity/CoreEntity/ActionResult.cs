using Newtonsoft.Json;
using System;
using System.Net;

namespace NGProjectAdmin.Entity.CoreEntity
{
    /// <summary>
    /// 动作执行结果
    /// </summary>
    public class ActionResult
    {
        /// <summary>
        /// HttpStatusCode
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// 执行消息
        /// </summary>
        public String Message { get; set; }

        /// <summary>
        /// 执行对象
        /// </summary>
        public Object Object { get; set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ActionResult()
        {

        }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="httpStatusCode">Http状态码</param>
        /// <param name="message">消息内容</param>
        /// <param name="obj">返回对象</param>
        public ActionResult(HttpStatusCode httpStatusCode, String message, Object obj)
        {
            HttpStatusCode = httpStatusCode;
            Message = message;
            Object = obj;
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <returns>json字符串</returns>
        public String ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Success标志
        /// </summary>
        /// <param name="obj">返回对象</param>
        /// <returns>ActionResult</returns>
        public static ActionResult Success(Object obj)
        {
            return new ActionResult(HttpStatusCode.OK, new String("OK"), obj);
        }

        /// <summary>
        /// OK标志
        /// </summary>
        /// <returns>ActionResult</returns>
        public static ActionResult OK()
        {
            return new ActionResult(HttpStatusCode.OK, new String("OK"), new String("OK"));
        }

        /// <summary>
        /// BadRequest标志
        /// </summary>
        /// <returns>ActionResult</returns>
        public static ActionResult BadRequest()
        {
            return new ActionResult(HttpStatusCode.BadRequest, new String("BadRequest"), null);
        }

        /// <summary>
        /// Unauthorized标志
        /// </summary>
        /// <returns>ActionResult</returns>
        public static ActionResult Unauthorized()
        {
            return new ActionResult(HttpStatusCode.Unauthorized, new String("Unauthorized"), null);
        }

        /// <summary>
        /// Forbidden标志
        /// </summary>
        /// <returns>ActionResult</returns>
        public static ActionResult Forbidden()
        {
            return new ActionResult(HttpStatusCode.Forbidden, new String("Forbidden"), null);
        }

        /// <summary>
        /// NotFound标志
        /// </summary>
        /// <returns>ActionResult</returns>
        public static ActionResult NotFound()
        {
            return new ActionResult(HttpStatusCode.NotFound, new String("NotFound"), null);
        }

        /// <summary>
        /// NoContent标志
        /// </summary>
        /// <returns>ActionResult</returns>
        public static ActionResult NoContent()
        {
            return new ActionResult(HttpStatusCode.NoContent, new String("NoContent"), null);
        }

    }
}
