using Autofac.Extensions.DependencyInjection;
using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.WebApi.AppCode.FrameworkExtensions;
using Quartz;
using NGProjectAdmin.WebApi.AppCode.FrameworkQuartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NGProjectAdmin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            #region �޸�PostgreSQL DateTimeд�����
          

            //Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone'
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

            #endregion

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
         .ConfigureWebHostDefaults(webBuilder =>
         {
             #region �����ļ��ȼ��ء��ȸ���

             var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddCommandLine(args)
               .Build();
             webBuilder.UseConfiguration(config).UseStartup<Startup>();

             #endregion
         })
         .ConfigureAppConfiguration((hostingContext, config) =>
         {
             #region SQL�ļ��ȼ��ء��ȸ���

             var basePath = Directory.GetCurrentDirectory();
             var directory = Path.Join(basePath, "SqlConfig");
             var files = Directory.GetFiles(directory, "*.config", SearchOption.AllDirectories);
             foreach (var file in files)
             {
                 config.AddXmlFile(file, optional: true, reloadOnChange: true);
             }

             #endregion

             #region ע��Apollo�ͻ�������

             var apolloSection = config.Build().GetSection("ApolloConfig");
             var apolloConfig = apolloSection.Get<ApolloConfig>();
             if (apolloConfig.IsEnabled)
             {
                 LogManager.UseConsoleLogging(Com.Ctrip.Framework.Apollo.Logging.LogLevel.Trace);
                 config.AddApollo(apolloSection).AddDefault();
             }

             #endregion

             #region ע��AspNetCoreRateLimit�������

             config.AddJsonFile("NGAdminRateLimitConfig.json", optional: true, reloadOnChange: true);

             #endregion
         })
         .UseServiceProviderFactory(new AutofacServiceProviderFactory())
         .ConfigureServices((hostContext, services) =>
         {
             #region ע����Quartz��ҵ

             services.AddQuartz(q =>
             {
                 q.UseMicrosoftDependencyInjectionJobFactory();

                 // Register the job, loading the schedule from configuration
                 q.AddJobAndTrigger<NGAdminFrameworkJob>(hostContext.Configuration);
             });

             services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

             #endregion
         });
    }
}
