using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Core.Handlers;

/// <summary>
/// Ȩ�޴���ӿ�
/// </summary>
public interface IPermissionHandler
{
    /// <summary>
    /// Ȩ����֤
    /// </summary>
    /// <param name="api"></param>
    /// <param name="httpMethod"></param>
    /// <param name="apiAccess"></param>
    /// <returns></returns>
    Task<bool> ValidateAsync(string api, string httpMethod, ApiAccessAttribute apiAccess);
}
