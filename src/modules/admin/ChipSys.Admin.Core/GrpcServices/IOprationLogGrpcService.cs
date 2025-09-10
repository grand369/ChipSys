using ProtoBuf.Grpc;
using System.ServiceModel;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.GrpcServices.Dtos;
using ChipSys.Admin.Core.Protos;

namespace ChipSys.Admin.Core.GrpcServices;

/// <summary>
/// ������־����ӿ�
/// </summary>
[ServiceContract(ConfigurationName = AdminConsts.AreaName)]
public interface IOprationLogGrpcService
{
    Task<ProtoLong> AddAsync(OperationLogAddGrpcInput input, CallContext context = default);
}
