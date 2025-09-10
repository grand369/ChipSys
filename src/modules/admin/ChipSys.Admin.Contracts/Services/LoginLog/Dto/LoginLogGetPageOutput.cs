namespace ChipSys.Admin.Services.LoginLog.Dto;

/// <summary>
/// ��ҳ��Ӧ
/// </summary>
public class LoginLogGetPageOutput
{
    /// <summary>
    /// ���
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// �ǳ�
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// ������
    /// </summary>
    public string CreatedUserName { get; set; }

    /// <summary>
    /// IP
    /// </summary>
    public string IP { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// ʡ��
    /// </summary>
    public string Province { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// ���������
    /// </summary>
    public string Isp { get; init; }

    /// <summary>
    /// �����
    /// </summary>
    public string Browser { get; set; }

    /// <summary>
    /// ����ϵͳ
    /// </summary>
    public string Os { get; set; }

    /// <summary>
    /// �豸
    /// </summary>
    public string Device { get; set; }

    /// <summary>
    /// ��ʱ�����룩
    /// </summary>
    public long ElapsedMilliseconds { get; set; }

    /// <summary>
    /// ����״̬
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// ������Ϣ
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public DateTime? CreatedTime { get; set; }
}
