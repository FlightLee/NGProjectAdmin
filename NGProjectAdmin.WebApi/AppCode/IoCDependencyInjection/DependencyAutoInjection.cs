//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using Autofac;
using System.Linq;
using System.Reflection;

namespace NGProjectAdmin.WebApi.AppCode.IoCDependencyInjection
{
    /// <summary>
    /// 依赖自动注入
    /// </summary>
    public class DependencyAutoInjection : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //业务逻辑层程序集
            Assembly service = Assembly.Load("NGProjectAdmin.Service");
            //数据库访问层程序集
            Assembly repository = Assembly.Load("NGProjectAdmin.Repository");

            //自动注入
            builder.RegisterAssemblyTypes(service).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();
            //自动注入
            builder.RegisterAssemblyTypes(repository).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces();
        }
    }
}
