using Microsoft.Extensions.DependencyInjection;

namespace ChipSys.Plugin.Lazy.SlideCaptcha.Core;

public class CaptchaBuilder
{
    public IServiceCollection Services { get; set; }

    public CaptchaBuilder(IServiceCollection serviceCollection)
    {
        Services = serviceCollection;
    }
}
