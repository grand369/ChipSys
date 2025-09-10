using Mapster;
using ProtoBuf.Grpc;
using ChipSys.Admin.Core.GrpcServices;
using ChipSys.Admin.Core.GrpcServices.Dtos;
using ChipSys.Admin.Core.Protos;
using ChipSys.Admin.Services.User;

namespace ChipSys.Admin.GrpcServices;

public class UserGrpcService : IUserGrpcService
{
    private readonly IUserService _userService;

    public UserGrpcService(IUserService userService)
    {
        _userService = userService;
    }

    public async Task GetDataPermissionAsync(ProtoString apiPath, CallContext context = default)
    {
        await _userService.GetDataPermissionAsync(apiPath);
    }

    public async Task<UserGetPermissionGrpcOutput> GetPermissionAsync(CallContext context = default)
    {
        var userPermission = await _userService.GetPermissionAsync();
        return userPermission.Adapt<UserGetPermissionGrpcOutput>();
    }
}
