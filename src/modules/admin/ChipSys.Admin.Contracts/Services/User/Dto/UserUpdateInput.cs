using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// �޸�
/// </summary>
public partial class UserUpdateInput: UserFormInput
{
    /// <summary>
    /// ����Id
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ���û�")]
    public override long Id { get; set; }
}
