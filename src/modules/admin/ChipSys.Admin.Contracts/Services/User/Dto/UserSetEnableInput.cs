namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// ��������
/// </summary>
public class UserSetEnableInput
{
    /// <summary>
    /// �û�Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool Enabled { get; set; }
}
