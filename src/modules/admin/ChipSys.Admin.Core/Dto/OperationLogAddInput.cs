namespace ChipSys.Admin.Services.Api.Dto;

/// <summary>
/// ���
/// </summary>
public class OperationLogAddInput
{
    /// <summary>
    /// �⻧Id
    /// </summary>
    public long? TenantId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// �ӿ�����
    /// </summary>
    public string ApiLabel { get; set; }

    /// <summary>
    /// �ӿڵ�ַ
    /// </summary>
    public string ApiPath { get; set; }

    /// <summary>
    /// �ӿ��ύ����
    /// </summary>
    public string ApiMethod { get; set; }

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
    public string Isp { get; set; }

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
    /// �������Ϣ
    /// </summary>
    public string BrowserInfo { get; set; }

    /// <summary>
    /// ��ʱ�����룩
    /// </summary>
    public long ElapsedMilliseconds { get; set; }

    /// <summary>
    /// ����״̬
    /// </summary>
    public bool? Status { get; set; }

    /// <summary>
    /// ������Ϣ
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    public string Params { get; set; }

    /// <summary>
    /// ״̬��
    /// </summary>
    public int? StatusCode { get; set; }

    /// <summary>
    /// �������
    /// </summary>
    public string Result { get; set; }

    /// <summary>
    /// �������û�Id
    /// </summary>
    public long? CreatedUserId { get; set; }

    /// <summary>
    /// �������û���
    /// </summary>
    public string CreatedUserName { get; set; }

    /// <summary>
    /// ����������
    /// </summary>
    public string CreatedUserRealName { get; set; }
}
