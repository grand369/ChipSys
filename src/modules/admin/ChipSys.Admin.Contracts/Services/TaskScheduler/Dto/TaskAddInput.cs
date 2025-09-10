using FreeScheduler;

namespace ChipSys.Admin.Services.TaskScheduler.Dto;

/// <summary>
/// ���
/// </summary>
public class TaskAddInput
{
    /// <summary>
    /// �������
    /// </summary>
    public string Topic { get; set; }

    /// <summary>
    /// �������
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// ����ִ�ж����֣�-1Ϊ����ѭ��
    /// </summary>
    public int Round { get; set; }

    /// <summary>
    /// ��ʱ����
    /// </summary>
    public TaskInterval Interval { get; set; }

    /// <summary>
    /// ��ʱ���� 60,60,60,120,120,1200,1200
    /// </summary>
    public string IntervalArgument { get; set; }

    /// <summary>
    /// �����ʼ�������ʼ���ַ�ö��ŷָ�
    /// </summary>
    public string AlarmEmail { get; set; }

    /// <summary>
    /// ʧ�����Դ���
    /// </summary>
    public int? FailRetryCount { get; set; }

    /// <summary>
    /// ʧ�����Լ�����룩
    /// </summary>
    public int? FailRetryInterval { get; set; }
}
