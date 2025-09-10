using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Permission.Dto;

/// <summary>
/// �޸�Ȩ�޵�
/// </summary>
public class PermissionUpdateDotInput : PermissionAddDotInput
{
    /// <summary>
    /// Ȩ��Id
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ��Ȩ�޵�")]
    public long Id { get; set; }
}
