using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Services.LoginLog.Dto;

namespace ChipSys.Admin.Services.LoginLog;

/// <summary>
/// 登录日志接口
/// </summary>
public interface ILoginLogService
{
    Task<PageOutput<LoginLogGetPageOutput>> GetPageAsync(PageInput<LoginLogGetPageInput> input);

    Task<long> AddAsync(LoginLogAddInput input);
}
