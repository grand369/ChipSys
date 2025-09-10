using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChipSys.Admin.Tools.Cache;

namespace ChipSys.Admin.Core.Extensions;

/// <summary>
/// Ip������չ
/// </summary>
public static class IpRateLimitExtensions
{
    /// <summary>
    /// ���Ip����
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="cacheConfig"></param>
    public static void AddIpRateLimit(this IServiceCollection services, IConfiguration configuration, CacheConfig cacheConfig)
    {
        #region IP����

        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));

        if (cacheConfig.TypeRateLimit == CacheType.Redis)
        {
            services.AddDistributedRateLimiting();
        }
        else
        {
            services.AddInMemoryRateLimiting();
        }
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        #endregion IP����
    }
}
