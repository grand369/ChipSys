namespace ChipSys.Admin.Domain.User;

/// <summary>
/// �û�����
/// </summary>
public enum UserType
{
    /// <summary>
    /// ��Ա
    /// </summary>
    Member = 0,

    /// <summary>
    /// ��ͨ�û�
    /// </summary>
    DefaultUser = 1,

    /// <summary>
    /// �⻧����Ա
    /// </summary>
    TenantAdmin = 10,

    /// <summary>
    /// ƽ̨����Ա
    /// </summary>
    PlatformAdmin = 100
}
