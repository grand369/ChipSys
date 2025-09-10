using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MapsterMapper;
using ChipSys.Admin.Core.Auth;
using ChipSys.Admin.Tools.Cache;
using ChipSys.Common.Extensions;

namespace ChipSys.Admin.Services;

public abstract class BaseService: IBaseService
{
    protected readonly object ServiceProviderLock = new object();
    protected IDictionary<Type, object> CachedServices = new Dictionary<Type, object>();
    private ICacheTool _cache;
    private ILoggerFactory _loggerFactory;
    private IMapper _mapper;
    private IUser _user;

    /// <summary>
    /// ����
    /// </summary>
    public ICacheTool Cache => LazyGetRequiredService(ref _cache);

    /// <summary>
    /// ��־����
    /// </summary>
    public ILoggerFactory LoggerFactory => LazyGetRequiredService(ref _loggerFactory);

    /// <summary>
    /// ӳ��
    /// </summary>
    public IMapper Mapper => LazyGetRequiredService(ref _mapper);

    public IServiceProvider ServiceProvider { get; set; }

    /// <summary>
    /// �û���Ϣ
    /// </summary>
    public IUser User => LazyGetRequiredService(ref _user);

    /// <summary>
    /// ��־
    /// </summary>
    protected ILogger Logger => _lazyLogger.Value;

    private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);

    protected TService LazyGetRequiredService<TService>(ref TService reference)
    {
        if (reference == null)
        {
            lock (ServiceProviderLock)
            {
                if (reference == null)
                {
                    reference = ServiceProvider.GetRequiredService<TService>();
                }
            }
        }

        return reference;
    }

    /// <summary>
    /// ��������ط���
    /// </summary>
    /// <typeparam name="TService">����ӿ�</typeparam>
    /// <returns></returns>
    [NonAction]
    public virtual TService LazyGetRequiredService<TService>()
    {
        return (TService)LazyGetRequiredService(typeof(TService));
    }

    /// <summary>
    /// ���ݷ������ͻ�������ط���
    /// </summary>
    /// <param name="serviceType">��������</param>
    /// <returns></returns>
    [NonAction]
    public virtual object LazyGetRequiredService(Type serviceType)
    {
        return CachedServices.GetOrAdd(serviceType, () => ServiceProvider.GetRequiredService(serviceType));
    }
}
