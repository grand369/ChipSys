using FreeScheduler;
using TaskStatus = FreeScheduler.TaskStatus;

namespace ChipSys.Admin.Services.TaskScheduler.Dto;

/// <summary>
/// �����ҳ��Ӧ
/// </summary>
public class TaskGetPageOutput
{
    /// <summary>
    /// ����
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// �������
    /// </summary>
    public string Topic { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// ����ִ�ж�����
    /// </summary>
    public int Round { get; set; }

    /// <summary>
    /// ��ʱ����
    /// </summary>
    public TaskInterval Interval { get; set; }

    /// <summary>
    /// ��ʱ����ֵ
    /// </summary>
    public string IntervalArgument { get; set; }

    /// <summary>
    /// ����״̬
    /// </summary>
    public TaskStatus Status { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// �������ʱ��
    /// </summary>
    public DateTime LastRunTime { get; set; }

    /// <summary>
    /// ��ǰ���е��ڼ���
    /// </summary>
    public int CurrentRound { get; set; }

    /// <summary>
    /// �����
    /// </summary>
    public int ErrorTimes { get; set; }
}
