namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// �������ò���
/// </summary>
public class UserBatchSetOrgInput
{
    /// <summary>
    /// �û�Id�б�
    /// </summary>
    public long[] UserIds { get; set; }

    /// <summary>
    /// ��������Ids
    /// </summary>
    public virtual long[] OrgIds { get; set; }

    /// <summary>
    /// ��������Id
    /// </summary>
    public long OrgId { get; set; }
}
