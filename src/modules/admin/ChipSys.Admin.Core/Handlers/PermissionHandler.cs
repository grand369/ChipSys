using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.GrpcServices;
using ChipSys.Admin.Core.Handlers;

namespace ChipSys.Admin.Core.Auth;

/// <summary>
/// Ȩ�޴���
/// </summary>
public class PermissionHandler : IPermissionHandler
{
    private readonly IUser _user;
    private readonly IUserGrpcService _userGrpcService;

    public PermissionHandler(IUser user, IUserGrpcService userGrpcService)
    {
        _user = user;
        _userGrpcService = userGrpcService;
    }

    /// <summary>
    /// Ȩ����֤
    /// </summary>
    /// <param name="api">�ӿ�·��</param>
    /// <param name="httpMethod">http���󷽷�</param>
    /// <param name="apiAccess">�ӿڷ���</param>
    /// <returns></returns>
    public async Task<bool> ValidateAsync(string api, string httpMethod, ApiAccessAttribute apiAccess)
    {
        if (_user.PlatformAdmin)
        {
            return true;
        }

        var userPermission = await _userGrpcService.GetPermissionAsync();

        var valid = userPermission.Apis.Any(m =>
            m.Path.NotNull() && m.Path.EqualsIgnoreCase($"/{api}")
            && m.HttpMethods.NotNull() && m.HttpMethods.Split(',').Any(n => n.NotNull() && n.EqualsIgnoreCase(httpMethod))
        );

        if (!valid && apiAccess != null) 
        {
            if (apiAccess.All)
            {
                valid = userPermission.Codes.All(a => apiAccess.Codes.Contains(a));
            }
            else
            {
                valid = userPermission.Codes.Any(a => apiAccess.Codes.Contains(a));
            }
        }

        return valid;
    }
}
