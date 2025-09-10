using ProtoBuf.Grpc;
using System.ServiceModel;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.GrpcServices.Dtos;
using ChipSys.Admin.Core.Protos;

namespace ChipSys.Admin.Core.GrpcServices;

/// <summary>
/// �û�����ӿ�
/// </summary>
[ServiceContract(ConfigurationName = AdminConsts.AreaName)]
public interface IUserGrpcService
{
    Task GetDataPermissionAsync(ProtoString apiPath, CallContext context = default);

    Task<UserGetPermissionGrpcOutput> GetPermissionAsync(CallContext context = default);
}
