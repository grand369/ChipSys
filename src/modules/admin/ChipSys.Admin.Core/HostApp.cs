using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;
using AspNetCoreRateLimit;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HealthChecks.UI.Client;
using FreeRedis;
using FreeScheduler;
using FreeSql;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using NLog;
using NLog.Web;
using Swashbuckle.AspNetCore.SwaggerGen;
using Yitter.IdGenerator;
using ChipSys.Admin.Core.Auth;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Configs;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Conventions;
using ChipSys.Admin.Core.Db;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Extensions;
using ChipSys.Admin.Core.Filters;
using ChipSys.Admin.Core.Handlers;
using ChipSys.Admin.Core.RegisterModules;
using ChipSys.Admin.Core.Startup;
using ChipSys.Admin.Core.Middlewares;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Tools.Cache;
using ChipSys.Common.Helpers;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using IP2Region.Net.Abstractions;
using IP2Region.Net.XDB;
using ProtoBuf.Grpc.Server;
using ChipSys.Admin.Core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ChipSys.Admin.Core.GrpcServices;

namespace ChipSys.Admin.Core;

/// <summary>
/// ����Ӧ��
/// </summary>
public class HostApp
{
    readonly HostAppOptions _hostAppOptions;

    /// <summary>
    /// ���������ļ�
    /// </summary>
    /// <param name="configuration">����</param>
    /// <param name="environmentName">������</param>
    /// <param name="directory">Ŀ¼</param>
    /// <param name="optional">��ѡ</param>
    /// <param name="reloadOnChange">�ȸ���</param>
    private static void AddJsonFilesFromDirectory(
        ConfigurationManager configuration,
        string environmentName,
        string directory = "ConfigCenter",
        bool optional = true,
        bool reloadOnChange = true)
    {
        var allFilePaths = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, directory).ToPath())
            .Where(p => p.EndsWith($".json", StringComparison.OrdinalIgnoreCase));

        var environmentFilePaths = allFilePaths.Where(p => p.EndsWith($".{environmentName}.json", StringComparison.OrdinalIgnoreCase));
        var otherFilePaths = allFilePaths.Except(environmentFilePaths);
        var filePaths = otherFilePaths.Concat(environmentFilePaths);

        foreach (var filePath in filePaths)
        {
            configuration.AddJsonFile(filePath, optional: optional, reloadOnChange: reloadOnChange);
        }
    }

    /// <summary>
    /// ����Ӧ��
    /// </summary>
    public HostApp()
    {
    }

    /// <summary>
    /// ����Ӧ��
    /// </summary>
    /// <param name="hostAppOptions"></param>
    public HostApp(HostAppOptions hostAppOptions)
    {
        _hostAppOptions = hostAppOptions;
    }

    /// <summary>
    /// ����Ӧ��
    /// </summary>
    /// <param name="args"></param>
    /// <param name="assembly"></param>
    public void Run(string[] args, Assembly assembly = null)
    {
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        try
        {
            //Ӧ�ó�������
            logger.Info("Application startup");

            var builder = WebApplication.CreateBuilder(args);
            _hostAppOptions?.ConfigurePreWebApplicationBuilder?.Invoke(builder);

            builder.ConfigureApplication(assembly ?? Assembly.GetCallingAssembly());
            //�����־��Ӧ���򣬱���.net�Դ���־���������̨
            builder.Logging.ClearProviders();
            //ʹ��NLog��־
            builder.Host.UseNLog();

            var services = builder.Services;
            var env = builder.Environment;
            var configuration = builder.Configuration;

            //��������
            configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            if (env.EnvironmentName.NotNull())
            {
                configuration.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }

            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            var appSettings = AppInfo.GetOptions<AppSettings>();
            services.Configure<RpcConfig>(configuration.GetSection("RpcConfig"));
            if (appSettings.UseConfigCenter)
            {
                AddJsonFilesFromDirectory(configuration, env.EnvironmentName, appSettings.ConfigCenterPath);
                services.Configure<AppConfig>(configuration.GetSection("AppConfig"));
                services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
                services.Configure<DbConfig>(configuration.GetSection("DbConfig"));
                services.Configure<CacheConfig>(configuration.GetSection("CacheConfig"));
                services.Configure<OSSConfig>(configuration.GetSection("OssConfig"));
                services.Configure<ImConfig>(configuration.GetSection("ImConfig"));
            }
            else
            {
                //appӦ������
                services.Configure<AppConfig>(ConfigHelper.Load("appconfig", env.EnvironmentName));
                //jwt����
                services.Configure<JwtConfig>(ConfigHelper.Load("jwtconfig", env.EnvironmentName));
                //���ݿ�����
                services.Configure<DbConfig>(ConfigHelper.Load("dbconfig", env.EnvironmentName));
                //��������
                services.Configure<CacheConfig>(ConfigHelper.Load("cacheconfig", env.EnvironmentName));
                //oss�ϴ�����
                services.Configure<OSSConfig>(ConfigHelper.Load("ossconfig", env.EnvironmentName));
                //im����
                services.Configure<ImConfig>(ConfigHelper.Load("imconfig", env.EnvironmentName));
                //��������
                configuration.AddJsonFile("./Configs/ratelimitconfig.json", optional: true, reloadOnChange: true);
                if (env.EnvironmentName.NotNull())
                {
                    configuration.AddJsonFile($"./Configs/ratelimitconfig.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                }
            }

            services.Configure<EmailConfig>(configuration.GetSection("Email"));

            //appӦ������
            var appConfig = AppInfo.GetOptions<AppConfig>();
            services.AddSingleton(appConfig);

            //jwt����
            services.AddSingleton(AppInfo.GetOptions<JwtConfig>());

            //���ݿ�����
            services.AddSingleton(AppInfo.GetOptions<DbConfig>());

            //��������
            services.AddSingleton(AppInfo.GetOptions<CacheConfig>());

            var hostAppContext = new HostAppContext()
            {
                Services = services,
                Environment = env,
                Configuration = configuration
            };

            //ʹ��Autofac����
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            //����Autofac����
            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                // ��������ע��
                builder.RegisterModule(new LifecycleModule(appConfig));

                // ������ע��
                builder.RegisterModule(new ControllerModule());

                // ģ��ע��
                builder.RegisterModule(new RegisterModule(appConfig));

                _hostAppOptions?.ConfigureAutofacContainer?.Invoke(builder, hostAppContext);
            });

            //����Kestrel������
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(appConfig.Kestrel.KeepAliveTimeout);
                options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(appConfig.Kestrel.RequestHeadersTimeout);
                options.Limits.MaxRequestBodySize = appConfig.Kestrel.MaxRequestBodySize;
            });

            //���ʵ�ַ
            if (appConfig.Urls?.Length > 0)
            {
                builder.WebHost.UseUrls(appConfig.Urls);
            }

            //���÷���
            ConfigureServices(services, env, configuration, appConfig);

            _hostAppOptions?.ConfigureWebApplicationBuilder?.Invoke(builder);

            var app = builder.Build();

            app.ConfigureApplication();

            app.Lifetime.ApplicationStarted.Register(() =>
            {
                AppInfo.IsRun = true;
            });

            app.Lifetime.ApplicationStopped.Register(() =>
            {
                AppInfo.IsRun = false;
            });

            //�����м��
            ConfigureMiddleware(app, env, configuration, appConfig);

            app.Run();

            //Ӧ�ó���ֹͣ
            logger.Info("Application shutdown");
        }
        catch (Exception exception)
        {
            //Ӧ�ó����쳣
            logger.Error(exception, "Application stopped because of exception");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
    }

    /// <summary>
    /// ���÷���
    /// </summary>
    /// <param name="services"></param>
    /// <param name="env"></param>
    /// <param name="configuration"></param>
    /// <param name="appConfig"></param>
    private void ConfigureServices(IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration, AppConfig appConfig)
    {
        var hostAppContext = new HostAppContext()
        {
            Services = services,
            Environment = env,
            Configuration = configuration
        };

        //������
        if (appConfig.Lang.EnableJson)
        {
            services.AddJsonLocalization(options => options.ResourcesPath = "Resources");
        }
        else
        {
            services.AddLocalization(opt => opt.ResourcesPath = "Resources");
        }

        _hostAppOptions?.ConfigurePreServices?.Invoke(hostAppContext);

        //�������
        services.AddHealthChecks();

        var cacheConfig = AppInfo.GetOptions<CacheConfig>();

        #region ����
        //�����ڴ滺��
        services.AddMemoryCache();
        if (cacheConfig.Type == CacheType.Redis)
        {
            //FreeRedis�ͻ���
            var redis = new RedisClient(cacheConfig.Redis.ConnectionString)
            {
                Serialize = JsonHelper.Serialize,
                Deserialize = JsonHelper.Deserialize
            };
            services.AddSingleton(redis);
            services.AddSingleton<IRedisClient>(redis);
            //Redis����
            services.AddSingleton<ICacheTool, RedisCacheTool>();
            //�ֲ�ʽRedis����
            services.AddSingleton<IDistributedCache>(new DistributedCache(redis));
            if(_hostAppOptions?.ConfigureIdGenerator != null)
            {
                _hostAppOptions?.ConfigureIdGenerator?.Invoke(appConfig.IdGenerator);
                YitIdHelper.SetIdGenerator(appConfig.IdGenerator);
            }
            else
            {
                //�ֲ�ʽId������
                services.AddIdGenerator();
            }
        }
        else
        {
            //�ڴ滺��
            services.AddSingleton<ICacheTool, MemoryCacheTool>();
            //�ֲ�ʽ�ڴ滺��
            services.AddDistributedMemoryCache();
            //Id������
            _hostAppOptions?.ConfigureIdGenerator?.Invoke(appConfig.IdGenerator);
            YitIdHelper.SetIdGenerator(appConfig.IdGenerator);
        }

        #endregion ����

        //Ȩ�޴���
        services.AddScoped<IPermissionHandler, PermissionHandler>();

        // ClaimType��������
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        //�û���Ϣ
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.TryAddScoped<IUser, User>();

        //�������ݿ�
        if (!_hostAppOptions.CustomInitDb)
        {
            services.AddDb(env, _hostAppOptions);
        }

        //����
        Assembly[] assemblies = AssemblyHelper.GetAssemblyList(appConfig.AssemblyNames);

        #region Mapster ӳ������
        services.AddScoped<IMapper>(sp => new Mapper());
        if (assemblies?.Length > 0)
        {
            TypeAdapterConfig.GlobalSettings.Scan(assemblies);
        }
        #endregion Mapster ӳ������

        #region Cors ����
        services.AddCors(options =>
        {
            //ָ���������ʱԤ��ȴ�ʱ��
            var preflightMaxAge = appConfig.PreflightMaxAge > 0 ? new TimeSpan(0, 0, appConfig.PreflightMaxAge) : new TimeSpan(0, 30, 0);
            options.AddDefaultPolicy(policy =>
            {
                policy.SetPreflightMaxAge(preflightMaxAge);

                var hasOrigins = appConfig.CorUrls?.Length > 0;
                if (hasOrigins)
                {
                    policy.WithOrigins(appConfig.CorUrls);
                }
                else
                {
                    policy.AllowAnyOrigin();
                }
                policy
                .AllowAnyHeader()
                .AllowAnyMethod();

                if (hasOrigins)
                {
                    policy.AllowCredentials();
                }

                policy.WithExposedHeaders("Content-Disposition");
            });

            //�����κ�Դ����Api���ԣ�ʹ��ʱ�ڿ��������߽ӿ�����������[EnableCors(AdminConsts.AllowAnyPolicyName)]
            options.AddPolicy(AdminConsts.AllowAnyPolicyName, policy =>
            {
                policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });

        #endregion Cors ����

        #region ������֤��Ȩ
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = nameof(ResponseAuthenticationHandler); //401
            options.DefaultForbidScheme = nameof(ResponseAuthenticationHandler);    //403
        })
        //.AddCookie(options =>
        //{
        //    options.Cookie.SameSite = SameSiteMode.Lax;
        //})
        .AddJwtBearer(options =>
        {
            //ids4
            if (appConfig.IdentityServer.Enable)
            {
                options.Authority = appConfig.IdentityServer.Url;
                options.RequireHttpsMetadata = appConfig.IdentityServer.RequireHttpsMetadata;
                options.Audience = appConfig.IdentityServer.Audience;
            }
            else
            {
                var jwtConfig = AppInfo.GetOptions<JwtConfig>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecurityKey)),
                    ClockSkew = TimeSpan.FromSeconds(10)
                };
            }
        })
        .AddScheme<AuthenticationSchemeOptions, ResponseAuthenticationHandler>(nameof(ResponseAuthenticationHandler), o => { });

        #endregion ������֤��Ȩ

        #region ������־

        services.AddScoped<ILogHandler, LogHandler>();

        #endregion ������־

        #region ������
        void mvcConfigure(MvcOptions options)
        {
            //options.Filters.Add<ControllerExceptionFilter>();
            options.Filters.Add<ValidateInputFilter>();
            if (appConfig.Validate.Login || appConfig.Validate.Permission)
            {
                options.Filters.Add<ValidatePermissionAttribute>();
            }
            //�ھ��нϸߵ� Order ֵ��ɸѡ��֮ǰ���� before ����
            //�ھ��нϸߵ� Order ֵ��ɸѡ��֮������ after ����
            if (appConfig.DynamicApi.FormatResult)
            {
                options.Filters.Add<FormatResultFilter>(20);
            }

            options.Filters.Add<ControllerLogFilter>(10);

            //��ֹȥ��ActionAsync��׺
            //options.SuppressAsyncSuffixInActionNames = false;

            if (env.IsDevelopment() || appConfig.Swagger.Enable)
            {
                //API����Լ��
                options.Conventions.Add(new ApiGroupConvention());
            }
        }

        var mvcBuilder = appConfig.AppType switch
        {
            AppType.Controllers => services.AddControllers(mvcConfigure),
            AppType.ControllersWithViews => services.AddControllersWithViews(mvcConfigure),
            AppType.MVC => services.AddMvc(mvcConfigure),
            _ => services.AddControllers(mvcConfigure)
        };

        if (assemblies?.Length > 0)
        {
            foreach (var assembly in assemblies)
            {
                services.AddValidatorsFromAssembly(assembly);
            }
        }
        services.AddFluentValidationAutoValidation();

        mvcBuilder.AddJsonOptions(options =>
        {
            var jsonSerializerOptions = options.JsonSerializerOptions;
            var currentJsonSerializerOptions = JsonHelper.GetCurrentOptions();
            currentJsonSerializerOptions.Adapt(jsonSerializerOptions);
            foreach(var converter in currentJsonSerializerOptions.Converters)
            {
                jsonSerializerOptions.Converters.Add(converter);
            }
        }).AddControllersAsServices();

        if (appConfig.Lang.EnableJson)
        {
            //����ģ����Ϣ
            var modules = new List<ModuleInfo>();
            foreach (var assembly in assemblies)
            {
                modules.Add(new ModuleInfo
                {
                    Assembly = assembly,
                    LocalizerType = assembly.GetTypes().FirstOrDefault(m => typeof(IModuleLocalizer).IsAssignableFrom(m))
                });
            }

            mvcBuilder
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    var module = modules.FirstOrDefault(m => m.Assembly == type.Assembly);
                    if (module != null && module.LocalizerType != null)
                    {
                        return factory.Create(module.LocalizerType);
                    }

                    return factory.Create(type);
                };
            });
        }

        if (appConfig.Swagger.EnableJsonStringEnumConverter)
            mvcBuilder.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        _hostAppOptions?.ConfigureMvcBuilder?.Invoke(mvcBuilder, hostAppContext);
        #endregion ������

        #region Swagger Api�ĵ�

        if (env.IsDevelopment() || appConfig.Swagger.Enable)
        {
            services.AddSwaggerGen(options =>
            {
                appConfig.Swagger.Projects?.ForEach(project =>
                {
                    options.SwaggerDoc(project.Code.ToLower(), new OpenApiInfo
                    {
                        Title = project.Name,
                        Version = project.Version,
                        Description = project.Description
                    });
                });

                options.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    var api = controllerAction.AttributeRouteInfo.Template;
                    api = Regex.Replace(api, @"[\{\\\/\}]", "-") + "-" + apiDesc.HttpMethod.ToLower();
                    return api.Replace("--", "-");
                });

                options.ResolveConflictingActions(apiDescription => apiDescription.First());

                string DefaultSchemaIdSelector(Type modelType)
                {
                    var modelName = modelType.Name;
                    if (appConfig.Swagger.EnableSchemaIdNamespace)
                    {
                        var nameSpaceList = appConfig.Swagger.AssemblyNameList;
                        if (nameSpaceList?.Length > 0)
                        {
                            var nameSpace = modelType.Namespace;
                            if (nameSpaceList.Where(a => nameSpace.Contains(a)).Any())
                            {
                                modelName = modelType.FullName;
                            }
                        }
                        else
                        {
                            modelName = modelType.FullName;
                        }
                    }

                    if (modelType.IsConstructedGenericType)
                    {
                        var prefix = modelType.GetGenericArguments()
                        .Select(DefaultSchemaIdSelector)
                        .Aggregate((previous, current) => previous + current);

                        modelName = modelName.Split('`').First() + prefix;
                    }
                    else
                    {
                        modelName = modelName.Replace("[]", "Array");
                    }

                    if (modelType.IsDefined(typeof(SchemaIdAttribute)))
                    {
                        var swaggerSchemaIdAttribute = modelType.GetCustomAttribute<SchemaIdAttribute>(false);
                        if (swaggerSchemaIdAttribute.SchemaId.NotNull())
                        {
                            return swaggerSchemaIdAttribute.SchemaId;
                        }
                        else
                        {
                            return swaggerSchemaIdAttribute.Prefix + modelName + swaggerSchemaIdAttribute.Suffix;
                        }
                    }

                    return modelName;
                }

                options.CustomSchemaIds(modelType => DefaultSchemaIdSelector(modelType));

                //֧�ֶ����
                options.DocInclusionPredicate((docName, apiDescription) =>
                {
                    var nonGroup = false;
                    var groupNames = new List<string>();
                    var dynamicApiAttribute = apiDescription.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is DynamicApiAttribute);
                    if (dynamicApiAttribute != null)
                    {
                        var dynamicApi = dynamicApiAttribute as DynamicApiAttribute;
                        if (dynamicApi.GroupNames?.Length > 0)
                        {
                            groupNames.AddRange(dynamicApi.GroupNames);
                        }
                    }

                    var apiGroupAttribute = apiDescription.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is ApiGroupAttribute);
                    if (apiGroupAttribute != null)
                    {
                        var apiGroup = apiGroupAttribute as ApiGroupAttribute;
                        if (apiGroup.GroupNames?.Length > 0)
                        {
                            groupNames.AddRange(apiGroup.GroupNames);
                        }
                        nonGroup = apiGroup.NonGroup;
                    }

                    return docName == apiDescription.GroupName || groupNames.Any(a => a == docName) || nonGroup;
                });

                string[] xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
                if (xmlFiles.Length > 0)
                {
                    foreach (var xmlFile in xmlFiles)
                    {
                        options.IncludeXmlComments(xmlFile, true);
                    }
                }

                var server = new OpenApiServer()
                {
                    Url = appConfig.Swagger.Url,
                    Description = ""
                };
                if (appConfig.ApiUI.Footer.Enable)
                {
                    server.Extensions.Add("extensions", new OpenApiObject
                    {
                        ["copyright"] = new OpenApiString(appConfig.ApiUI.Footer.Content)
                    });
                }
                options.AddServer(server);

                if (appConfig.Swagger.EnableEnumSchemaFilter)
                {
                    options.SchemaFilter<EnumSchemaFilter>();
                }
                if (appConfig.Swagger.EnableOrderTagsDocumentFilter)
                {
                    options.DocumentFilter<OrderTagsDocumentFilter>();
                }
                options.OrderActionsBy(apiDesc =>
                {
                    var order = 0;
                    var objOrderAttribute = apiDesc.CustomAttributes().FirstOrDefault(x => x is OrderAttribute);
                    if (objOrderAttribute != null)
                    {
                        var orderAttribute = objOrderAttribute as OrderAttribute;
                        order = orderAttribute.Value;
                    }
                    return (int.MaxValue - order).ToString().PadLeft(int.MaxValue.ToString().Length, '0');
                });

                #region ��������Token�İ�ť

                if (appConfig.IdentityServer.Enable)
                {
                    //����Jwt��֤����
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "oauth2",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });

                    //ͳһ��֤
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Description = "oauth2��¼��Ȩ",
                        Flows = new OpenApiOAuthFlows
                        {
                            Implicit = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri($"{appConfig.IdentityServer.Url}/connect/authorize", UriKind.Absolute),
                                TokenUrl = new Uri($"{appConfig.IdentityServer.Url}/connect/token", UriKind.Absolute),
                                Scopes = new Dictionary<string, string>
                                {
                                    { "admin.server.api", "admin���api" }
                                }
                            }
                        }
                    });
                }
                else
                {
                    //����Jwt��֤����
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });

                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Value: Bearer {token}",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
                }

                #endregion ��������Token�İ�ť
            });
        }

        #endregion Swagger Api�ĵ�

        services.AddHttpClient();

        _hostAppOptions?.ConfigureServices?.Invoke(hostAppContext);

        #region IP����

        if (appConfig.RateLimit)
        {
            services.AddIpRateLimit(configuration, cacheConfig);
        }

        #endregion IP����

        //��ֹNLog����״̬��Ϣ
        services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);

        //���ܷ���
        if (appConfig.MiniProfiler)
        {
            services.AddMiniProfiler();
        }

        //��̬api
        services.AddDynamicApi(options =>
        {
            options.FormatResult = appConfig.DynamicApi.FormatResult;
            options.FormatResultType = typeof(ResultOutput<>);
            options.AddAssemblyOptions(GetType().Assembly);
            _hostAppOptions?.ConfigureDynamicApi?.Invoke(options);
        });

        //oss�ļ��ϴ�
        services.AddOSS();

        //IP��ַ��λ��
        if (appConfig.IP2Region.Enable)
        {
            services.AddSingleton<ISearcher>(new Searcher(CachePolicy.Content, Path.Combine(AppContext.BaseDirectory, "ip2region.xdb")));
        }

        //im��ʱͨѶ
        var imConfig = AppInfo.GetOptions<ImConfig>();
        if (imConfig.Enable)
        {
            services.AddIm();
        }

        // Api�ĵ�����
        //services.AddSingleton<IApiDocumentHandler, ApiDocumentHandler>();

        //Grpc
        services.AddCodeFirstGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            //options.ResponseCompressionLevel = CompressionLevel.Optimal;
        });
        //for postman
        services.AddCodeFirstGrpcReflection();

        var rpcConfig = AppInfo.GetOptions<RpcConfig>();
        if (rpcConfig?.Grpc != null && rpcConfig.Grpc.Enable)
        {
            services.AddMyGrpcClients(AssemblyHelper.GetAssemblyList(rpcConfig.Grpc.AssemblyNames), rpcConfig, PolicyHelper.GetPolicyList());
        }
        if (rpcConfig?.Http != null && rpcConfig.Http.Enable)
        {
            services.AddMyHttpClients(AssemblyHelper.GetAssemblyList(rpcConfig.Http.AssemblyNames), rpcConfig, PolicyHelper.GetPolicyList());
        }

        _hostAppOptions?.ConfigurePostServices?.Invoke(hostAppContext);
    }

    /// <summary>
    /// �����м��
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <param name="configuration"></param>
    /// <param name="appConfig"></param>
    private void ConfigureMiddleware(WebApplication app, IWebHostEnvironment env, IConfiguration configuration, AppConfig appConfig)
    {
        var hostAppMiddlewareContext = new HostAppMiddlewareContext()
        {
            App = app,
            Environment = env,
            Configuration = configuration
        };

        _hostAppOptions?.ConfigurePreMiddleware?.Invoke(hostAppMiddlewareContext);

        //�쳣����
        app.UseMiddleware<ExceptionMiddleware>();

        IdentityModelEventSource.ShowPII = true;

        //������
        app.UseMyLocalization();

        //IP����
        if (appConfig.RateLimit)
        {
            app.UseIpRateLimiting();
        }

        //���ܷ���
        if (appConfig.MiniProfiler)
        {
            app.UseMiniProfiler();
        }

        //��̬�ļ�
        app.UseDefaultFiles();
        app.UseStaticFiles();

        //·��
        app.UseRouting();

        //����
        app.UseCors();

        //app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

        //��֤
        app.UseAuthentication();

        //��Ȩ
        app.UseAuthorization();

        //��¼�û���ʼ������Ȩ��
        if (appConfig.Validate.DataPermission)
        {
            app.Use(async (ctx, next) =>
            {
                var user = ctx.RequestServices.GetRequiredService<IUser>();
                if (user?.Id > 0)
                {
                    //�ų��������ߵ�¼�ӿ�
                    var endpoint = ctx.GetEndpoint();
                    if (appConfig.Validate.ApiDataPermission && endpoint != null && !endpoint.Metadata.Any(m => m.GetType() == typeof(AllowAnonymousAttribute) || m.GetType() == typeof(LoginAttribute)))
                    {
                        var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                        var template = actionDescriptor?.AttributeRouteInfo?.Template;
                        AppInfo.CurrentDataPermissionApiPath = template.NotNull() ? $"/{template}" : null;
                    }

                    var userGrpcService = ctx.RequestServices.GetRequiredService<IUserGrpcService>();
                    await userGrpcService.GetDataPermissionAsync(AppInfo.CurrentDataPermissionApiPath);
                }

                await next();
            });
        }

        //���ö˵�
        app.MapControllers();

        //��ȡö���б��ӿ�
        if (env.IsDevelopment())
        {
            foreach (var project in appConfig.Swagger?.Projects)
            {
                app.MapGet($"/api/{project.Code.ToLower()}/get-enums", (ApiHelper apiHelper) => ResultOutput.Ok(apiHelper.GetEnumList()));
            }
        }

        _hostAppOptions?.ConfigureMiddleware?.Invoke(hostAppMiddlewareContext);

        #region Swagger Api�ĵ�
        if (env.IsDevelopment() || appConfig.Swagger.Enable)
        {
            var routePrefix = appConfig.ApiUI.RoutePrefix;
            if (!appConfig.ApiUI.Enable && routePrefix.IsNull())
            {
                routePrefix = appConfig.Swagger.RoutePrefix;
            }
            
            app.UseSwagger(optoins =>
            {
                optoins.RouteTemplate = routePrefix + (optoins.RouteTemplate.StartsWith("/") ? "" : "/") + optoins.RouteTemplate;
            });
            app.UseSwaggerUI(options =>
            {
                options.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");

                options.RoutePrefix = appConfig.Swagger.RoutePrefix;
                appConfig.Swagger.Projects?.ForEach(project =>
                {
                    options.SwaggerEndpoint($"/{routePrefix}/swagger/{project.Code.ToLower()}/swagger.json", project.Name);
                });

                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);//�۵�Api
                //options.DefaultModelsExpandDepth(-1);//����ʾModels
                if (appConfig.MiniProfiler)
                {
                    options.InjectJavascript("/swagger/mini-profiler.js?v=4.2.22+2.0");
                    options.InjectStylesheet("/swagger/mini-profiler.css?v=4.2.22+2.0");
                }

                _hostAppOptions?.ConfigureSwaggerUI?.Invoke(options);
            });
        }
        #endregion Swagger Api�ĵ�

        //ʹ�ý������
        if (appConfig.HealthChecks.Enable)
        {
            app.MapHealthChecks(appConfig.HealthChecks.Path, new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }

        //����������ȹ�������
        if (appConfig.TaskSchedulerUI.Enable)
        {
            app.UseFreeSchedulerUI(appConfig.TaskSchedulerUI.Path.NotNull() ? appConfig.TaskSchedulerUI.Path : "/task");
        }

        //�Զ�ͬ���ӿ�����
        //if (appConfig.Swagger.EnableAutoSync)
        //{
        //    var apiDocumentHandler = app.Services.GetService<IApiDocumentHandler>();
        //    Task.Run(async () => { await apiDocumentHandler.SyncAsync(); });
        //}

        //Grpc
        var rpcConfig = AppInfo.GetOptions<RpcConfig>();
        if (rpcConfig?.Grpc != null && rpcConfig.Grpc.Enable)
        {
            IEnumerable<Assembly> assemblies = [];
            if (rpcConfig.Grpc.ServerAssemblyNames?.Length > 0)
            {
                var serverAssemblies = AssemblyHelper.GetAssemblyList(rpcConfig.Grpc.ServerAssemblyNames);
                assemblies = assemblies.Union(serverAssemblies).ToList();
            }

            app.UseMyMapGrpcService(assemblies);
        }
        
        //for postman
        app.MapCodeFirstGrpcReflectionService();

        _hostAppOptions?.ConfigurePostMiddleware?.Invoke(hostAppMiddlewareContext);
    }
}
