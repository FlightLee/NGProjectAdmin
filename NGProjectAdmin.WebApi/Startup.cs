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
            #region ע��IoC���Ʒ�ת

            builder.RegisterModule<DependencyAutoInjection>();

            #endregion
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region ע��AspNetCoreRateLimit���

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = NGAdminGlobalContext.RedisConfig.ConnectionString;
                options.InstanceName = NGAdminGlobalContext.RateLimitConfig.InstanceName;
            });

            //��������
            services.AddOptions();

            //������Ӧ����
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            //����Ip����
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));

            //ע��������͹���洢
            services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();

            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            services.AddControllers();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //���ã���������Կ��������
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            #endregion

            #region ע��ϵͳSwagger���
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NGAdmin ASP.NET Core WebApi Documentation",
                    Description = "NGAdmin�ӿ�<br/>",
                    Version = "V-1.0"
                }); ;

                // �����ӿ�ע��
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "NGAdmin.Net.WebApi.xml");
                c.IncludeXmlComments(xmlPath, true);

                var xmlCommonPath = Path.Combine(AppContext.BaseDirectory, "NGAdmin.Net.Common.xml");
                c.IncludeXmlComments(xmlCommonPath);

                var xmlModelPath = Path.Combine(AppContext.BaseDirectory, "NGAdmin.Net.Entity.xml");
                c.IncludeXmlComments(xmlModelPath);

                if (NGAdminGlobalContext.JwtSettings.IsEnabled)
                {
                    // header���token
                    c.OperationFilter<SecurityRequirementsOperationFilter>();

                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "JWT��Ȩ�����¿�������Bearer token",//ע������֮����һ���ո�
                        Name = "Authorization",//jwtĬ�ϵĲ�������
                        In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                        Type = SecuritySchemeType.ApiKey
                    });
                }
            });




            #endregion

            #region ע��ϵͳȫ�ֿ���

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

            #region ��ʼ��ϵͳȫ������
            InitConfiguration();

            //ȫ�־�̬�����ȸ���
            ChangeToken.OnChange(() => Configuration.GetReloadToken(), () =>
            {
                InitConfiguration();
            });

            #endregion     

            #region ע��ϵͳȫ��Jwt��֤

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

            #region ע��ϵͳȫ�ֲ�������

            services.AddQueuePolicy(options =>
            {
                //��󲢷�������
                options.MaxConcurrentRequests = NGAdminGlobalContext.ConcurrencyLimiterConfig.MaxConcurrentRequests;
                //������г�������
                options.RequestQueueLimit = NGAdminGlobalContext.ConcurrencyLimiterConfig.RequestQueueLimit;
            });

            #endregion

            #region ע��ϵͳȫ�ֹ�����

            services.AddMvc(options =>
            {
                options.Filters.Add<ActionAuthorization>(); // ��������֤������
            });


          //  services.AddSingleton<PermissionAttribute>(); // ���Ȩ����֤������

            #endregion

            #region ע��ϵͳAutoMapper���

            services.AddAutoMapper(typeof(AutoMapperProfile));

            #endregion


            #region ע��MiniProfiler���ܷ������

            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = NGAdminGlobalContext.MiniProfilerConfig.RouteBasePath;

                (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(NGAdminGlobalContext.MiniProfilerConfig.CacheDuration);

                options.ColorScheme = StackExchange.Profiling.ColorScheme.Dark;

            }).AddEntityFramework();//��ʾSQL��估��ʱ;

            #endregion

            #region ע��Cap�ֲ�ʽ�������

            if (NGAdminGlobalContext.CapConfig.IsEnabled)
            {
                services.AddCap(x =>
                {
                    //Ĭ������
                    x.DefaultGroupName = NGAdminGlobalContext.CapConfig.DefaultGroupName;

                    #region ע�����ݿ�ʵ��

                    //ʹ��MySql
                    x.UseMySql(opt =>
                    {
                        opt.ConnectionString = NGAdminGlobalContext.DBConfig.ConnectionString;
                        opt.TableNamePrefix = NGAdminGlobalContext.CapConfig.TableNamePrefix;
                    });



                    #endregion

                    #region ע��MQ�м��

                    //ʹ��Redis
                    x.UseRedis(NGAdminGlobalContext.RedisConfig.ConnectionString);

                    #endregion

                    #region ע�������

                    if (NGAdminGlobalContext.ConsulConfig.IsEnabled)
                    {
                        // ע�� Dashboard
                        x.UseDashboard();

                        // ע��ڵ㵽 Consul
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

            #region ֧��ASP.NET CORE�ڴ滺�����

            services.AddMemoryCache();

            #endregion


            #region ע��LazyCaptcha�������������

            services.AddCaptcha(Configuration, option =>
            {
                option.CaptchaType = CaptchaType.ARITHMETIC_ZH; // ��֤������
                option.CodeLength = 2; // ��֤�볤��, Ҫ����CaptchaType���ú�.  ������Ϊ�������ʽʱ�����ȴ�������ĸ���
                option.ExpirySeconds = 1; // ��֤�����ʱ��
                option.IgnoreCase = true; // �Ƚ�ʱ�Ƿ���Դ�Сд
                option.StoreageKeyPrefix = ""; // �洢��ǰ׺

                option.ImageOption.Animation = true; // �Ƿ����ö���
                option.ImageOption.FrameDelay = 30; // ÿ֡�ӳ�,Animation=trueʱ��Ч, Ĭ��30

                option.ImageOption.Width = 150; // ��֤����
                option.ImageOption.Height = 50; // ��֤��߶�
                option.ImageOption.BackgroundColor = SixLabors.ImageSharp.Color.White; // ��֤�뱳��ɫ

                option.ImageOption.BubbleCount = 4; // ��������
                option.ImageOption.BubbleMinRadius = 5; // ������С�뾶
                option.ImageOption.BubbleMaxRadius = 15; // �������뾶
                option.ImageOption.BubbleThickness = 1; // ���ݱ��غ��

                option.ImageOption.InterferenceLineCount = 3; // ����������

                option.ImageOption.FontSize = 36; // �����С
                option.ImageOption.FontFamily = DefaultFontFamilys.Instance.Kaiti; // ����
            });

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime aft)
        {

            #region POST��PUT���Body����

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



            #region ����ϵͳȫ����ͣ�¼�

            aft.ApplicationStarted.Register(async () =>
            {
                Console.WriteLine("Application Started");

                //�Զ��������ݿ�
                await NGAdminApplication.BuildDatabase();

                //����ϵͳ���𻺴�
                await NGAdminApplication.LoadSystemCache(app);

                //����ҵ����ҵ
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

                //����ϵͳ����
                await NGAdminApplication.ClearSystemCache(app);
            });


            #region ���ò����������м��

            app.UseConcurrencyLimiter();

            #endregion

            #region ����ϵͳJwt��֤�м��

            if (NGAdminGlobalContext.JwtSettings.IsEnabled)
            {
                //��֤�м��
                app.UseAuthentication();
            }

            #endregion


            #region ����ϵͳSwagger���

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


            #region ϵͳȫ��Ĭ��������Ŀ

            app.UseStaticFiles();

            app.UseRouting();

            //����ϵͳȫ�ֿ���
            app.UseCors("cors");

            app.UseAuthorization();

            //����AspNetCoreRateLimit���
            app.UseIpRateLimiting();

            //����ȫ��ͳһ�쳣����
            app.UseMiddleware<GlobalExceptionHandlerExtensions>();

            //���Ubuntu��Nginx�����ܻ�ȡIP����
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            //����ϵͳȫ�ַ�������
            if (NGAdminGlobalContext.ConsulConfig.IsEnabled)
            {
                app.RegisterConsul(aft);
            }

            //MiniProfiler���ܷ������
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



        #region ��ʼ��ȫ�־�̬����
        /// <summary>
        /// ��ʼ��ȫ�־�̬����
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
