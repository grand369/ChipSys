namespace ChipSys.Admin.Services.User.Events;

/// <summary>
/// ����ת��
/// </summary>
public class UserOrgChangeEvent
{
    /// <summary>
    /// �û�Id�б�
    /// </summary>
    public long[] UserIds { get; set; }

    /// <summary>
    /// ��������Id�б�
    /// </summary>
    public virtual long[] OrgIds { get; set; }

    /// <summary>
    /// ��������Id
    /// </summary>
    public long OrgId { get; set; }
}
