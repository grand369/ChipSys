using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChipSys.Admin.Core.Captcha;
using ChipSys.Admin.Core.Consts;

namespace ChipSys.Admin.Core.Extensions;

/// <summary>
/// ������֤�������չ
/// </summary>
public static class SlideCaptchaServiceExtensions
{
    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddSlideCaptcha(this IServiceCollection services)
    {
        var configuration = AppInfo.GetRequiredService<IConfiguration>(false);
        //������֤��
        services.AddSlideCaptcha(configuration, options =>
        {
            options.StoreageKeyPrefix = CacheKeys.Captcha;
        });
        services.AddScoped<ISlideCaptcha, SlideCaptcha>();
        return services;
    }
}
