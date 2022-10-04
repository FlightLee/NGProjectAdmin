using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using NGProjectAdmin.Common.Global;
using System;
using System.Linq;

namespace NGProjectAdmin.WebApi.AppCode.FrameworkExtensions
{
    public static class RegisterConsulExtensions
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            var IP = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).
                AddressList.
                FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.
                ToString();

            //请求注册的 Consul 地址
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{NGAdminGlobalContext.ConsulConfig.ConsulHostIP}:{NGAdminGlobalContext.ConsulConfig.ConsulHostPort}"));
            var httpCheck = new AgentServiceCheck()
            {
                //服务启动多久后注册
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(NGAdminGlobalContext.ConsulConfig.DeregisterCriticalServiceAfter),
                //健康检查时间间隔，或者称为心跳间隔
                Interval = TimeSpan.FromSeconds(NGAdminGlobalContext.ConsulConfig.Interval),
                //健康检查地址
                HTTP = $"http://{IP}:{NGAdminGlobalContext.ConsulConfig.ServicePort}/API/RuYiAdminHealth/Get",
                Timeout = TimeSpan.FromSeconds(NGAdminGlobalContext.ConsulConfig.Timeout)
            };

            var ID = Guid.NewGuid().ToString();
            // Register service with consul
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = ID,
                Name = NGAdminGlobalContext.ConsulConfig.ServiceName + $"({ID})",
                Address = IP,
                Port = NGAdminGlobalContext.ConsulConfig.ServicePort,
                //添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
                Tags = new[] { $"urlprefix-/{NGAdminGlobalContext.ConsulConfig.ServiceName}" }
            };

            //服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
            consulClient.Agent.ServiceRegister(registration).Wait();
            lifetime.ApplicationStopping.Register(() =>
            {
                //服务停止时取消注册
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });

            return app;
        }
    }
}
