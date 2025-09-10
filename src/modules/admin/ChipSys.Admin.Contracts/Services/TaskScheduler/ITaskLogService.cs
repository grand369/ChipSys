using FreeScheduler;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Domain.Task.Dto;

namespace ChipSys.Admin.Services.TaskScheduler;

/// <summary>
/// 任务日志接口
/// </summary>
public interface ITaskLogService
{
    PageOutput<TaskLog> GetPage(PageInput<TaskLogGetPageInput> input);

    void Add(TaskLog input);
}
