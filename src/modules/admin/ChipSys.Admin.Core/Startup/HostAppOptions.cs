using Autofac;
using FreeSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using Yitter.IdGenerator;
using ChipSys.Admin.Core.Configs;
using ChipSys.DynamicApi;

namespace ChipSys.Admin.Core.Startup;

/// <summary>
/// ����Ӧ������
/// </summary>
public class HostAppOptions
{
    /// <summary>
    /// ����ǰ��Ӧ�ó��򹹽���
    /// </summary>
    public Action<WebApplicationBuilder> ConfigurePreWebApplicationBuilder { get; set; }

    /// <summary>
    /// ����Ӧ�ó��򹹽���
    /// </summary>
    public Action<WebApplicationBuilder> ConfigureWebApplicationBuilder { get; set; }

    /// <summary>
    /// ����ǰ�÷���
    /// </summary>
    public Action<HostAppContext> ConfigurePreServices { get; set; }

    /// <summary>
    /// ���÷���
    /// </summary>
    public Action<HostAppContext> ConfigureServices { get; set; }

    /// <summary>
    /// ���ú��÷���
    /// </summary>
    public Action<HostAppContext> ConfigurePostServices { get; set; }

    /// <summary>
    /// ����mvc������
    /// </summary>
    public Action<IMvcBuilder, HostAppContext> ConfigureMvcBuilder { get; set; }

    /// <summary>
    /// ����Autofac����
    /// </summary>
    public Action<ContainerBuilder, HostAppContext> ConfigureAutofacContainer { get; set; }

    /// <summary>
    /// ����ǰ���м��
    /// </summary>
    public Action<HostAppMiddlewareContext> ConfigurePreMiddleware { get; set; }

    /// <summary>
    /// �����м��
    /// </summary>
    public Action<HostAppMiddlewareContext> ConfigureMiddleware { get; set; }

    /// <summary>
    /// ���ú����м��
    /// </summary>
    public Action<HostAppMiddlewareContext> ConfigurePostMiddleware { get; set; }

    /// <summary>
    /// ����FreeSql������
    /// </summary>
    public Action<FreeSqlBuilder, DbConfig> ConfigureFreeSqlBuilder { get; set; }

    /// <summary>
    /// ����FreeSqlͬ���ṹ
    /// </summary>
    public Action<IFreeSql, DbConfig> ConfigureFreeSqlSyncStructure { get; set; }

    /// <summary>
    /// ǰ������FreeSql
    /// </summary>
    public Action<IFreeSql, DbConfig> ConfigurePreFreeSql { get; set; }

    /// <summary>
    /// ����FreeSql
    /// </summary>
    public Action<IFreeSql, DbConfig> ConfigureFreeSql { get; set; }

    
    /// <summary>
    /// ���ö�̬Api
    /// </summary>
    public Action<DynamicApiOptions> ConfigureDynamicApi { get; set; }

    /// <summary>
    /// ����SwaggerUI
    /// </summary>
    public Action<SwaggerUIOptions> ConfigureSwaggerUI { get; set; }

    /// <summary>
    /// ����ѩ��Ư���㷨
    /// </summary>
    public Action<IdGeneratorOptions> ConfigureIdGenerator { get; set; }

    /// <summary>
    /// �Զ������ݿ��ʼ��
    /// </summary>
    public bool CustomInitDb { get; set; } = false;

}
