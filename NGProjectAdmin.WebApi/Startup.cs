using AspNetCoreRateLimit;
using Autofac;
using DotNetCore.CAP.Dashboard.NodeDiscovery;
using Lazy.Captcha.Core;
using Lazy.Captcha.Core.Generator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Entity.AutoMapperConfig;
using NGProjectAdmin.Net.Common.Global;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.AuthorizationFilter;
using NGProjectAdmin.WebApi.AppCode.FrameworkClass;
using NGProjectAdmin.WebApi.AppCode.FrameworkExtensions;
using NGProjectAdmin.WebApi.AppCode.IoCDependencyInjection;
using SqlSugar;
using StackExchange.Profiling.Storage;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region 注册IoC控制反转

            builder.RegisterModule<DependencyAutoInjection>();

            #endregion
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 注册AspNetCoreRateLimit组件

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = NGAdminGlobalContext.RedisConfig.ConnectionString;
                options.InstanceName = NGAdminGlobalContext.RateLimitConfig.InstanceName;
            });

            //加载配置
            services.AddOptions();

            //加载相应配置
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            //加载Ip规则
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));

            //注入计数器和规则存储
            services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();

            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            services.AddControllers();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //配置（计数器密钥生成器）
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            #endregion

            #region 注册系统Swagger组件
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NGAdmin ASP.NET Core WebApi Documentation",
                    Description = "NGAdmin接口<br/>",
                    Version = "V-1.0"
                }); ;

                // 开启接口注释
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "NGAdmin.Net.WebApi.xml");
                c.IncludeXmlComments(xmlPath, true);

                var xmlCommonPath = Path.Combine(AppContext.BaseDirectory, "NGAdmin.Net.Common.xml");
                c.IncludeXmlComments(xmlCommonPath);

                var xmlModelPath = Path.Combine(AppContext.BaseDirectory, "NGAdmin.Net.Entity.xml");
                c.IncludeXmlComments(xmlModelPath);

                if (NGAdminGlobalContext.JwtSettings.IsEnabled)
                {
                    // header添加token
                    c.OperationFilter<SecurityRequirementsOperationFilter>();

                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "JWT授权，在下框中输入Bearer token",//注意两者之间是一个空格
                        Name = "Authorization",//jwt默认的参数名称
                        In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                        Type = SecuritySchemeType.ApiKey
                    });
                }
            });




            #endregion

            #region 注册系统全局跨域

            services.AddCors(options =>
            {
                options.AddPolicy("cors", builder =>
                {
                    builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });

            #endregion

            #region 初始化系统全局配置
            InitConfiguration();

            //全局静态配置热更新
            ChangeToken.OnChange(() => Configuration.GetReloadToken(), () =>
            {
                InitConfiguration();
            });

            #endregion     

            #region 注册系统全局Jwt认证

            if (NGAdminGlobalContext.JwtSettings.IsEnabled)
            {
                JwtSettings jwtSettings = new JwtSettings();
                services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
                Configuration.GetSection("JwtSettings").Bind(jwtSettings);

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).
                AddJwtBearer(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("act", "expired");
                            }
                            return Task.CompletedTask;
                        }
                    };

                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            }
            #endregion

            #region 注册系统全局并发策略

            services.AddQueuePolicy(options =>
            {
                //最大并发请求数
                options.MaxConcurrentRequests = NGAdminGlobalContext.ConcurrencyLimiterConfig.MaxConcurrentRequests;
                //请求队列长度限制
                options.RequestQueueLimit = NGAdminGlobalContext.ConcurrencyLimiterConfig.RequestQueueLimit;
            });

            #endregion

            #region 注册系统全局过滤器

            services.AddMvc(options =>
            {
                options.Filters.Add<ActionAuthorization>(); // 添加身份验证过滤器
            });


          //  services.AddSingleton<PermissionAttribute>(); // 添加权限验证过滤器

            #endregion

            #region 注册系统AutoMapper组件

            services.AddAutoMapper(typeof(AutoMapperProfile));

            #endregion


            #region 注册MiniProfiler性能分析组件

            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = NGAdminGlobalContext.MiniProfilerConfig.RouteBasePath;

                (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(NGAdminGlobalContext.MiniProfilerConfig.CacheDuration);

                options.ColorScheme = StackExchange.Profiling.ColorScheme.Dark;

            }).AddEntityFramework();//显示SQL语句及耗时;

            #endregion

            #region 注册Cap分布式事务组件

            if (NGAdminGlobalContext.CapConfig.IsEnabled)
            {
                services.AddCap(x =>
                {
                    //默认组名
                    x.DefaultGroupName = NGAdminGlobalContext.CapConfig.DefaultGroupName;

                    #region 注册数据库实例

                    //使用MySql
                    x.UseMySql(opt =>
                    {
                        opt.ConnectionString = NGAdminGlobalContext.DBConfig.ConnectionString;
                        opt.TableNamePrefix = NGAdminGlobalContext.CapConfig.TableNamePrefix;
                    });



                    #endregion

                    #region 注册MQ中间件

                    //使用Redis
                    x.UseRedis(NGAdminGlobalContext.RedisConfig.ConnectionString);

                    #endregion

                    #region 注册服务发现

                    if (NGAdminGlobalContext.ConsulConfig.IsEnabled)
                    {
                        // 注册 Dashboard
                        x.UseDashboard();

                        // 注册节点到 Consul
                        x.UseDiscovery(d =>
                        {
                            d.DiscoveryServerHostName = NGAdminGlobalContext.ConsulConfig.ConsulHostIP;
                            d.DiscoveryServerPort = NGAdminGlobalContext.ConsulConfig.ConsulHostPort;
                            d.CurrentNodeHostName = NGAdminGlobalContext.CapConfig.CurrentNodeHostName;
                            d.CurrentNodePort = NGAdminGlobalContext.ConsulConfig.ServicePort;
                            d.NodeId = NGAdminGlobalContext.CapConfig.NodeId;
                            d.NodeName = NGAdminGlobalContext.CapConfig.NodeName;
                        });
                    }

                    #endregion
                });
            }

            #endregion

            #region 支持ASP.NET CORE内存缓存组件

            services.AddMemoryCache();

            #endregion


            #region 注册LazyCaptcha组件及参数设置

            services.AddCaptcha(Configuration, option =>
            {
                option.CaptchaType = CaptchaType.ARITHMETIC_ZH; // 验证码类型
                option.CodeLength = 2; // 验证码长度, 要放在CaptchaType设置后.  当类型为算术表达式时，长度代表操作的个数
                option.ExpirySeconds = 1; // 验证码过期时间
                option.IgnoreCase = true; // 比较时是否忽略大小写
                option.StoreageKeyPrefix = ""; // 存储键前缀

                option.ImageOption.Animation = true; // 是否启用动画
                option.ImageOption.FrameDelay = 30; // 每帧延迟,Animation=true时有效, 默认30

                option.ImageOption.Width = 150; // 验证码宽度
                option.ImageOption.Height = 50; // 验证码高度
                option.ImageOption.BackgroundColor = SixLabors.ImageSharp.Color.White; // 验证码背景色

                option.ImageOption.BubbleCount = 4; // 气泡数量
                option.ImageOption.BubbleMinRadius = 5; // 气泡最小半径
                option.ImageOption.BubbleMaxRadius = 15; // 气泡最大半径
                option.ImageOption.BubbleThickness = 1; // 气泡边沿厚度

                option.ImageOption.InterferenceLineCount = 3; // 干扰线数量

                option.ImageOption.FontSize = 36; // 字体大小
                option.ImageOption.FontFamily = DefaultFontFamilys.Instance.Kaiti; // 字体
            });

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime aft)
        {

            #region POST、PUT添加Body参数

            app.Use(async (context, next) =>
            {
                if (context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase) ||
                context.Request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
                {
                    context.Request.EnableBuffering();
                    using (var reader = new StreamReader(context.Request.Body, encoding: Encoding.UTF8
                        , detectEncodingFromByteOrderMarks: false, leaveOpen: true))
                    {
                        var body = await reader.ReadToEndAsync();
                        context.Items.Add("body", body);
                        context.Request.Body.Position = 0;
                    }
                }
                await next.Invoke();
            });

            #endregion



            #region 启用系统全局启停事件

            aft.ApplicationStarted.Register(async () =>
            {
                Console.WriteLine("Application Started");

                //自动构建数据库
                await NGAdminApplication.BuildDatabase();

                //加载系统级别缓存
                await NGAdminApplication.LoadSystemCache(app);

                //启动业务作业
                await NGAdminApplication.StartScheduleJobAsync(app);
            });

            #endregion

            aft.ApplicationStopped.Register(() =>
            {
                Console.WriteLine("Application Stopped");
            });

            aft.ApplicationStopping.Register(async () =>
            {
                Console.WriteLine("Application Stopping");

                //清理系统缓存
                await NGAdminApplication.ClearSystemCache(app);
            });


            #region 启用并发限制数中间件

            app.UseConcurrencyLimiter();

            #endregion

            #region 启用系统Jwt认证中间件

            if (NGAdminGlobalContext.JwtSettings.IsEnabled)
            {
                //认证中间件
                app.UseAuthentication();
            }

            #endregion


            #region 启用系统Swagger组件

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NGAdmin.Net.WebAPI v1"));
            }
            else if (env.IsProduction() && NGAdminGlobalContext.SystemConfig.SupportSwaggerOnProduction)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NGAdmin.Net.WebAPI v1"));
            }

            #endregion


            #region 系统全局默认启用项目

            app.UseStaticFiles();

            app.UseRouting();

            //启用系统全局跨域
            app.UseCors("cors");

            app.UseAuthorization();

            //启用AspNetCoreRateLimit组件
            app.UseIpRateLimiting();

            //启用全局统一异常处理
            app.UseMiddleware<GlobalExceptionHandlerExtensions>();

            //解决Ubuntu下Nginx代理不能获取IP问题
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            //启用系统全局服务治理
            if (NGAdminGlobalContext.ConsulConfig.IsEnabled)
            {
                app.RegisterConsul(aft);
            }

            //MiniProfiler性能分析组件
            app.UseMiniProfiler();

            app.UseEndpoints(endpoints =>
            {
                if (NGAdminGlobalContext.SignalRConfig.IsEnabled)
                {
                    endpoints.MapHub<ChatHub>(NGAdminGlobalContext.SignalRConfig.Pattern);
                }

                endpoints.MapControllers();
            });

            #endregion     
        }



        #region 初始化全局静态配置
        /// <summary>
        /// 初始化全局静态配置
        /// </summary>
        private void InitConfiguration()
        {
            NGAdminGlobalContext.DBConfig = Configuration.GetSection("DBConfig").Get<DBConfig>();
            NGAdminGlobalContext.SystemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
            NGAdminGlobalContext.Configuration = Configuration;
            NGAdminGlobalContext.RedisConfig = Configuration.GetSection("RedisConfig").Get<RedisConfig>();     
            NGAdminGlobalContext.DirectoryConfig = Configuration.GetSection("DirectoryConfig").Get<DirectoryConfig>();       
            NGAdminGlobalContext.JwtSettings = Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            NGAdminGlobalContext.ConcurrencyLimiterConfig = Configuration.GetSection("ConcurrencyLimiterConfig").Get<ConcurrencyLimiterConfig>();
            NGAdminGlobalContext.CodeGeneratorConfig = Configuration.GetSection("CodeGeneratorConfig").Get<CodeGeneratorConfig>();
            NGAdminGlobalContext.LogConfig = Configuration.GetSection("LogConfig").Get<LogConfig>();
            NGAdminGlobalContext.ActiveMQConfig = Configuration.GetSection("ActiveMQConfig").Get<ActiveMQConfig>();
            NGAdminGlobalContext.MiniProfilerConfig = Configuration.GetSection("MiniProfilerConfig").Get<MiniProfilerConfig>();
            NGAdminGlobalContext.CapConfig = Configuration.GetSection("CapConfig").Get<CapConfig>();
            NGAdminGlobalContext.ConsulConfig = Configuration.GetSection("ConsulConfig").Get<ConsulConfig>();
            NGAdminGlobalContext.SignalRConfig = Configuration.GetSection("SignalRConfig").Get<SignalRConfig>();
            NGAdminGlobalContext.QuartzConfig = Configuration.GetSection("QuartzConfig").Get<QuartzConfig>();
            NGAdminGlobalContext.RateLimitConfig = Configuration.GetSection("RateLimitConfig").Get<RateLimitConfig>();
            NGAdminGlobalContext.SystemCacheConfig = Configuration.GetSection("SystemCacheConfig").Get<SystemCacheConfig>();
            NGAdminGlobalContext.MailConfig = Configuration.GetSection("MailConfig").Get<MailConfig>();
        }

        #endregion
    }
}
