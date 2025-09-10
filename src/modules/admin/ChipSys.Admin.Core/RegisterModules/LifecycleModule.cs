using Autofac;
using System.Reflection;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Configs;
using ChipSys.Common.Helpers;
using Module = Autofac.Module;

namespace ChipSys.Admin.Core.RegisterModules;

/// <summary>
/// ��������ע��
/// </summary>
public class LifecycleModule : Module
{
    private readonly AppConfig _appConfig;

    public LifecycleModule(AppConfig appConfig)
    {
        _appConfig = appConfig;
    }

    protected override void Load(ContainerBuilder builder)
    {
        // ���Ҫע��ĳ���
        Assembly[] assemblies = null;
        if (_appConfig.AssemblyNames?.Length > 0)
        {
            assemblies = AssemblyHelper.GetAssemblyList(_appConfig.AssemblyNames);
        }

        if (!(assemblies?.Length > 0))
        {
            return;
        }

        //�޽ӿ�ע�뵥��
        builder.RegisterAssemblyTypes(assemblies)
        .Where(t => t.GetCustomAttribute<InjectSingletonAttribute>(false) != null)
        .SingleInstance()
        .PropertiesAutowired();

        //�нӿ�ע�뵥��
        builder.RegisterAssemblyTypes(assemblies)
        .Where(t => t.GetCustomAttribute<InjectSingletonAttribute>(false) != null)
        .AsImplementedInterfaces()
        .SingleInstance()
        .PropertiesAutowired();

        //�޽ӿ�ע��������
        builder.RegisterAssemblyTypes(assemblies)
        .Where(t => t.GetCustomAttribute<InjectScopedAttribute>(false) != null)
        .InstancePerLifetimeScope()
        .PropertiesAutowired();

        //�нӿ�ע��������
        builder.RegisterAssemblyTypes(assemblies)
        .Where(t => t.GetCustomAttribute<InjectScopedAttribute>(false) != null)
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope()
        .PropertiesAutowired();

        //�޽ӿ�ע��˲ʱ
        builder.RegisterAssemblyTypes(assemblies)
        .Where(t => t.GetCustomAttribute<InjectTransientAttribute>(false) != null)
        .InstancePerDependency()
        .PropertiesAutowired();

        //�нӿ�ע��˲ʱ
        builder.RegisterAssemblyTypes(assemblies)
        .Where(t => t.GetCustomAttribute<InjectTransientAttribute>(false) != null)
        .AsImplementedInterfaces()
        .InstancePerDependency()
        .PropertiesAutowired();
    }
}
