namespace ChipSys.Admin.Services.Role.Dto;

/// <summary>
/// ��ɫ�û��б�����
/// </summary>
public partial class RoleGetRoleUserListInput
{
    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ��ɫId
    /// </summary>
    public long? RoleId { get; set; }
}
