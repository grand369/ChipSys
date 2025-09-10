using Microsoft.Extensions.DependencyInjection;
using ChipSys.Admin.Core.Configs;

namespace ChipSys.Admin.Core.Extensions;

/// <summary>
/// Im��չ
/// </summary>
public static class ImExtensions
{
    /// <summary>
    /// ���Im
    /// </summary>
    /// <param name="services"></param>
    public static void AddIm(this IServiceCollection services)
    {
        var imConfig = AppInfo.GetOptions<ImConfig>();

        ImHelper.Initialization(new ImClientOptions
        {
            Redis = new FreeRedis.RedisClient(imConfig.RedisConnectionString),
            Servers = imConfig.Servers,
        });

        ImHelper.Instance.OnSend += (s, e) =>
        {
            //Console.WriteLine($"ImClient.SendMessage(server={e.Server},data={JsonHelper.Serialize(e.Message)})");
        };

        ImHelper.EventBus(
            t =>
            {
                //Console.WriteLine(t.clientId + "������");
            },
            t =>
            {
                //Console.WriteLine(t.clientId + "������");
            }
        );
    }
}
