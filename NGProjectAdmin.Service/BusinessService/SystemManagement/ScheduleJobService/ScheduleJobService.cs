
using Quartz;
using Quartz.Impl;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.Base;
using NGProjectAdmin.Repository.BusinessRepository.RedisRepository;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.ScheduleJobRepository;
using NGProjectAdmin.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.ScheduleJobService
{
    /// <summary>
    /// 计划任务业务层实现
    /// </summary>
    public class ScheduleJobService : NGAdminBaseService<SysScheduleJob>, IScheduleJobService
    {
        #region 属性及构造函数

        /// <summary>
        /// 计划任务仓储实例
        /// </summary>
        private readonly IScheduleJobRepository scheduleJobRepository;

        /// <summary>
        /// 计划任务
        /// </summary>
        private IScheduler scheduler;

        /// <summary>
        /// Redis仓储实例
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scheduleJobRepository"></param>
        public ScheduleJobService(IScheduleJobRepository scheduleJobRepository,
                                  IRedisRepository redisRepository) : base(scheduleJobRepository)
        {
            this.scheduleJobRepository = scheduleJobRepository;
            this.redisRepository = redisRepository;
        }

        #endregion

        #region 启动计划任务

        /// <summary>
        /// 启动计划任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <returns></returns>
        public async Task StartScheduleJobAsync(Guid jobId)
        {
            try
            {
                var scheduleJobs = await this.redisRepository.GetAsync<List<SysScheduleJob>>(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName);
                SysScheduleJob scheduleJob = scheduleJobs.Where(t => t.Id == jobId).FirstOrDefault();

                if (scheduleJob != null)
                {
                    #region 预设运行时间

                    if (scheduleJob.StartTime == null)
                    {
                        scheduleJob.StartTime = DateTime.Now;
                    }
                    DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(scheduleJob.StartTime, 1);

                    if (scheduleJob.EndTime == null)
                    {
                        scheduleJob.EndTime = DateTime.MaxValue.AddDays(-1);
                    }
                    DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(scheduleJob.EndTime, 1);

                    #endregion

                    var jobName = scheduleJob.JobName;
                    var jobGroup = NGAdminGlobalContext.QuartzConfig.ScheduleJobGroup;
                    var jobTrigger = NGAdminGlobalContext.QuartzConfig.ScheduleJobTrigger + "/" + jobName;

                    scheduler = await GetSchedulerAsync();

                    // 构建JobDetail
                    IJobDetail job = JobBuilder.Create(Type.GetType($"{scheduleJob.NameSpace}.{scheduleJob.JobImplement}"))
                                               .WithIdentity(jobName, jobGroup)
                                               .Build();
                    // 构建JobTrigger
                    ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                                                       .StartAt(starRunTime)
                                                                       .EndAt(endRunTime)
                                                                       .WithIdentity(jobTrigger, jobGroup)
                                                                       .WithCronSchedule(scheduleJob.CronExpression)
                                                                       .Build();

                    await scheduler.ScheduleJob(job, trigger);
                    await scheduler.Start();

                    //更新任务状态
                    scheduleJob.JobStatus = JobStatus.Running;
                    //this.scheduleJobRepository.UpdateEntity(scheduleJob);

                    scheduleJob.Modifier = Guid.NewGuid();
                    scheduleJob.ModifyTime = DateTime.Now;
                    scheduleJob.VersionId = Guid.NewGuid();

                    //更新状态
                    NGAdminDbScope.NGDbContext.Updateable<SysScheduleJob>(scheduleJob).ExecuteCommand();

                    #region 数据一致性维护

                    //删除旧数据
                    var old = scheduleJobs.Where(t => t.Id == scheduleJob.Id).FirstOrDefault();
                    scheduleJobs.Remove(old);

                    //添加新数据
                    scheduleJobs.Add(scheduleJob);

                    //回写缓存
                    await this.redisRepository.SetAsync(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName, scheduleJobs, -1);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException(ex.Message);
            }
        }

        #endregion

        #region 暂停计划任务

        /// <summary>
        /// 暂停计划任务
        /// </summary>
        /// <param name="jobId">作业编号</param>
        /// <returns></returns>
        public async Task PauseScheduleJobAsync(Guid jobId)
        {
            try
            {
                var scheduleJobs = await this.redisRepository.GetAsync<List<SysScheduleJob>>(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName);
                SysScheduleJob scheduleJob = scheduleJobs.Where(t => t.Id == jobId).FirstOrDefault();

                if (scheduleJob != null)
                {
                    var jobName = scheduleJob.JobName;
                    var jobGroup = NGAdminGlobalContext.QuartzConfig.ScheduleJobGroup;

                    scheduler = await GetSchedulerAsync();
                    var jobKey = new JobKey(jobName, jobGroup);

                    if (await scheduler.CheckExists(jobKey))
                    {
                        //任务暂停
                        await scheduler.PauseJob(jobKey);
                    }

                    scheduleJob.JobStatus = JobStatus.Stopped;
                    //this.scheduleJobRepository.UpdateEntity(scheduleJob);

                    scheduleJob.Modifier = Guid.NewGuid();
                    scheduleJob.ModifyTime = DateTime.Now;
                    scheduleJob.VersionId = Guid.NewGuid();

                    //更新状态
                    NGAdminDbScope.NGDbContext.Updateable<SysScheduleJob>(scheduleJob).ExecuteCommand();

                    #region 数据一致性维护

                    //删除旧数据
                    var old = scheduleJobs.Where(t => t.Id == scheduleJob.Id).FirstOrDefault();
                    scheduleJobs.Remove(old);

                    //添加新数据
                    scheduleJobs.Add(scheduleJob);

                    //回写缓存
                    await this.redisRepository.SetAsync(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName, scheduleJobs, -1);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException(ex.Message);
            }
        }

        #endregion

        #region 恢复计划任务

        /// <summary>
        /// 恢复计划任务
        /// </summary>
        /// <param name="jobId">作业编号</param>
        /// <returns></returns>
        public async Task ResumeScheduleJobAsync(Guid jobId)
        {
            try
            {
                var scheduleJobs = await this.redisRepository.GetAsync<List<SysScheduleJob>>(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName);
                SysScheduleJob scheduleJob = scheduleJobs.Where(t => t.Id == jobId).FirstOrDefault();

                if (scheduleJob != null)
                {
                    var jobName = scheduleJob.JobName;
                    var jobGroup = NGAdminGlobalContext.QuartzConfig.ScheduleJobGroup;

                    scheduler = await GetSchedulerAsync();
                    var jobKey = new JobKey(jobName, jobGroup);

                    if (!await scheduler.CheckExists(jobKey))
                    {
                        await StartScheduleJobAsync(jobId);
                    }
                    //恢复
                    await scheduler.ResumeJob(jobKey);

                    scheduleJob.JobStatus = JobStatus.Running;
                    //this.scheduleJobRepository.UpdateEntity(scheduleJob);

                    scheduleJob.Modifier = Guid.NewGuid();
                    scheduleJob.ModifyTime = DateTime.Now;
                    scheduleJob.VersionId = Guid.NewGuid();

                    //更新状态
                    NGAdminDbScope.NGDbContext.Updateable<SysScheduleJob>(scheduleJob).ExecuteCommand();

                    #region 数据一致性维护

                    //删除旧数据
                    var old = scheduleJobs.Where(t => t.Id == scheduleJob.Id).FirstOrDefault();
                    scheduleJobs.Remove(old);

                    //添加新数据
                    scheduleJobs.Add(scheduleJob);

                    //回写缓存
                    await this.redisRepository.SetAsync(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName, scheduleJobs, -1);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException(ex.Message);
            }
        }

        #endregion

        #region 删除计划任务

        /// <summary>
        /// 删除计划任务
        /// </summary>
        /// <param name="jobId">作业编号</param>
        /// <returns></returns>
        public async Task DeleteScheduleJobAsync(Guid jobId)
        {
            try
            {
                var scheduleJobs = await this.redisRepository.GetAsync<List<SysScheduleJob>>(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName);
                SysScheduleJob scheduleJob = scheduleJobs.Where(t => t.Id == jobId).FirstOrDefault();

                if (scheduleJob != null)
                {
                    var jobName = scheduleJob.JobName;
                    var jobGroup = NGAdminGlobalContext.QuartzConfig.ScheduleJobGroup;

                    scheduler = await GetSchedulerAsync();
                    var jobKey = new JobKey(jobName, jobGroup);

                    if (await scheduler.CheckExists(jobKey))
                    {
                        await scheduler.PauseJob(jobKey);
                        await scheduler.DeleteJob(jobKey);
                    }

                    scheduleJob.JobStatus = JobStatus.Stopped;
                    //this.scheduleJobRepository.UpdateEntity(scheduleJob);

                    scheduleJob.IsDel = (int)DeletionType.Deleted;
                    scheduleJob.Modifier = Guid.NewGuid();
                    scheduleJob.ModifyTime = DateTime.Now;
                    scheduleJob.VersionId = Guid.NewGuid();

                    //逻辑删除
                    NGAdminDbScope.NGDbContext.Updateable<SysScheduleJob>(scheduleJob).ExecuteCommand();

                    #region 数据一致性维护

                    //删除旧数据
                    var old = scheduleJobs.Where(t => t.Id == scheduleJob.Id).FirstOrDefault();
                    scheduleJobs.Remove(old);

                    //回写缓存
                    await this.redisRepository.SetAsync(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName, scheduleJobs, -1);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException(ex.Message);
            }
        }

        #endregion

        #region 启动业务作业

        /// <summary>
        /// 启动业务作业
        /// </summary>
        /// <returns></returns>
        public async Task StartScheduleJobAsync()
        {
            var scheduleJobs = await this.redisRepository.GetAsync<List<SysScheduleJob>>(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName);

            //支持集群作业
            if (NGAdminGlobalContext.QuartzConfig.SupportGroup)
            {
                //仅加载本节点定时任务
                scheduleJobs = scheduleJobs.Where(t => t.GroupId == NGAdminGlobalContext.QuartzConfig.GroupId).ToList();
            }

            foreach (var item in scheduleJobs)
            {
                if (item.JobStatus.Equals(JobStatus.Running))
                {
                    #region 预设时间

                    if (item.StartTime == null)
                    {
                        item.StartTime = DateTime.Now;
                    }
                    DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(item.StartTime, 1);

                    if (item.EndTime == null)
                    {
                        item.EndTime = DateTime.MaxValue.AddDays(-1);
                    }
                    DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(item.EndTime, 1);

                    #endregion

                    var jobName = item.JobName;
                    var jobGroup = NGAdminGlobalContext.QuartzConfig.ScheduleJobGroup;
                    var jobTrigger = NGAdminGlobalContext.QuartzConfig.ScheduleJobTrigger + "/" + jobName;

                    var schedf = new StdSchedulerFactory();
                    var scheduler = await schedf.GetScheduler();

                    IJobDetail job = JobBuilder.Create(Type.GetType($"{item.NameSpace}.{item.JobImplement}"))
                      .WithIdentity(jobName, jobGroup)
                      .Build();
                    ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                                 .StartAt(starRunTime)
                                                 .EndAt(endRunTime)
                                                 .WithIdentity(jobTrigger, jobGroup)
                                                 .WithCronSchedule(item.CronExpression)
                                                 .Build();

                    await scheduler.ScheduleJob(job, trigger);
                    await scheduler.Start();
                }
            }
        }

        #endregion

        #region 加载计划任务缓存

        /// <summary>
        /// 加载计划任务缓存
        /// </summary>
        public async Task LoadBusinessScheduleJobCache()
        {
            var sqlKey = "sqls:sql:query_sysschedulejob";
            var strSQL = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value;

            int totalCount = 0;
            var scheduleJobs = await this.scheduleJobRepository.SqlQueryAsync(new QueryCondition(), totalCount, strSQL);
            await this.redisRepository.SetAsync(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName, scheduleJobs, -1);

            NGLoggerContext.Info("系统计划任务缓存加载完成");
        }

        #endregion

        #region 清理计划任务缓存

        /// <summary>
        /// 清理计划任务缓存
        /// </summary>
        public async Task ClearBusinessScheduleJobCache()
        {
            await this.redisRepository.DeleteAsync(new String[] { NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName });

            NGLoggerContext.Info("系统计划任务缓存清理完成");
        }

        #endregion

        #region 获取任务对象

        /// <summary>
        /// 获取任务对象
        /// </summary>
        /// <returns>任务对象</returns>
        private async Task<IScheduler> GetSchedulerAsync()
        {
            if (scheduler != null)
            {
                return scheduler;
            }
            else
            {
                ISchedulerFactory schedf = new StdSchedulerFactory();
                scheduler = await schedf.GetScheduler();
                return scheduler;
            }
        }

        #endregion
    }
}
