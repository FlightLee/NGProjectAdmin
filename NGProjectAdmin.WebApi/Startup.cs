using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using NGProjectAdmin.Common.Global;

namespace NGProjectAdmin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            #region 初始化系统全局配置

            InitConfiguration();

            //全局静态配置热更新
            ChangeToken.OnChange(() => Configuration.GetReloadToken(), () =>
            {
                InitConfiguration();
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// 初始化全局静态配置
        /// </summary>
        private void InitConfiguration()
        {
            NGAdminGlobalContext.DBConfig = Configuration.GetSection("DBConfig").Get<DBConfig>();
            //NGAdminGlobalContext.SystemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
        }
    }
}
