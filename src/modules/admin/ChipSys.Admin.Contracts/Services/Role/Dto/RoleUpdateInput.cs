using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Role.Dto;

/// <summary>
/// �޸�
/// </summary>
public partial class RoleUpdateInput : RoleAddInput
{
    /// <summary>
    /// ��ɫId
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ���ɫ")]
    public long Id { get; set; }
}
