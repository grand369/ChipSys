using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Auth.Dto;

/// <summary>
/// ����ע��
/// </summary>
public class AuthRegByEmailInput
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
    /// ����
    /// </summary>
    [Required(ErrorMessage = "����������")]
    public string Password { get; set; }

    /// <summary>
    /// ��ҵ����
    /// </summary>
    [Required(ErrorMessage = "��������ҵ����")]
    public string CorpName { get; set; }
}
