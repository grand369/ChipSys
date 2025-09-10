using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Role.Dto;

/// <summary>
/// ��ӽ�ɫ�û��б�
/// </summary>
public class RoleAddRoleUserListInput
{
    /// <summary>
    /// ��ɫ
    /// </summary>
    [Required(ErrorMessage = "��ѡ���ɫ")]
    public long RoleId { get; set; }

    /// <summary>
    /// �û�
    /// </summary>
    public long[] UserIds { get; set; }
}
