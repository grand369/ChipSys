using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Configs;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Domain.OperationLog;
using ChipSys.Admin.Services.OperationLog.Dto;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using DotNetCore.CAP;
using ChipSys.Admin.Services.Api.Dto;

namespace ChipSys.Admin.Services.OperationLog;

/// <summary>
/// ������־����
/// </summary>
[Order(200)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class OperationLogService : BaseService, IOperationLogService, IDynamicApi, ICapSubscribe
{
    private readonly IOperationLogRepository _operationLogRep;
    private readonly AppConfig _appConfig;

    public OperationLogService(
        IOperationLogRepository operationLogRep,
        IOptions<AppConfig> appConfig
    )
    {
        _operationLogRep = operationLogRep;
        _appConfig = appConfig.Value;
    }

    /// <summary>
    /// ��ѯ��ҳ
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<OperationLogGetPageOutput>> GetPageAsync(PageInput<OperationLogGetPageInput> input)
    {
        var select = _operationLogRep.Select.WhereDynamicFilter(input.DynamicFilter);
        if (User.PlatformAdmin)
        {
            select = select.DisableGlobalFilter();
        }

        if (input.Filter != null)
        {
            var addStartTime = input.Filter.AddStartTime;
            var addEndTime = input.Filter.AddEndTime;
            select = select
            .WhereIf(input.Filter.CreatedUserName.NotNull(), a => a.CreatedUserName.Contains(input.Filter.CreatedUserName))
            .WhereIf(input.Filter.Status.HasValue, a => a.Status == input.Filter.Status)
            .WhereIf(input.Filter.Api.NotNull(), a => a.ApiPath.Contains(input.Filter.Api) || a.ApiLabel.Contains(input.Filter.Api))
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
        .ToListAsync<OperationLogGetPageOutput>();

        var data = new PageOutput<OperationLogGetPageOutput>()
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
    [CapSubscribe(SubscribeNames.OperationLogAdd)]
    public async Task<long> AddAsync(OperationLogAddInput input)
    {
        var entity = Mapper.Map<OperationLogEntity>(input);
        await _operationLogRep.InsertAsync(entity);

        return entity.Id;
    }
}
