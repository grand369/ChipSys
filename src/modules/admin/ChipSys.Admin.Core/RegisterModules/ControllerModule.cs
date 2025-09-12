using Autofac;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Module = Autofac.Module;

namespace ChipSys.Admin.Core.RegisterModules;

public class ControllerModule : Module
{
    /// <summary>
    /// 控制器注册
    /// </summary>
    public ControllerModule()
    {
    }

    protected override void Load(ContainerBuilder builder)
    {
        var controllerTypes = Assembly.GetExecutingAssembly().GetExportedTypes()
        .Where(a => typeof(ControllerBase).IsAssignableFrom(a) && !a.IsAbstract && !a.IsInterface && a.IsPublic)
        .ToArray();

        // 注册所有控制器支持属性注入
        builder.RegisterTypes(controllerTypes).PropertiesAutowired();
    }
}