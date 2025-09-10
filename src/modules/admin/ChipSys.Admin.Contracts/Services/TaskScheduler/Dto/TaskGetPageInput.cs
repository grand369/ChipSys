using TaskStatus = FreeScheduler.TaskStatus;

namespace ChipSys.Admin.Domain.Task.Dto;

/// <summary>
/// �����ҳ����
/// </summary>
public partial class TaskGetPageInput
{
    /// <summary>
    /// ��������
    /// </summary>
    public string GroupName { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    public string TaskName { get; set; }

    /// <summary>
    /// ��ȺId
    /// </summary>
    public string ClusterId { get; set; }

    /// <summary>
    /// ����״̬
    /// </summary>
    public TaskStatus? TaskStatus { get; set; }

    /// <summary>
    /// ������ʼʱ��
    /// </summary>
    public DateTime? StartAddTime { get; set; }

    /// <summary>
    /// ��������ʱ��
    /// </summary>
    public DateTime? EndAddTime { get; set; }
}
