namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// ��������
/// </summary>
public class UserSetManagerInput
{
    /// <summary>
    /// �û�Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// ����Id
    /// </summary>
	public long OrgId { get; set; }

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool IsManager { get; set; }
}
