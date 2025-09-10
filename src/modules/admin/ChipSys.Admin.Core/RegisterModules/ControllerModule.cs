using Autofac;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Module = Autofac.Module;

namespace ChipSys.Admin.Core.RegisterModules;

public class ControllerModule : Module
{
    /// <summary>
    /// ������ע��
    /// </summary>
    public ControllerModule()
    {
    }

    protected override void Load(ContainerBuilder builder)
    {
        var controllerTypes = Assembly.GetExecutingAssembly().GetExportedTypes()
        .Where(a => typeof(ControllerBase).IsAssignableFrom(a) && !a.IsAbstract && !a.IsInterface && a.IsPublic)
        .ToArray();

        // �������п�������֧������ע��
        builder.RegisterTypes(controllerTypes).PropertiesAutowired();
    }
}
