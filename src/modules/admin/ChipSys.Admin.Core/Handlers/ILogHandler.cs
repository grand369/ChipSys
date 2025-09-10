using Microsoft.AspNetCore.Mvc.Filters;

namespace ChipSys.Admin.Core.Handlers;

/// <summary>
/// ������־����ӿ�
/// </summary>
public interface ILogHandler
{
    /// <summary>
    /// д������־
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    Task LogAsync(ActionExecutingContext context, ActionExecutionDelegate next);
}
