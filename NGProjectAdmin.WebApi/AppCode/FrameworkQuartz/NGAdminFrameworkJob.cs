//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Quartz;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using System;
using System.Threading.Tasks;

namespace RuYiAdmin.Net.WebApi.AppCode.FrameworkQuartz
{
    [DisallowConcurrentExecution]
    public class NGAdminFrameworkJob : IJob
    {
        private readonly ILogger<NGAdminFrameworkJob> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public NGAdminFrameworkJob(ILogger<NGAdminFrameworkJob> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 执行框架作业
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            String time = DateTime.Now.ToString("HH:mm");
            if (time.Equals("04:00"))
            {
                //清理临时文件、释放服务器存储空间
                NGFileContext.ClearDirectory(NGAdminGlobalContext.DirectoryConfig.GetTempPath());
            }

            _logger.LogInformation("RuYiAdmin Framework Job Executed!");

            return Task.CompletedTask;
        }
    }
}