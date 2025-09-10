namespace ChipSys.Admin.Services.OperationLog.Dto;

/// <summary>
/// ��ѯ��ҳ����
/// </summary>
public class OperationLogGetPageInput
{
    /// <summary>
    /// ������
    /// </summary>
    public string CreatedUserName { get; set; }

    /// <summary>
    /// ����״̬
    /// </summary>
    public bool? Status { get; set; }

    /// <summary>
    /// �����ӿ�
    /// </summary>
    public string Api { get; set; }

    /// <summary>
    /// IP
    /// </summary>
    public string IP { get; set; }

    /// <summary>
    /// ������ʼʱ��
    /// </summary>
    public DateTime? AddStartTime { get; set; }

    /// <summary>
    /// ��������ʱ��
    /// </summary>
    public DateTime? AddEndTime { get; set; }
}
