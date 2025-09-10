namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// ���
/// </summary>
public class UserAddInput: UserFormInput
{
    /// <summary>
    /// ��������Ids
    /// </summary>
    public virtual long[] OrgIds { get; set; }

    /// <summary>
    /// ��������Id
    /// </summary>
    public long OrgId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public virtual string Password { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; } = true;
}
