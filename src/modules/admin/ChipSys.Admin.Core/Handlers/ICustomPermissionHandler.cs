using Microsoft.AspNetCore.Mvc.Filters;

namespace ChipSys.Admin.Core.Handlers;

/// <summary>
/// �Զ���Ȩ�޴���ӿ�
/// </summary>
public interface ICustomPermissionHandler
{
    /// <summary>
    /// Ȩ����֤
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task<bool> ValidateAsync(AuthorizationFilterContext context);
}
