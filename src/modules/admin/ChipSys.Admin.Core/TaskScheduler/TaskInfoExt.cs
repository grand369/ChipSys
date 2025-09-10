using FreeSql.DataAnnotations;

namespace ChipSys.Admin.Domain;

/// <summary>
/// �����ʼ�
/// </summary>
public class TaskInfoExt
{
    /// <summary>
    /// ����Id
    /// </summary>
    public string TaskId { get; set; }

    /// <summary>
    /// �����ʼ�������ʼ���ַ�򶺺ŷָ�
    /// </summary>
    [Column(StringLength = 500)]
    public string AlarmEmail { get; set; }

    /// <summary>
    /// ʧ�����Դ���
    /// </summary>
    public int? FailRetryCount { get; set; }

    /// <summary>
    /// ʧ�����Լ�����룩
    /// </summary>
    public int? FailRetryInterval { get; set; }

    /// <summary>
    /// ���ʱ��
    /// </summary>
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// ����û�Id
    /// </summary>
    public long? CreatedUserId { get; set; }

    /// <summary>
    /// �޸�ʱ��
    /// </summary>
    public DateTime? ModifiedTime { get; set; }

    /// <summary>
    /// �޸��û�Id
    /// </summary>
    public long? ModifiedUserId { get; set; }
}
