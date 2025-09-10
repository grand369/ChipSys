using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Permission.Dto;

/// <summary>
/// �����⻧Ȩ��
/// </summary>
public class PermissionSaveTenantPermissionsInput
{
    [Required(ErrorMessage = "�⻧����Ϊ��")]
    public long TenantId { get; set; }

    [Required(ErrorMessage = "Ȩ�޲���Ϊ��")]
    public List<long> PermissionIds { get; set; }
}
