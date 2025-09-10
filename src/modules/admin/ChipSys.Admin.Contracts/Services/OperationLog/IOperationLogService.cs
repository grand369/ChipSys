using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Services.Api.Dto;
using ChipSys.Admin.Services.OperationLog.Dto;

namespace ChipSys.Admin.Services.OperationLog;

/// <summary>
/// 操作日志接口
/// </summary>
public interface IOperationLogService
{
    Task<PageOutput<OperationLogGetPageOutput>> GetPageAsync(PageInput<OperationLogGetPageInput> input);

    Task<long> AddAsync(OperationLogAddInput input);
}
