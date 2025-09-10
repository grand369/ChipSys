using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Permission.Dto;

/// <summary>
/// �޸�Ȩ�޲˵�
/// </summary>
public class PermissionUpdateMenuInput : PermissionAddMenuInput
{
    /// <summary>
    /// Ȩ��Id
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ��˵�")]
    public long Id { get; set; }
}
