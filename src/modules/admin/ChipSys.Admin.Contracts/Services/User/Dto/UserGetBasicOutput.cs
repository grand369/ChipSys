namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// �û�������Ϣ
/// </summary>
public class UserGetBasicOutput
{
    /// <summary>
    /// ͷ��
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// �ǳ�
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// �ֻ���
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// ����¼ʱ��
    /// </summary>
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// ����¼IP
    /// </summary>
    public string LastLoginIP { get; set; }

    /// <summary>
    /// ����¼����
    /// </summary>
    public string LastLoginCountry { get; set; }

    /// <summary>
    /// ����¼ʡ��
    /// </summary>
    public string LastLoginProvince { get; set; }

    /// <summary>
    /// ����¼����
    /// </summary>
    public string LastLoginCity { get; set; }
}
