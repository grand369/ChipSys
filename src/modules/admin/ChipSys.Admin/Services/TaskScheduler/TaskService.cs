using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mapster;
using FreeScheduler;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Validators;
using ChipSys.Admin.Domain;
using ChipSys.Admin.Domain.Task.Dto;
using ChipSys.Admin.Services.TaskScheduler.Dto;
using ChipSys.Admin.Repositories;
using ChipSys.Admin.Resources;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;

namespace ChipSys.Admin.Services.TaskScheduler;

/// <summary>
/// �������
/// </summary>
[Order(70)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class TaskService : BaseService, ITaskService, IDynamicApi
{
    private readonly Scheduler _scheduler;
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskExtRepository _taskExtRepository;
    private readonly AdminLocalizer _adminLocalizer;

    public TaskService(Scheduler scheduler, 
        TaskRepository taskRepository, 
        TaskExtRepository taskExtRepository,
        AdminLocalizer adminLocalizer
    )
    {
        _scheduler = scheduler;
        _taskRepository = taskRepository;
        _taskExtRepository = taskExtRepository;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// ��ѯ�����ʼ�
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<string> GetAlerEmailAsync(string id)
    {
        return await _taskExtRepository.Where(a => a.TaskId == id).ToOneAsync(a => a.AlarmEmail);
    }

    /// <summary>
    /// ��ѯ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TaskGetOutput> GetAsync(string id)
    {
        var taskInfo = Datafeed.GetTask(_scheduler, id);

        if (taskInfo == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["���񲻴���"]);
        }

        var taskGetOutput = taskInfo.Adapt<TaskGetOutput>();
        var taskExt = await _taskExtRepository.Where(a => a.TaskId == id).ToOneAsync(a => new
        {
            a.AlarmEmail,
            a.FailRetryCount,
            a.FailRetryInterval
        });

        if (taskExt != null)
        {
            taskGetOutput.AlarmEmail = taskExt.AlarmEmail;
            taskGetOutput.FailRetryCount = taskExt.FailRetryCount;
            taskGetOutput.FailRetryInterval = taskExt.FailRetryInterval;
        }

        return taskGetOutput;
    }

    /// <summary>
    /// ��ѯ��ҳ
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<TaskGetPageOutput>> GetPage(PageInput<TaskGetPageInput> input)
    {
        //var result = Datafeed.GetPage(_scheduler.Value,
        //    input.Filter.ClusterId,
        //    input.Filter.Topic,
        //    input.Filter.TaskStatus,
        //    input.Filter.StartAddTime,
        //    input.Filter.EndAddTime,
        //    input.PageSize,
        //    input.CurrentPage
        //);

        //var data = new PageOutput<TaskListOutput>()
        //{
        //    List = result.Tasks.Adapt<List<TaskListOutput>>(),
        //    Total = result.Total
        //};

        var taskName = input.Filter?.TaskName;
        var groupName = input.Filter?.GroupName;
        var taskStatus = input.Filter?.TaskStatus;
        var startAddTime = input.Filter?.StartAddTime;
        var endAddTime = input.Filter?.EndAddTime;

        var list = await _taskRepository.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(groupName.NotNull(), a => a.Topic.Contains(groupName))
        .WhereIf(taskName.NotNull(), a => a.Topic.Contains(taskName))
        .WhereIf(taskStatus.HasValue,a=> a.Status == taskStatus)
        .WhereIf(startAddTime.HasValue && !endAddTime.HasValue, a => a.CreateTime >= startAddTime)
        .WhereIf(endAddTime.HasValue && !startAddTime.HasValue, a => a.CreateTime < endAddTime.Value.AddDays(1))
        .WhereIf(startAddTime.HasValue && endAddTime.HasValue, a => a.CreateTime.BetweenEnd(startAddTime.Value, endAddTime.Value.AddDays(1)))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<TaskGetPageOutput>();

        var data = new PageOutput<TaskGetPageOutput>()
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
    public async Task<string> Add(TaskAddInput input)
    {
        if (input.IntervalArgument.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["�����붨ʱ����"]);
        }

        var scheduler = _scheduler;

        var taskld = Datafeed.AddTask(scheduler, input.Topic, input.Body, input.Round, input.Interval, input.IntervalArgument);

        if (taskld.NotNull())
        {
            Pause(taskld);

            await _taskExtRepository.InsertAsync(new TaskInfoExt
            {
                TaskId = taskld,
                AlarmEmail = input.AlarmEmail,
                FailRetryCount = input.FailRetryCount,
                FailRetryInterval = input.FailRetryInterval,
            });
        }

        return taskld;
    }

    /// <summary>
    /// �޸�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(TaskUpdateInput input)
    {
        var scheduler = _scheduler;

        var entity = await _taskRepository.GetAsync(a => a.Id == input.Id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["���񲻴���"]);
        }

        if (entity.Status == FreeScheduler.TaskStatus.Running)
        {
            Pause(entity.Id);
        }

        Mapper.Map(input, entity);

        if (entity.Status == FreeScheduler.TaskStatus.Completed)
        {
            entity.Status = FreeScheduler.TaskStatus.Paused;
        }

        await _taskRepository.UpdateAsync(entity);

        var taskExt = await _taskExtRepository.Select.WhereDynamic(entity.Id).ToOneAsync();
        if(taskExt != null)
        {
            taskExt.AlarmEmail = input.AlarmEmail;
            taskExt.FailRetryCount = input.FailRetryCount;
            taskExt.FailRetryInterval = input.FailRetryInterval;
            await _taskExtRepository.UpdateAsync(taskExt);
        }
        else
        {
            await _taskExtRepository.InsertAsync(new TaskInfoExt
            {
                TaskId = entity.Id,
                AlarmEmail = input.AlarmEmail,
                FailRetryCount = input.FailRetryCount,
                FailRetryInterval = input.FailRetryInterval,
            });
        }

        if (entity.Status != FreeScheduler.TaskStatus.Paused)
        {
            Resume(entity.Id);
        }
    }

    /// <summary>
    /// ��ͣ����
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void Pause([BindRequired][ValidateRequired("��ѡ������")]string id)
    {
        var scheduler = _scheduler;
        scheduler.PauseTask(id);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void Resume([BindRequired][ValidateRequired("��ѡ������")] string id)
    {
        var scheduler = _scheduler;
        scheduler.ResumeTask(id);
    }

    /// <summary>
    /// ִ������
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void Run([BindRequired][ValidateRequired("��ѡ������")] string id)
    {
        var scheduler = _scheduler;
        scheduler.RunNowTask(id);
    }

    /// <summary>
    /// ɾ������
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task Delete([BindRequired][ValidateRequired("��ѡ������")] string id)
    {
        var scheduler = _scheduler;
        scheduler.RemoveTask(id);

        await _taskExtRepository.DeleteAsync(a => a.TaskId == id);
    }

    /// <summary>
    /// ����ִ������
    /// </summary>
    /// <param name="ids"></param>
    public void BatchRun([BindRequired][ValidateRequired("��ѡ������")] string[] ids)
    {
        foreach (var id in ids)
        {
            Run(id);
        }
    }

    /// <summary>
    /// ������ͣ����
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public void BatchPause([BindRequired][ValidateRequired("��ѡ������")] string[] ids)
    {
        foreach (var id in ids)
        {
            Pause(id);
        }
    }

    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public void BatchResume([BindRequired][ValidateRequired("��ѡ������")] string[] ids)
    {
        foreach (var id in ids)
        {
            Resume(id);
        }
    }

    /// <summary>
    /// ����ɾ������
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchDelete([BindRequired][ValidateRequired("��ѡ������")] string[] ids)
    {
        foreach (var id in ids)
        {
            await Delete(id);
        }
    }
}
