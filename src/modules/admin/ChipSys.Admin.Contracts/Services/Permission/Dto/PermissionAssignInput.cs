using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Consts;

namespace ChipSys.Admin.Services.Permission.Dto;

/// <summary>
/// Ȩ�޷���
/// </summary>
public class PermissionAssignInput
{
    /// <summary>
    /// ƽ̨
    /// </summary>
    public string Platform { get; set; } = AdminConsts.WebName;

    [Required(ErrorMessage = "��ɫ����Ϊ��")]
    public long RoleId { get; set; }

    [Required(ErrorMessage = "Ȩ�޲���Ϊ��")]
    public List<long> PermissionIds { get; set; }
}
