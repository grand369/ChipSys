using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ChipSys.Admin.Core.Startup;

/// <summary>
/// HostApp�м��������
/// </summary>
public class HostAppMiddlewareContext
{
    /// <summary>
    /// Ӧ��
    /// </summary>
    public WebApplication App { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public IHostEnvironment Environment { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public IConfiguration Configuration { get; set; }
}

