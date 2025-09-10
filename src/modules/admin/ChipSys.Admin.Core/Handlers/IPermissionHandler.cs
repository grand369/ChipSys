using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Core.Handlers;

/// <summary>
/// 权限处理接口
/// </summary>
public interface IPermissionHandler
{
    /// <summary>
    /// 权限验证
    /// </summary>
    /// <param name="api"></param>
    /// <param name="httpMethod"></param>
    /// <param name="apiAccess"></param>
    /// <returns></returns>
    Task<bool> ValidateAsync(string api, string httpMethod, ApiAccessAttribute apiAccess);
}
