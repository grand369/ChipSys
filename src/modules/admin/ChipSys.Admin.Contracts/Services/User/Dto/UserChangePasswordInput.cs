using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// �޸�����
/// </summary>
public class UserChangePasswordInput
{
    /// <summary>
    /// ������
    /// </summary>
    [Required(ErrorMessage = "�����������")]
    public string OldPassword { get; set; }

    /// <summary>
    /// ������
    /// </summary>
    [Required(ErrorMessage = "������������")]
    public string NewPassword { get; set; }

    /// <summary>
    /// ȷ��������
    /// </summary>
    [Required(ErrorMessage = "������ȷ��������")]
    public string ConfirmPassword { get; set; }
}
