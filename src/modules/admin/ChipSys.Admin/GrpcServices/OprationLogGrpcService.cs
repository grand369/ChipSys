using Mapster;
using ProtoBuf.Grpc;
using ChipSys.Admin.Core.GrpcServices;
using ChipSys.Admin.Core.GrpcServices.Dtos;
using ChipSys.Admin.Core.Protos;
using ChipSys.Admin.Services.Api.Dto;
using ChipSys.Admin.Services.OperationLog;

namespace ChipSys.Admin.GrpcServices;

public class OprationLogGrpcService : IOprationLogGrpcService
{
    private readonly IOperationLogService _operationLogService;

    public OprationLogGrpcService(IOperationLogService operationLogService)
    {
        _operationLogService = operationLogService;
    }

    public async Task<ProtoLong> AddAsync(OperationLogAddGrpcInput input, CallContext context = default)
    {
        return await _operationLogService.AddAsync(input.Adapt<OperationLogAddInput>());
    }
}
