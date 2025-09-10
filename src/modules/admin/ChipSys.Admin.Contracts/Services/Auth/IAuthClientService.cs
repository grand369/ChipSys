using Refit;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Auth;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Services.Auth.Dto;

namespace ChipSys.Admin.Services.Auth;

/// <summary>
/// ��֤��Ȩ�ͻ��˽ӿ�
/// </summary>
[HttpClientContract(AdminConsts.AreaName)]
public interface IAuthClientService
{
    [Post("/api/admin/auth/login")]
    Task<TokenInfo> LoginAsync(AuthLoginInput input);
}
