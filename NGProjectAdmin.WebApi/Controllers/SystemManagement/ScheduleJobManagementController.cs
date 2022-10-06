//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Quartz;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Class.Extensions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Entity.BusinessConstant;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.BusinessService.RedisService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.ScheduleJobService;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 计划任务控制器
    /// </summary>
    public class ScheduleJobManagementController : NGAdminBaseController<SysScheduleJob>
    {
        #region 属性及构造函数

        /// <summary>
        /// 计划任务接口实例
        /// </summary>
        private readonly IScheduleJobService scheduleJobService;

        /// <summary>
        /// Redis服务接口实例
        /// </summary>
        private readonly IRedisService redisService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scheduleJobService"></param>
        /// <param name="redisService"></param>
        public ScheduleJobManagementController(IScheduleJobService scheduleJobService,
                                               IRedisService redisService) : base(scheduleJobService)
        {
            this.scheduleJobService = scheduleJobService;
            this.redisService = redisService;
        }

        #endregion

        #region 查询任务列表

        /// <summary>
        /// 查询任务列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        [Permission("job:query:list")]
        public async Task<IActionResult> Post(QueryCondition queryCondition)
        {
            var scheduleJobs = await this.redisService.GetAsync<List<SysScheduleJob>>(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName);
            scheduleJobs = scheduleJobs.AsQueryable().Where(QueryCondition.BuildExpression<SysScheduleJob>(queryCondition.QueryItems)).ToList();

            if (!String.IsNullOrEmpty(queryCondition.Sort))
            {
                scheduleJobs = scheduleJobs.Sort<SysScheduleJob>(queryCondition.Sort);
            }

            var actionResult = new QueryResult<SysScheduleJob>();
            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.TotalCount = scheduleJobs.Count;
            actionResult.List = scheduleJobs.Skip(queryCondition.PageIndex * queryCondition.PageSize).Take(queryCondition.PageSize).ToList();

            return Ok(actionResult);
        }

        #endregion

        #region 查询任务信息

        /// <summary>
        /// 查询任务信息
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{jobId}")]
        [Log(OperationType.QueryEntity)]
        [Permission("job:query:list")]
        public async Task<IActionResult> GetById(Guid jobId)
        {
            var actionResult = new Entity.CoreEntity.ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            var scheduleJobs = await this.redisService.GetAsync<List<SysScheduleJob>>(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName);
            actionResult.Object = scheduleJobs.Where(t => t.Id == jobId).FirstOrDefault();

            return Ok(actionResult);
        }

        #endregion

        #region 新增计划任务

        /// <summary>
        /// 新增计划任务
        /// </summary>
        /// <param name="scheduleJob">计划任务对象</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.AddEntity)]
        [Permission("job:add:entity")]
        public async Task<IActionResult> Add([FromBody] SysScheduleJob scheduleJob)
        {
            //校验CronExpression表达式
            bool expression = CronExpression.IsValidExpression(scheduleJob.CronExpression);
            if (!expression)
            {
                throw new NGAdminCustomException("invalid cron expression");
            }

            scheduleJob.JobStatus = JobStatus.Planning;

            //支持集群作业
            if (NGAdminGlobalContext.QuartzConfig.SupportGroup)
            {
                //保存为本节点定时任务
                scheduleJob.GroupId = NGAdminGlobalContext.QuartzConfig.GroupId;
            }

            var actionResult = await this.scheduleJobService.AddAsync(scheduleJob);

            #region 数据一致性维护

            var scheduleJobs = await this.redisService.GetAsync<List<SysScheduleJob>>(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName);

            //添加新数据
            scheduleJobs.Add(scheduleJob);

            //回写缓存
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName, scheduleJobs, -1);

            #endregion

            return Ok(actionResult);
        }

        #endregion

        #region 编辑计划任务

        /// <summary>
        /// 编辑计划任务
        /// </summary>
        /// <param name="scheduleJob">计划任务对象</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Log(OperationType.EditEntity)]
        [Permission("job:edit:entity")]
        public async Task<IActionResult> Put([FromBody] SysScheduleJob scheduleJob)
        {
            //校验CronExpression表达式
            bool expression = CronExpression.IsValidExpression(scheduleJob.CronExpression);
            if (!expression)
            {
                throw new NGAdminCustomException("invalid cron expression");
            }

            var scheduleJobs = await this.redisService.GetAsync<List<SysScheduleJob>>(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName);
            var job = scheduleJobs.Where(t => t.Id == scheduleJob.Id).FirstOrDefault();

            if (job.JobStatus == JobStatus.Running)
            {
                throw new NGAdminCustomException("job is running,please stop it first");
            }

            var actionResult = await this.scheduleJobService.UpdateAsync(scheduleJob);

            #region 数据一致性维护

            //删除旧数据
            scheduleJobs.Remove(job);

            //添加新数据
            scheduleJobs.Add(scheduleJob);

            //回写缓存
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName, scheduleJobs, -1);

            #endregion

            return Ok(actionResult);
        }

        #endregion

        #region 删除计划任务

        /// <summary>
        /// 删除计划任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{jobId}")]
        [Permission("job:del:entity")]
        [Log(OperationType.DeleteEntity)]
        public async Task<IActionResult> DeleteAsync(Guid jobId)
        {
            var scheduleJobs = await this.redisService.GetAsync<List<SysScheduleJob>>(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName);
            var job = scheduleJobs.Where(t => t.Id == jobId).FirstOrDefault();

            if (!NGAdminGlobalContext.QuartzConfig.SupportGroup)
            {
                //单机版，不支持集群
                await this.scheduleJobService.DeleteScheduleJobAsync(jobId);

                #region 数据一致性维护

                //删除旧数据
                scheduleJobs.Remove(job);

                //回写缓存
                await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName, scheduleJobs, -1);

                #endregion

                var actionResult = new Entity.CoreEntity.ActionResult();
                actionResult.HttpStatusCode = HttpStatusCode.OK;
                actionResult.Message = "OK";

                return Ok(actionResult);
            }
            else if (NGAdminGlobalContext.QuartzConfig.SupportGroup && job.GroupId != null && job.GroupId == NGAdminGlobalContext.QuartzConfig.GroupId)
            {
                //支持集群、且为本节点任务
                await this.scheduleJobService.DeleteScheduleJobAsync(jobId);

                #region 数据一致性维护

                //删除旧数据
                scheduleJobs.Remove(job);

                //回写缓存
                await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName, scheduleJobs, -1);

                #endregion

                var actionResult = new Entity.CoreEntity.ActionResult();
                actionResult.HttpStatusCode = HttpStatusCode.OK;
                actionResult.Message = "OK";

                return Ok(actionResult);
            }
            else
            {
                var msg = JsonConvert.SerializeObject(new QuartzJobDTO()
                {
                    JobId = job.Id,
                    GroupId = job.GroupId,
                    Action = JobAction.Delete
                });

                //支持集群、且不为本节点任务
                this.redisService.PublishMessage(NGAdminGlobalContext.QuartzConfig.ChanelName, msg);

                return Ok("OK");
            }
        }

        #endregion

        #region 启动计划任务

        /// <summary>
        /// 启动计划任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{jobId}")]
        [Log(OperationType.StartScheduleJob)]
        [Permission("schedule:job:add")]
        public async Task<IActionResult> StartScheduleJobAsync(Guid jobId)
        {
            var scheduleJobs = await this.redisService.GetAsync<List<SysScheduleJob>>(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName);
            var job = scheduleJobs.Where(t => t.Id == jobId).FirstOrDefault();

            if (!NGAdminGlobalContext.QuartzConfig.SupportGroup)
            {
                //单机版，不支持集群
                await this.scheduleJobService.StartScheduleJobAsync(jobId);
            }
            else if (NGAdminGlobalContext.QuartzConfig.SupportGroup && job.GroupId != null && job.GroupId == NGAdminGlobalContext.QuartzConfig.GroupId)
            {
                //支持集群、且为本节点任务
                await this.scheduleJobService.StartScheduleJobAsync(jobId);
            }
            else
            {
                var msg = JsonConvert.SerializeObject(new QuartzJobDTO()
                {
                    JobId = job.Id,
                    GroupId = job.GroupId,
                    Action = JobAction.Start
                });

                //支持集群、且不为本节点任务
                this.redisService.PublishMessage(NGAdminGlobalContext.QuartzConfig.ChanelName, msg);
            }

            return Ok("OK");
        }

        #endregion

        #region 暂停计划任务

        /// <summary>
        /// 暂停计划任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{jobId}")]
        [Log(OperationType.PauseScheduleJob)]
        [Permission("schedule:job:pause")]
        public async Task<IActionResult> PauseScheduleJob(Guid jobId)
        {
            var scheduleJobs = await this.redisService.GetAsync<List<SysScheduleJob>>(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName);
            var job = scheduleJobs.Where(t => t.Id == jobId).FirstOrDefault();

            if (!NGAdminGlobalContext.QuartzConfig.SupportGroup)
            {
                //单机版，不支持集群
                await this.scheduleJobService.PauseScheduleJobAsync(jobId);
            }
            else if (NGAdminGlobalContext.QuartzConfig.SupportGroup && job.GroupId != null && job.GroupId == NGAdminGlobalContext.QuartzConfig.GroupId)
            {
                //支持集群、且为本节点任务
                await this.scheduleJobService.PauseScheduleJobAsync(jobId);
            }
            else
            {
                var msg = JsonConvert.SerializeObject(new QuartzJobDTO()
                {
                    JobId = job.Id,
                    GroupId = job.GroupId,
                    Action = JobAction.Pause
                });

                //支持集群、且不为本节点任务
                this.redisService.PublishMessage(NGAdminGlobalContext.QuartzConfig.ChanelName, msg);
            }

            return Ok("OK");
        }

        #endregion

        #region 恢复计划任务

        /// <summary>
        /// 恢复计划任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{jobId}")]
        [Log(OperationType.ResumeScheduleJob)]
        [Permission("schedule:job:resume")]
        public async Task<IActionResult> ResumeScheduleJob(Guid jobId)
        {
            var scheduleJobs = await this.redisService.GetAsync<List<SysScheduleJob>>(NGAdminGlobalContext.SystemCacheConfig.ScheduleJobCacheName);
            var job = scheduleJobs.Where(t => t.Id == jobId).FirstOrDefault();

            if (!NGAdminGlobalContext.QuartzConfig.SupportGroup)
            {
                //单机版，不支持集群
                await this.scheduleJobService.ResumeScheduleJobAsync(jobId);
            }
            else if (NGAdminGlobalContext.QuartzConfig.SupportGroup && job.GroupId != null && job.GroupId == NGAdminGlobalContext.QuartzConfig.GroupId)
            {
                //支持集群、且为本节点任务
                await this.scheduleJobService.ResumeScheduleJobAsync(jobId);
            }
            else
            {
                var msg = JsonConvert.SerializeObject(new QuartzJobDTO()
                {
                    JobId = job.Id,
                    GroupId = job.GroupId,
                    Action = JobAction.Resume
                });

                //支持集群、且不为本节点任务
                this.redisService.PublishMessage(NGAdminGlobalContext.QuartzConfig.ChanelName, msg);
            }

            return Ok("OK");
        }

        #endregion
    }
}
