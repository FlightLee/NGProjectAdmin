using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Service.BusinessService.SystemManagement.LogService;
using System;
using System.IO;

namespace NGProjectAdmin.WebApi.AppCode.ActionFilters
{
    /// <summary>
    /// 审计日志过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class LogAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        private OperationType OperationType { get; set; }

        /// <summary>
        /// 行为描述信息
        /// </summary>
        private String Description { get; set; }

        public LogAttribute(OperationType operationType, String description = null)
        {
            this.OperationType = operationType;
            this.Description = description;
        }

        public override async void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);

            //判断审计日志记录开关是否开启
            if (!NGAdminGlobalContext.LogConfig.IsEnabled)
            {
                return;
            }

            var token = context.HttpContext.GetToken();
            //获取用户
            var user = NGRedisContext.Get<SysUserDTO>(token);
            if (user == null)
            {
                context.Exception = new Exception("token is invalid");
                return;
            }

            #region 监控信息入库

            var log = LogServiceExtension.GetSysLog(context.HttpContext);

            //字段赋值
            log.OperationType = this.OperationType;
            if (!String.IsNullOrEmpty(this.Description))
            {
                log.Remark = this.Description;
            }

            #region 设置返回值

            var isFileStream = context.Result.GetType().Equals(typeof(Microsoft.AspNetCore.Mvc.FileStreamResult));
            var isFile = context.Result.GetType().Equals(typeof(Microsoft.AspNetCore.Mvc.FileResult));
            var forbidenResult = context.Result.GetType().Equals(typeof(Microsoft.AspNetCore.Mvc.ForbidResult));
            if (!isFileStream && !isFile && !forbidenResult)
            {
                var result = JsonConvert.SerializeObject(((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value);
                if (result != null)
                {
                    if (result.Length > 2000)
                    {
                        log.Result = result.Substring(0, 2000) + "...";

                        //返回结果落盘
                        var monitoringLogsPath = String.Join(String.Empty, NGAdminGlobalContext.DirectoryConfig.GetMonitoringLogsPath(), "/" + log.Id + ".txt");
                        var file = new FileStream(monitoringLogsPath, FileMode.Create);
                        byte[] byteArray = System.Text.Encoding.Default.GetBytes(result);
                        file.Write(byteArray, 0, byteArray.Length);
                        file.Flush();
                        file.Close();
                    }
                    else
                    {
                        log.Result = result;
                    }
                }
            }

            #endregion

            //记录审计日志
            await log.WriteAsync();

            #endregion
        }
    }
}
