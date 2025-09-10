using ProtoBuf.Grpc;
using ChipSys.Admin.Core.GrpcServices;
using ChipSys.Admin.Core.GrpcServices.Dtos;
using ChipSys.Admin.Domain.Api;

namespace ChipSys.Admin.GrpcServices;

public class ApiGrpcService : IApiGrpcService
{
    private readonly IApiRepository _apiRepository;

    public ApiGrpcService(IApiRepository apiRepository)
    {
        _apiRepository = apiRepository;
    }

    public async Task<GrpcOutput<List<ApiGrpcOutput>>> GetApiList(CallContext context = default)
    {
        var data = await _apiRepository.Select.ToListAsync<ApiGrpcOutput>();
        var output = new GrpcOutput<List<ApiGrpcOutput>>()
        {
            Data = data
        };
        return output;
    }
}
