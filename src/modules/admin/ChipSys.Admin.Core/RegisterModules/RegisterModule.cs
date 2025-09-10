using Autofac;
using Autofac.Extras.DynamicProxy;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Module = Autofac.Module;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Configs;
using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Core.Repositories;
using ChipSys.Common.Helpers;

namespace ChipSys.Admin.Core.RegisterModules;

public class RegisterModule : Module
{
    private readonly AppConfig _appConfig;

    /// <summary>
    /// ģ��ע��
    /// </summary>
    /// <param name="appConfig">AppConfig</param>
    public RegisterModule(AppConfig appConfig)
    {
        _appConfig = appConfig;
    }

    protected override void Load(ContainerBuilder builder)
    {
        //��������
        var interceptorServiceTypes = new List<Type>();
        if (_appConfig.Aop.Transaction)
        {
            builder.RegisterType<TransactionInterceptor>();
            builder.RegisterType<TransactionAsyncInterceptor>();
            interceptorServiceTypes.Add(typeof(TransactionInterceptor));
        }

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

        static bool Predicate(Type a) => !a.IsDefined(typeof(NonRegisterIOCAttribute), true) 
            && (a.Name.EndsWith("Service") || a.Name.EndsWith("Repository") || typeof(IRegisterIOC).IsAssignableFrom(a)) 
            && !a.IsAbstract && !a.IsInterface && a.IsPublic;

        //�нӿ�ʵ��
        builder.RegisterAssemblyTypes(assemblies)
        .Where(new Func<Type, bool>(Predicate))
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope()
        .PropertiesAutowired()// ����ע��
        .InterceptedBy(interceptorServiceTypes.ToArray())
        .EnableInterfaceInterceptors();

        //�޽ӿ�ʵ��
        builder.RegisterAssemblyTypes(assemblies)
        .Where(new Func<Type, bool>(Predicate))
        .InstancePerLifetimeScope()
        .PropertiesAutowired()// ����ע��
        .InterceptedBy(interceptorServiceTypes.ToArray())
        .EnableClassInterceptors();

        //�����ϣ����ע��
        builder.RegisterGeneric(typeof(PasswordHasher<>)).As(typeof(IPasswordHasher<>)).SingleInstance().PropertiesAutowired();

        //�ִ�����ע��
        builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepositoryBase<>)).InstancePerLifetimeScope().PropertiesAutowired();
        builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepositoryBase<,>)).InstancePerLifetimeScope().PropertiesAutowired();
    }
}
