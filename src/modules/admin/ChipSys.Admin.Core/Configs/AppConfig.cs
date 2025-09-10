using System.Text.RegularExpressions;
using Yitter.IdGenerator;

namespace ChipSys.Admin.Core.Configs;

/// <summary>
/// Ӧ������
/// </summary>
public class AppConfig
{
    public AppType AppType { get; set; } = AppType.Controllers;

    /// <summary>
    /// Api��ַ
    /// </summary>
    public string[] Urls { get; set; }

    /// <summary>
    /// �����ַ
    /// </summary>
    public string[] CorUrls { get; set; }

    private string[] _assemblyNames;

    /// <summary>
    /// ��������
    /// </summary>
    public string[] AssemblyNames
    {
        get => _assemblyNames;
        set
        {
            _assemblyNames = value;

            if (value.Contains("ChipSys.Admin"))
            {
                if (!value.Contains("ChipSys.Admin.Contracts"))
                {
                    _assemblyNames = [.. _assemblyNames, "ChipSys.Admin.Contracts"];
                }
            }

            if (!value.Contains("ChipSys.Admin.Core"))
            {
                _assemblyNames = [.. _assemblyNames, "ChipSys.Admin.Core"];
            }
        }
    }

    private string[] _enumListAssemblyNames;

    /// <summary>
    /// ö���б��������
    /// </summary>
    public string[] EnumListAssemblyNames
    {
        get => _enumListAssemblyNames;
        set
        {
            _enumListAssemblyNames = value;
            if (value.Contains("ChipSys.Admin"))
            {
                if (!value.Contains("ChipSys.Admin.Contracts"))
                {
                    _enumListAssemblyNames = [.. _enumListAssemblyNames, "ChipSys.Admin.Contracts"];
                }

                if (!value.Contains("ChipSys.Admin.Core"))
                {
                    _enumListAssemblyNames = [.. _enumListAssemblyNames, "ChipSys.Admin.Core"];
                }
            }
        }
    }

    /// <summary>
    /// �⻧����
    /// </summary>
    public bool Tenant { get; set; } = false;

    /// <summary>
    /// �ֲ�ʽ����Ψһ��ʶ
    /// </summary>
    public string DistributeKey { get; set; }

    /// <summary>
    /// Swagger�ĵ�
    /// </summary>
    public SwaggerConfig Swagger { get; set; } = new SwaggerConfig();

    /// <summary>
    /// �°�Api�ĵ�
    /// </summary>
    public ApiUIConfig ApiUI { get; set; } = new ApiUIConfig();

    /// <summary>
    /// MiniProfiler���ܷ�����
    /// </summary>
    public bool MiniProfiler { get; set; } = false;

    /// <summary>
    /// ͳһ��֤��Ȩ������
    /// </summary>
    public IdentityServer IdentityServer { get; set; } = new IdentityServer();

    /// <summary>
    /// Aop����
    /// </summary>
    public AopConfig Aop { get; set; } = new AopConfig();

    /// <summary>
    /// ��־����
    /// </summary>
    public LogConfig Log { get; set; } = new LogConfig();

    /// <summary>
    /// ��֤����
    /// </summary>
    public ValidateConfig Validate { get; set; } = new ValidateConfig();

    /// <summary>
    /// ����
    /// </summary>
    public bool RateLimit { get; set; } = false;

    /// <summary>
    /// ��֤������
    /// </summary>
    public VarifyCodeConfig VarifyCode { get; set; } = new VarifyCodeConfig();

    /// <summary>
    /// Ĭ������
    /// </summary>
    public string DefaultPassword { get; set; } = "123asd";

    /// <summary>
    /// ��̬Api����
    /// </summary>
    public DynamicApiConfig DynamicApi { get; set; } = new DynamicApiConfig();

    /// <summary>
    /// ʵ�ֱ�׼��ʶ�����ϣ
    /// </summary>
    public bool PasswordHasher { get; set; } = false;

    /// <summary>
    /// ��������С
    /// </summary>
    [Obsolete("��ʹ�� Kestrel: { MaxRequestBodySize: 104857600 }����")]
    public long? MaxRequestBodySize { get; set; } = 104857600;

