using ProtoBuf.Grpc;
using System.ServiceModel;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.GrpcServices.Dtos;

namespace ChipSys.Admin.Core.GrpcServices;

/// <summary>
/// Api����ӿ�
/// </summary>
[ServiceContract(ConfigurationName = AdminConsts.AreaName)]
public interface IApiGrpcService
{
    Task<GrpcOutput<List<ApiGrpcOutput>>> GetApiList(CallContext context = default);
}
