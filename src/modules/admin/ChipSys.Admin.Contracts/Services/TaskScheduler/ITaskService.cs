using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Domain.Task.Dto;
using ChipSys.Admin.Services.TaskScheduler.Dto;

namespace ChipSys.Admin.Services.TaskScheduler;

/// <summary>
/// 任务接口
/// </summary>
public interface ITaskService
{
    Task<TaskGetOutput> GetAsync(string id);

    Task<PageOutput<TaskGetPageOutput>> GetPage(PageInput<TaskGetPageInput> input);

    Task<string> Add(TaskAddInput input);

    Task UpdateAsync(TaskUpdateInput input);

    void Pause(string id);

    void Resume(string id);

    void Run(string id);

    Task Delete(string id);
}