    /// <summary>
    /// Kestrel������
    /// </summary>
    public KestrelConfig Kestrel { get; set; } = new KestrelConfig();

    /// <summary>
    /// �����������
    /// </summary>
    public HealthChecksConfig HealthChecks { get; set; } = new HealthChecksConfig();

    /// <summary>
    /// ָ���������ʱԤ��ȴ�ʱ�䣬����Ϊ��λ��Ĭ��30����
    /// </summary>
    public int PreflightMaxAge { get; set; }

    /// <summary>
    /// ������ȹ����������
    /// </summary>
    public TaskSchedulerUIConfig TaskSchedulerUI { get; set; } = new TaskSchedulerUIConfig();

    /// <summary>
    /// Id����������
    /// </summary>
    public IdGeneratorConfig IdGenerator { get; set; } = new IdGeneratorConfig();

    /// <summary>
    /// ��������
    /// </summary>
    public LangConfig Lang { get; set; } = new LangConfig();

    /// <summary>
    /// IP��ַ��λ����
    /// </summary>
    public IP2RegionConfig IP2Region { get; set; } = new IP2RegionConfig();
}

/// <summary>
/// Kestrel����������
/// </summary>
public class KestrelConfig
{
    /// <summary>
    /// HTTP���ӱ����ʱ�䣬��λ��
    /// </summary>
    public double KeepAliveTimeout { get; set; } = 130;

    /// <summary>
    /// ��������ͷ�ʱ�䣬��λ��
    /// </summary>
    public double RequestHeadersTimeout { get; set; } = 30;

    /// <summary>
    /// ��������С����λbytes
    /// </summary>
    public long? MaxRequestBodySize { get; set; } = 30000000;
}

/// <summary>
/// ��������
/// </summary>
public class LangConfig
{
    /// <summary>
    /// ����Json����
    /// </summary>
    public bool EnableJson { get; set; } = true;

    /// <summary>
    /// Ĭ������
    /// </summary>
    public string DefaultLang { get; set; } = "zh";

    /// <summary>
    /// �����б�
    /// </summary>
    public string[] Langs { get; set; }

    /// <summary>
    /// ������������б�
    /// </summary>
    public string[] RequestCultureProviders { get; set; }
}

/// <summary>
/// IP��ַ��λ����
/// </summary>
public class IP2RegionConfig
{
    /// <summary>
    /// ����
    /// </summary>
    public bool Enable { get; set; } = false;

    /// <summary>
    /// ���ݿ�·��
    /// </summary>
    public string DbPath { get; set; }
}

/// <summary>
/// Swagger����
/// </summary>
public class SwaggerConfig
{
    /// <summary>
    /// ����
    /// </summary>
    public bool Enable { get; set; } = false;

    /// <summary>
    /// ����ö�ټܹ�������
    /// </summary>
    public bool EnableEnumSchemaFilter { get; set; } = true;

    /// <summary>
    /// ���ýӿ������ĵ�������
    /// </summary>
    public bool EnableOrderTagsDocumentFilter { get; set; } = true;

    /// <summary>
    /// ����ö��������
    /// </summary>
    public bool EnableJsonStringEnumConverter { get; set; } = false;

    /// <summary>
    /// ����SchemaId�����ռ�
    /// </summary>
    public bool EnableSchemaIdNamespace { get; set; } = false;

    /// <summary>
    /// �����б�
    /// </summary>
    public string[] AssemblyNameList { get; set; }

    private string _RoutePrefix = "swagger";
    /// <summary>
    /// ���ʵ�ַ
    /// </summary>
    public string RoutePrefix { get => Regex.Replace(_RoutePrefix, "^\\/+|\\/+$", ""); set => _RoutePrefix = value; }

    /// <summary>
    /// ��ַ
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// ��Ŀ�б�
    /// </summary>
    public List<ProjectConfig> Projects { get; set; }

    /// <summary>
    /// �����Զ�ͬ��
    /// </summary>
    public bool EnableAutoSync { get; set; }
}

/// <summary>
///�°�Api�ĵ�����
/// </summary>
public class ApiUIConfig
{
    /// <summary>
    /// ����
    /// </summary>
    public bool Enable { get; set; } = false;


