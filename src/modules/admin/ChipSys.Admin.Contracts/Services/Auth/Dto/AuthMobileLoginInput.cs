using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Auth.Dto;

/// <summary>
/// �ֻ��ŵ�¼��Ϣ
/// </summary>
public class AuthMobileLoginInput
{
    /// <summary>
    /// �ֻ���
    /// </summary>
    [Required(ErrorMessage = "�������ֻ���")]
    public string Mobile { get; set; }

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
