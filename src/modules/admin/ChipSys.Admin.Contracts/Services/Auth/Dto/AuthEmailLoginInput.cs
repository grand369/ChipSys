using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Auth.Dto;

/// <summary>
/// �����¼��Ϣ
/// </summary>
public class AuthEmailLoginInput
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
}
