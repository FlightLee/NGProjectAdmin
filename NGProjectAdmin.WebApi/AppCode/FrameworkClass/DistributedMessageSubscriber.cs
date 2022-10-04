using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Entity.BusinessConstant;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Service.BusinessService.RedisService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.ScheduleJobService;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NGProjectAdmin.WebApi.AppCode.FrameworkClass
{
    /// <summary>
    /// 分布式消息订阅器
    /// </summary>
    public class DistributedMessageSubscriber : IHostedService
    {
        #region 属性及构造函数

        /// <summary>
        /// ServiceProvider实例
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// 计划任务接口实例
        /// </summary>
        private readonly IScheduleJobService scheduleJobService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="scheduleJobService"></param>
        public DistributedMessageSubscriber(IServiceProvider serviceProvider, IScheduleJobService scheduleJobService)
        {
            this.serviceProvider = serviceProvider;
            this.scheduleJobService = scheduleJobService;
        }

        #endregion

        #region 开始事件

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("DistributedMessageSubscriber Started");
            if (NGAdminGlobalContext.QuartzConfig.SupportGroup)
            {
                Task.Run(() =>
                {
                    using (var scope = this.serviceProvider.CreateScope())
                    {
                        var redisService = scope.ServiceProvider.GetService<IRedisService>();
                        redisService.SubscribeMessage(NGAdminGlobalContext.QuartzConfig.ChanelName, new Action<String>(async message =>
                        {
                            Console.WriteLine("DistributedMessageSubscriber Message Recieved");
                            var msg = JsonConvert.DeserializeObject<QuartzJobDTO>(message.ToString());
                            if (msg.GroupId != null && msg.GroupId == NGAdminGlobalContext.QuartzConfig.GroupId)
                            {
                                switch (msg.Action)
                                {
                                    case JobAction.Delete:
                                        await this.scheduleJobService.DeleteScheduleJobAsync(msg.JobId);
                                        break;
                                    case JobAction.Start:
                                        await this.scheduleJobService.StartScheduleJobAsync(msg.JobId);
                                        break;
                                    case JobAction.Pause:
                                        await this.scheduleJobService.PauseScheduleJobAsync(msg.JobId);
                                        break;
                                    case JobAction.Resume:
                                        await this.scheduleJobService.ResumeScheduleJobAsync(msg.JobId);
                                        break;
                                    default: break;
                                }
                            }
                        }));
                    }
                });
            }
            return Task.CompletedTask;
        }

        #endregion

        #region 停止事件

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("DistributedMessageSubscriber Stoped");
            return Task.CompletedTask;
        }

        #endregion
    }
}
