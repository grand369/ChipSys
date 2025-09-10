using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Services.LoginLog.Dto;

namespace ChipSys.Admin.Services.LoginLog;

/// <summary>
/// ��¼��־�ӿ�
/// </summary>
public interface ILoginLogService
{
    Task<PageOutput<LoginLogGetPageOutput>> GetPageAsync(PageInput<LoginLogGetPageInput> input);

    Task<long> AddAsync(LoginLogAddInput input);
}