    private string _RoutePrefix="";
    /// <summary>
    /// ���ʵ�ַ
    /// </summary>
    public string RoutePrefix { get => Regex.Replace(_RoutePrefix, "^\\/+|\\/+$", ""); set => _RoutePrefix = value; }

    public SwaggerFooterConfig Footer { get; set; } = new SwaggerFooterConfig();
}

/// <summary>
/// Swaggerҳ������
/// </summary>
public class SwaggerFooterConfig
{
    /// <summary>
    /// ����
    /// </summary>
    public bool Enable { get; set; } = false;

    /// <summary>
    /// ����
    /// </summary>
    public string Content { get; set; }
}

/// <summary>
/// ͳһ��֤��Ȩ����������
/// </summary>
public class IdentityServer
{
    /// <summary>
    /// ����
    /// </summary>
    public bool Enable { get; set; } = false;

    /// <summary>
    /// ��ַ
    /// </summary>
    public string Url { get; set; } = "https://localhost:5000";

    /// <summary>
    /// ����Https
    /// </summary>
    public bool RequireHttpsMetadata { get; set; } = false;

    /// <summary>
    /// ����
    /// </summary>
    public string Audience { get; set; } = "admin.server.api";
}

/// <summary>
/// Aop����
/// </summary>
public class AopConfig
{
    /// <summary>
    /// ����
    /// </summary>
    public bool Transaction { get; set; } = true;
}



/// <summary>
/// ��־����
/// </summary>
public class LogConfig
{
    /// <summary>
    /// ������־
    /// </summary>
    public bool Operation { get; set; } = true;

    /// <summary>
    /// ��¼��־��ʽ
    /// </summary>
    public LogMethod Method { get; set; } = LogMethod.Grpc;
}

/// <summary>
/// ��֤����
/// </summary>
public class ValidateConfig
{
    /// <summary>
    /// ��¼
    /// </summary>
    public bool Login { get; set; } = true;

    /// <summary>
    /// �ӿ�Ȩ��
    /// </summary>
    public bool Permission { get; set; } = true;

    /// <summary>
    /// ����Ȩ��
    /// </summary>
    public bool DataPermission { get; set; } = true;

    /// <summary>
    /// �ӿ�����Ȩ��
    /// </summary>
    public bool ApiDataPermission { get; set; } = false;
}

/// <summary>
/// ��֤������
/// </summary>
public class VarifyCodeConfig
{
    /// <summary>
    /// ����
    /// </summary>
    public bool Enable { get; set; } = true;

    /// <summary>
    /// ������־
    /// </summary>
    public string[] Fonts { get; set; }// = new[] { "Times New Roman", "Verdana", "Arial", "Gungsuh", "Impact" };
}

/// <summary>
/// ��Ŀ����
/// </summary>
public class ProjectConfig
{
    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// �汾
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Description { get; set; }
}

/// <summary>
/// ��̬api����
/// </summary>
public class DynamicApiConfig
{
    /// <summary>
    /// �����ʽ��
    /// </summary>
    public bool FormatResult { get; set; } = true;
}

/// <summary>
/// �����������
/// </summary>
public class HealthChecksConfig
{
    /// <summary>
    /// ����
    /// </summary>
    public bool Enable { get; set; } = true;

    /// <summary>
    /// ����·��
    /// </summary>
    public string Path { get; set; } = "/health";
}

/// <summary>
/// ������ȹ������
/// </summary>
public class TaskSchedulerUIConfig
{
    /// <summary>
    /// ����
    /// </summary>
    public bool Enable { get; set; } = false;

    /// <summary>
    /// ����·��
    /// </summary>
    public string Path { get; set; } = "/task";
}

public class IdGeneratorConfig: IdGeneratorOptions
{
    public string CachePrefix { get; set; } = "zhontai:workerid:";
}

/// <summary>
/// Ӧ�ó�������
/// </summary>
public enum AppType
{
    Controllers,
    ControllersWithViews,
    MVC
}

/// <summary>
/// ��¼��־��ʽ
/// </summary>
public enum LogMethod
{
    Grpc,
    Cap
}
