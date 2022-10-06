using FluentFTP;
using Microsoft.Extensions.Configuration;
using NGProjectAdmin.Net.Common.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Common.Global
{
    public class NGAdminGlobalContext
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        public static DBConfig DBConfig { get; set; }

        /// <summary>
        /// 系统配置
        /// </summary>
        public static SystemConfig SystemConfig { get; set; }

        /// <summary>
        /// Redis配置
        /// </summary>
        public static RedisConfig RedisConfig { get; set; }


        /// <summary>
        /// Elasticsearch配置    全文检索 暂不启用
        /// </summary>
        public static ElasticsearchConfig ElasticsearchConfig { get; set; }


        /// <summary>
        /// Ftp配置 暂不启用
        /// </summary>
        public static FtpConfig FtpConfig { get; set; }

        /// <summary>
        /// Kafka配置  消息订阅推送 暂不启用 高数据量大数据处理
        /// </summary>
        public static KafkaConfig KafkaConfig { get; set; }


        /// <summary>
        /// RabbitMQ配置  消息订阅推送 暂不启用 可靠性较高
        /// </summary>
        public static RabbitMQConfig RabbitMQConfig { get; set; }


        /// <summary>
        /// SmartThreadPool配置  暂不启用 线程池 多任务批量处理
        /// </summary>
        public static SmartThreadPoolConfig SmartThreadPoolConfig { get; set; }

        /// <summary>
        /// 系统目录配置
        /// </summary>
        public static DirectoryConfig DirectoryConfig { get; set; }

        /// <summary>
        /// 审计日志设置 暂不启用
        /// </summary>
        public static LogConfig LogConfig { get; set; }

        /// <summary>
        /// Meilisearch配置   --全文搜索 对中文支持更友好 暂不启用
        /// </summary>
        public static MeilisearchConfig MeilisearchConfig { get; set; }

        /// <summary>
        /// 代码生成器配置
        /// </summary>
        public static CodeGeneratorConfig CodeGeneratorConfig { get; set; }

        /// <summary>
        /// Jwt配置
        /// </summary>
        public static JwtSettings JwtSettings { get; set; }

        /// <summary>
        /// 系统并发配置
        /// </summary>
        public static ConcurrencyLimiterConfig ConcurrencyLimiterConfig { get; set; }

        /// <summary>
        /// 全局配置
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 系统缓存配置
        /// </summary>
        public static SystemCacheConfig SystemCacheConfig { get; set; }

        /// <summary>
        /// ActiveMQ配置  消息队列  消息订阅推送 
        /// </summary>
        public static ActiveMQConfig ActiveMQConfig { get; set; }

        /// <summary>
        /// MiniProfiler配置
        /// </summary>
        public static MiniProfilerConfig MiniProfilerConfig { get; set; }

        /// <summary>
        /// Cap分布式事务配置
        /// </summary>
        public static CapConfig CapConfig { get; set; }

        /// <summary>
        /// Consul配置
        /// </summary>
        public static ConsulConfig ConsulConfig { get; set; }

        /// <summary>
        /// 消息中间件配置
        /// </summary>
        public static MomConfig MomConfig { get; set; }


        /// <summary>
        /// SignalR配置
        /// </summary>
        public static SignalRConfig SignalRConfig { get; set; }

        /// <summary>
        /// 定时任务配置
        /// </summary>
        public static QuartzConfig QuartzConfig { get; set; }

        /// <summary>
        /// AspNetCoreRateLimit配置
        /// </summary>
        public static RateLimitConfig RateLimitConfig { get; set; }


        /// <summary>
        /// 全局路由模板
        /// </summary>
        public const String RouteTemplate = "API/[controller]/[action]";

        /// <summary>
        /// Smtp邮件配置
        /// </summary>
        public static MailConfig MailConfig { get; set; }
    }
}
