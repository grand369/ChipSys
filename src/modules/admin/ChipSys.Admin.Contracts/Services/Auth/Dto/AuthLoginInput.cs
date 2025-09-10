using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Domain.User;

namespace ChipSys.Admin.Services.Auth.Dto;

/// <summary>
/// ��¼��Ϣ
/// </summary>
public class AuthLoginInput
{
    /// <summary>
    /// �û���
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// �ֻ���
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// �����ַ
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// �˺�����
    /// </summary>
    public AccountType AccountType { get; set; } = AccountType.UserName;

    /// <summary>
    /// ����
    /// </summary>
    [Required(ErrorMessage = "���벻��Ϊ��")]
    public string Password { get; set; }

    /// <summary>
    /// �����
    /// </summary>
    public string PasswordKey { get; set; }

    /// <summary>
    /// ��֤��Id
    /// </summary>
    public string CaptchaId { get; set; }

    /// <summary>
    /// ��֤������
    /// </summary>
    public string CaptchaData { get; set; }
}
