namespace ChipSys.Admin.Services.Auth.Dto;

/// <summary>
/// ��ѯ��Կ
/// </summary>
public class AuthGetPasswordEncryptKeyOutput
{
    /// <summary>
    /// �����
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// ���������Կ
    /// </summary>
    public string EncryptKey { get; set; }

    /// <summary>
    /// �����������
    /// </summary>
    public string Iv { get;  set; }
}
