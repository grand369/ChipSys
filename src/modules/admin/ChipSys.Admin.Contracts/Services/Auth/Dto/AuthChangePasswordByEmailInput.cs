using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Auth.Dto;

/// <summary>
/// �����������
/// </summary>
public class AuthChangePasswordByEmailInput
{
    /// <summary>
    /// �����ַ
    /// </summary>
    [Required(ErrorMessage = "�����������ַ")]
    public string Email { get; set; }

    /// <summary>
    /// ��֤��
    /// </summary>
    [Required(ErrorMessage = "��������֤��")]
    public string Code { get; set; }

    /// <summary>
    /// ��֤��Id
    /// </summary>
    [Required(ErrorMessage = "���ȡ��֤��")]
    public string CodeId { get; set; }

    /// <summary>
    /// ������
    /// </summary>
    [Required(ErrorMessage = "������������")]
    public string NewPassword { get; set; }

    /// <summary>
    /// ȷ��������
    /// </summary>
    public string ConfirmPassword { get; set; }
}
