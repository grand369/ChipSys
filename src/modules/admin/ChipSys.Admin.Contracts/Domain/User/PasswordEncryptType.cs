namespace ChipSys.Admin.Domain.User;

/// <summary>
/// �����������
/// </summary>
public enum PasswordEncryptType
{
    /// <summary>
    /// 32λMD5����
    /// </summary>
    MD5Encrypt32 = 0,

    /// <summary>
    /// ��׼��ʶ�����ϣ
    /// </summary>
    PasswordHasher = 1,
}
