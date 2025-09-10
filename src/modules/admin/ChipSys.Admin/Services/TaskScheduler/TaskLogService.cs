using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Domain.Task.Dto;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Repositories;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using FreeScheduler;
using Mapster;

namespace ChipSys.Admin.Services.TaskScheduler;

/// <summary>
/// ������־����
/// </summary>
[Order(71)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class TaskLogService : BaseService, ITaskLogService, IDynamicApi
{
    private readonly Scheduler _scheduler;
    private readonly AdminLocalizer _adminLocalizer;
    private readonly ITaskLogRepository _taskLogRep;

    public TaskLogService(Scheduler scheduler, 
        AdminLocalizer adminLocalizer,
        TaskLogRepository taskLogRep)
    {
        _scheduler = scheduler;
        _adminLocalizer = adminLocalizer;
        _taskLogRep = taskLogRep;
    }

    /// <summary>
    /// ��ѯ��ҳ
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public PageOutput<TaskLog> GetPage(PageInput<TaskLogGetPageInput> input)
    {
        if (!(input.Filter != null && input.Filter.TaskId.NotNull()))
        {
            throw ResultOutput.Exception(_adminLocalizer["��ѡ������"]);
        }

        var result = Datafeed.GetLogs(_scheduler, input.Filter.TaskId, input.PageSize, input.CurrentPage);

        var data = new PageOutput<TaskLog>()
        {
            List = result.Logs.Adapt<List<TaskLog>>(),
            Total = result.Total
        };

        return data;
    }

    /// <summary>
    /// ���
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [NonAction]
    public void Add(TaskLog input)
    {
        _taskLogRep.InsertAsync(input);
    }
}
