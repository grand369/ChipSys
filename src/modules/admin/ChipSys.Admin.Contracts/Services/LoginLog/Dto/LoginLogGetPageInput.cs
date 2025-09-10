namespace ChipSys.Admin.Services.LoginLog.Dto;

/// <summary>
/// ��ҳ����
/// </summary>
public class LoginLogGetPageInput
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
