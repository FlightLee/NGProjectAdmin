using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.System;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Repository.Base;
using NGProjectAdmin.Service.BusinessService.SystemManagement.LogService;
using System;

namespace NGProjectAdmin.WebApi.AppCode.AuthorizationFilter
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ActionAuthorization : ActionFilterAttribute
    {
        #region 系统前置鉴权

        /// <summary>
        /// 系统前置鉴权
        /// </summary>
        /// <param name="context">ActionExecutingContext</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //处理匿名方法
            foreach (var item in context.ActionDescriptor.EndpointMetadata)
            {
                if (item.GetType().Name.Equals(typeof(AllowAnonymousAttribute).Name))
                {
                    return;
                }
            }

            //处理白名单
            var whiteList = NGAdminGlobalContext.SystemConfig.WhiteList;
            if (!String.IsNullOrEmpty(whiteList))
            {
                var array = whiteList.Split(',');
                var url = context.HttpContext.Request.Path.Value;
                foreach (var item in array)
                {
                    if (url.EndsWith(item))
                    {
                        return;
                    }
                }
            }

            //Jwt验证
            if (NGAdminGlobalContext.SystemConfig.CheckJwtToken)
            {
                if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Result = new UnauthorizedObjectResult("unauthorized jwt token");
                    return;
                }
            }

            //Token验证
            if (NGAdminGlobalContext.SystemConfig.CheckToken)
            {
                if (!context.HttpContext.Request.Headers.ContainsKey("token"))
                {
                    context.Result = new UnauthorizedObjectResult("token is neccesarry");
                    return;
                }
                else
                {
                    #region 用户token续时操作

                    //获取用户token
                    var token = context.HttpContext.GetToken();
                    //获取用户
                    var user = NGRedisContext.Get<SysUserDTO>(token);
                    //用户token合法
                    if (user != null)
                    {
                        String salt = context.HttpContext.GetTokenSalt();
                        Guid id = Guid.Parse(salt);

                        if (id == Guid.Empty)
                        {
                            //处理非法请求
                            context.Result = new BadRequestObjectResult("illegal salt,access denied");
                            return;
                        }

                        var entity = NGAdminDbScope.NGDbContext.Queryable<SysRecord>().Where(t => t.Id == id).First();
                        if (entity == null || int.Parse(entity.Remark) <= 1)
                        {
                            #region 记录合法salt

                            if (entity == null)
                            {
                                SysRecord record = new SysRecord();
                                record.Id = id;
                                record.Remark = "1";
                                record.Creator = user.Id;
                                record.CreateTime = DateTime.Now;
                                record.Modifier = user.Id;
                                record.ModifyTime = DateTime.Now;
                                record.VersionId = Guid.NewGuid();
                                NGAdminDbScope.NGDbContext.Insertable<SysRecord>(record).ExecuteCommand();
                            }
                            else
                            {
                                entity.Remark = "2";
                                entity.Modifier = user.Id;
                                entity.ModifyTime = DateTime.Now;
                                entity.VersionId = Guid.NewGuid();
                                NGAdminDbScope.NGDbContext.Updateable<SysRecord>(entity).ExecuteCommand();
                            }

                            #endregion

                            #region 用户token续时

                            var tokenExpiration = NGAdminGlobalContext.SystemConfig.UserTokenExpiration * 60;
                            NGRedisContext.Expire(token, tokenExpiration);

                            #endregion
                        }
                        else
                        {
                            #region 记录Token劫持行为

                            #region Token劫持记录审计日志

                            var log = LogServiceExtension.GetSysLog(context.HttpContext);

                            //字段赋值
                            log.OperationType = OperationType.TokenHijacked;
                            log.Remark = $"用户{String.Join("/", user.LogonName, user.DisplayName)}的Token被劫持，使用口令为{token}，请管理员警惕、关注！";

                            log.Write();

                            #endregion

                            #region 发送Token劫持告警消息

                            var msg = new SystemMessage();

                            msg.Message = "Broadcast";
                            msg.MessageType = MessageType.Broadcast;

                            BroadcastMessage broadcastMessage = new BroadcastMessage();
                            broadcastMessage.Title = "用户Token劫持告警";
                            broadcastMessage.Message = log.Remark;
                            broadcastMessage.MessageLevel = MessageLevel.Severity;

                            msg.Object = broadcastMessage;

                            NGActiveMQContext.SendTopic(JsonConvert.SerializeObject(msg));

                            #endregion

                            #endregion

                            context.Result = new ForbidResult("illegal access");
                            return;
                        }
                    }
                    else
                    {
                        //处理非法请求
                        context.Result = new UnauthorizedObjectResult("invalid user token");
                        return;
                    }

                    #endregion
                }
            }

            //其他头部验证
            var headerConfig = NGAdminGlobalContext.SystemConfig.HeaderConfig;
            if (!String.IsNullOrEmpty(headerConfig))
            {
                var array = headerConfig.Split(',');
                foreach (var item in array)
                {
                    if (!context.HttpContext.Request.Headers.ContainsKey(item))
                    {
                        context.Result = new BadRequestObjectResult(item.ToLower() + " is necessary");
                        return;
                    }
                }
            }
        }

        #endregion
    }
}
