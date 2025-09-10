using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Domain.LoginLog;
using ChipSys.Admin.Services.LoginLog.Dto;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;

namespace ChipSys.Admin.Services.LoginLog;

/// <summary>
/// ��¼��־����
/// </summary>
[Order(190)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class LoginLogService : BaseService, ILoginLogService, IDynamicApi, ICapSubscribe
{
    private readonly ILoginLogRepository _loginLogRep;

    public LoginLogService(ILoginLogRepository loginLogRep)
    {
        _loginLogRep = loginLogRep;
    }

    /// <summary>
    /// ��ѯ��ҳ
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<LoginLogGetPageOutput>> GetPageAsync(PageInput<LoginLogGetPageInput> input)
    {
        var select = _loginLogRep.Select.WhereDynamicFilter(input.DynamicFilter);
        if (User.PlatformAdmin)
        {
            select = select.DisableGlobalFilter();
        }

        if(input.Filter != null)
        {
            var addStartTime = input.Filter.AddStartTime;
            var addEndTime = input.Filter.AddEndTime;
            select = select
            .WhereIf(input.Filter.CreatedUserName.NotNull(), a => a.CreatedUserName.Contains(input.Filter.CreatedUserName))
            .WhereIf(input.Filter.Status.HasValue, a => a.Status == input.Filter.Status)
            .WhereIf(input.Filter.IP.NotNull(), a => a.IP.Contains(input.Filter.IP))
            .WhereIf(addStartTime.HasValue && !addEndTime.HasValue, a => a.CreatedTime >= addStartTime)
            .WhereIf(addEndTime.HasValue && !addStartTime.HasValue, a => a.CreatedTime < addEndTime.Value.AddDays(1))
            .WhereIf(addStartTime.HasValue && addEndTime.HasValue, a => a.CreatedTime.Value.BetweenEnd(addStartTime.Value, addEndTime.Value.AddDays(1)))
            ;
        }

        var list = await select
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<LoginLogGetPageOutput>();

        var data = new PageOutput<LoginLogGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [CapSubscribe(SubscribeNames.LoginLogAdd)]
    public async Task<long> AddAsync(LoginLogAddInput input)
    {
        var entity = Mapper.Map<LoginLogEntity>(input);
        await _loginLogRep.InsertAsync(entity);

        return entity.Id;
    }
}
