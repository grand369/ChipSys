using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChipSys.Admin.Core.Startup;

/// <summary>
/// HostApp������
/// </summary>
public class HostAppContext
{
    /// <summary>
    /// ����
    /// </summary>
    public IServiceCollection Services { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public IHostEnvironment Environment { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public IConfiguration Configuration { get; set; }
}

