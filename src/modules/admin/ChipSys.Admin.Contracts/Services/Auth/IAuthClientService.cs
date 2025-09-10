using Refit;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Auth;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Services.Auth.Dto;

namespace ChipSys.Admin.Services.Auth;

/// <summary>
/// 认证授权客户端接口
/// </summary>
[HttpClientContract(AdminConsts.AreaName)]
public interface IAuthClientService
{
    [Post("/api/admin/auth/login")]
    Task<TokenInfo> LoginAsync(AuthLoginInput input);
}
