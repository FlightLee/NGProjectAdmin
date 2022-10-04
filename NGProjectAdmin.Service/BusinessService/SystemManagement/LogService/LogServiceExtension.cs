using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Repository.Base;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.LogService
{
    /// <summary>
    /// 审计日志工具类
    /// </summary>
    public static class LogServiceExtension
    {
        #region 异步记录审计日志

        /// <summary>
        /// 记录审计日志
        /// </summary>
        /// <param name="log">日志对象</param>
        /// <returns></returns>
        public static async Task WriteAsync(this SysLog log)
        {
            //判断审计日志记录开关是否开启
            if (!NGAdminGlobalContext.LogConfig.IsEnabled)
            {
                return;
            }

            //if (NGAdminGlobalContext.LogConfig.SupportMongoDB)
            //{
            //    //审计日志存入MongoDB
            //    IMongoCollection<SysLog> mongoCollection = RuYiMongoDBContext.Instance.GetBusinessCollection<SysLog>("SysLog");
            //    await mongoCollection.InsertOneAsync(log);
            //} else
            if (NGAdminGlobalContext.LogConfig.SupportElasticsearch)
            {
                //审计日志存入Elasticsearch
                var response = await NGEsNestContext.Instance.IndexDocumentAsync<SysLog>(log);
                if (!response.IsValid)
                {
                    throw new NGAdminCustomException(response.OriginalException.Message);
                }
            }
            else if (NGAdminGlobalContext.LogConfig.SupportMeilisearch)
            {
                //审计日志存入Meilisearch
                var index =NGMeilisearchContext.Instance.GetIndex();
                await index.AddDocumentsAsync<SysLog>(new SysLog[] { log });
            }
            else
            {
                //审计日志存入关系库
                await NGAdminDbScope.NGDbContext.Insertable<SysLog>(log).ExecuteCommandAsync();
            }
        }

        #endregion

        #region 同步记录审计日志

        /// <summary>
        /// 记录审计日志
        /// </summary>
        /// <param name="log">日志对象</param>
        /// <returns></returns>
        public static void Write(this SysLog log)
        {
            //判断审计日志记录开关是否开启
            if (!NGAdminGlobalContext.LogConfig.IsEnabled)
            {
                return;
            }

            //if (NGAdminGlobalContext.LogConfig.SupportMongoDB)
            //{
            //    //审计日志存入MongoDB
            //    IMongoCollection<SysLog> mongoCollection = RuYiMongoDBContext.Instance.GetBusinessCollection<SysLog>("SysLog");
            //    mongoCollection.InsertOne(log);
            //}
            //else
            if (NGAdminGlobalContext.LogConfig.SupportElasticsearch)
            {
                //审计日志存入Elasticsearch
                var response = NGEsNestContext.Instance.IndexDocument<SysLog>(log);
                if (!response.IsValid)
                {
                    throw new NGAdminCustomException(response.OriginalException.Message);
                }
            }
            else if (NGAdminGlobalContext.LogConfig.SupportMeilisearch)
            {
                //审计日志存入Meilisearch
                var index = NGMeilisearchContext.Instance.GetIndex();
                index.AddDocumentsAsync<SysLog>(new SysLog[] { log });
            }
            else
            {
                //审计日志存入关系库
                NGAdminDbScope.NGDbContext.Insertable<SysLog>(log).ExecuteCommand();
            }
        }

        #endregion

        #region 获取审计日志对象

        /// <summary>
        /// 获取审计日志对象
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>审计日志对象</returns>
        public static SysLog GetSysLog(HttpContext httpContext)
        {
            var token = httpContext.GetToken();
            //获取用户
            var user = NGRedisContext.Get<SysUserDTO>(token);

            SysLog log = new SysLog();

            log.Id = Guid.NewGuid();

            log.UserId = user.Id;
            log.UserName = user.DisplayName + "/" + user.LogonName;

            log.OrgId = user.OrgId;
            if (log.OrgId.Equals(Guid.Empty))
            {
                log.OrgName = "none";
            }
            else
            {
                log.OrgName = user.OrgName;
            }

            log.System = httpContext.Request.Headers["User-Agent"].FirstOrDefault().ToString().Split('(')[1].Split(')')[0];
            log.Browser = httpContext.Request.Headers["sec-ch-ua"];

            var ip = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = httpContext.Connection.RemoteIpAddress.ToString();
            }
            log.IP = ip;

            log.RequestMethod = httpContext.Request.Method;
            log.RequestUrl = httpContext.Request.Path.Value;

            #region 设置日志参数

            switch (httpContext.Request.Method)
            {
                case "GET":
                case "DELETE":
                    log.Params = httpContext.Request.Path;
                    break;
                case "PUT":
                case "POST":
                    httpContext.Request.EnableBuffering();
                    httpContext.Request.Body.Position = 0;
                    StreamReader reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8);
                    log.Params = reader.ReadToEndAsync().GetAwaiter().GetResult();
                    httpContext.Request.Body.Position = 0;
                    break;
                default: break;
            }
            if (log.Params != null && log.Params.Length > 1024)
            {
                log.Params = log.Params.Substring(0, 1021) + "...";
            }

            #endregion

            log.OldValue = String.Empty;
            log.NewValue = String.Empty;
            log.Remark = $"{user.DisplayName}于{DateTime.Now}访问了{log.RequestUrl}接口";

            log.IsDel = (int)DeletionType.Undeleted;
            log.Creator = user.Id;
            log.CreateTime = DateTime.Now;
            log.Modifier = user.Id;
            log.ModifyTime = DateTime.Now;
            log.VersionId = Guid.NewGuid();

            return log;
        }

        #endregion

        #region 无口令获取日志对象

        /// <summary>
        /// 无口令获取日志对象
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>日志对象</returns>
        public static SysLog GetSysLogWithoutToken(HttpContext httpContext)
        {
            SysLog log = new SysLog();

            log.Id = Guid.NewGuid();

            log.UserId = Guid.Empty;
            log.UserName = String.Empty;

            log.OrgId = Guid.Empty;
            log.OrgName = "none";

            log.System = httpContext.Request.Headers["User-Agent"].FirstOrDefault().ToString().Split('(')[1].Split(')')[0];
            log.Browser = httpContext.Request.Headers["sec-ch-ua"];

            var ip = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = httpContext.Connection.RemoteIpAddress.ToString();
            }
            log.IP = ip;

            log.RequestMethod = httpContext.Request.Method;
            log.RequestUrl = httpContext.Request.Path.Value;

            #region 设置日志参数

            switch (httpContext.Request.Method)
            {
                case "GET":
                case "DELETE":
                    log.Params = httpContext.Request.Path;
                    break;
                case "PUT":
                case "POST":
                    httpContext.Request.EnableBuffering();
                    httpContext.Request.Body.Position = 0;
                    StreamReader reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8);
                    log.Params = reader.ReadToEndAsync().GetAwaiter().GetResult();
                    httpContext.Request.Body.Position = 0;
                    break;
                default: break;
            }
            if (log.Params != null && log.Params.Length > 1024)
            {
                log.Params = log.Params.Substring(0, 1021) + "...";
            }

            #endregion

            log.OldValue = String.Empty;
            log.NewValue = String.Empty;
            log.Remark = $"于{DateTime.Now}访问了{log.RequestUrl}接口";

            log.IsDel = (int)DeletionType.Undeleted;
            log.Creator = Guid.Empty;
            log.CreateTime = DateTime.Now;
            log.Modifier = Guid.Empty;
            log.ModifyTime = DateTime.Now;
            log.VersionId = Guid.NewGuid();

            return log;
        }

        #endregion
    }
}
