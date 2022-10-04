//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.System;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Service.BusinessService.MQService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.LogService;
using NGProjectAdmin.WebApi.AppCode.FrameworkClass;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.WebApi.AppCode.FrameworkExtensions
{
    /// <summary>
    /// 全局异常处理中间件
    /// </summary>
    public class GlobalExceptionHandlerExtensions
    {
        #region 属性及其构造函数

        private readonly RequestDelegate _next;  //上下文请求 
        private readonly ILogger<GlobalExceptionHandlerExtensions> _logger;
        private readonly IMQService mqService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next">上下文请求</param>
        /// <param name="logger">日志对象</param>
        /// <param name="mqService">消息队列实例</param>
        public GlobalExceptionHandlerExtensions(RequestDelegate next,
            ILogger<GlobalExceptionHandlerExtensions> logger,
            IMQService mqService)
        {
            this._next = next;
            this._logger = logger;
            this.mqService = mqService;
        }

        #endregion

        #region 处理上下文请求

        /// <summary>
        /// 处理上下文请求
        /// </summary>
        /// <param name="httpContext">http会话对象</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext); //处理上下文请求
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex); //捕获异常,在HandleExceptionAsync中处理
            }
        }

        #endregion

        #region 全局统一异常处理

        /// <summary>
        /// 全局统一异常处理
        /// </summary>
        /// <param name="context">http会话对象</param>
        /// <param name="exception">全局异常处理</param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";  //返回json 类型
            var response = context.Response;

            var errorResponse = new ErrorResponse { Success = false };

            // 自定义的异常错误信息类型
            switch (exception)
            {
                case ApplicationException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                case NGAdminCustomException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;

                    #region 记录SQL注入式攻击行为

                    if (ex.Message.Contains("Sql Injection Attack"))
                    {
                        #region 记录SQL注入式攻击审计日志

                        var token = context.GetToken();
                        //获取用户
                        var user = NGRedisContext.Get<SysUserDTO>(token);

                        var log = LogServiceExtension.GetSysLog(context);

                        //字段赋值
                        log.OperationType = OperationType.SqlInjectionAttack;
                        log.Remark = $"{String.Join("/", user.LogonName, user.DisplayName)}对系统进行了Sql注入式攻击，使用口令为{token}，请管理员警惕、关注！";

                        await log.WriteAsync();

                        #endregion

                        #region 发送Sql注入式攻击告警消息

                        var msg = new SystemMessage();

                        msg.Message = "Broadcast";
                        msg.MessageType = MessageType.Broadcast;

                        BroadcastMessage broadcastMessage = new BroadcastMessage();
                        broadcastMessage.Title = "Sql注入式攻击告警";
                        broadcastMessage.Message = log.Remark;
                        broadcastMessage.MessageLevel = MessageLevel.Warning;

                        msg.Object = broadcastMessage;

                        this.mqService.SendTopic(JsonConvert.SerializeObject(msg));

                        #endregion
                    }

                    #endregion

                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "Internal Server Error";
                    break;
            }

            //错误日志控制台输出
            _logger.LogError(exception.Message);

            //错误日志文件输出
            var errorInfo = new StringBuilder();
            //errorInfo.AppendLine("Time:" + DateTime.Now);
            errorInfo.AppendLine("StackTrace:" + (exception.StackTrace == null ? "" : exception.StackTrace));
            errorInfo.AppendLine("Source:" + (exception.Source == null ? "" : exception.Source));
            errorInfo.AppendLine("Message:" + exception.Message);
            errorInfo.AppendLine("InnerExceptionMessage:" + (exception.InnerException == null ? "" : exception.InnerException.Message));
            NGLoggerContext.Error(errorInfo.ToString());

            //返回统一异常
            var result = errorResponse;
            if (!context.Response.HasStarted)
            {
                await context.Response.WriteAsync(result.ToJson());
            }
        }

        #endregion
    }
}
