using ChipSys.Plugin.Lazy.SlideCaptcha.Core.Validator;
using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Captcha.Dto;

/// <summary>
/// ����������֤��
/// </summary>
public class SendEmailCodeInput
{
    /// <summary>
    /// �����ַ
    /// </summary>
    [Required(ErrorMessage = "�����������ַ")]
    public string Email { get; set; }

    /// <summary>
    /// ��֤��Id
    /// </summary>
    public string? CodeId { get; set; }

    /// <summary>
    /// ��֤��Id
    /// </summary>
    [Required(ErrorMessage = "����ɰ�ȫ��֤")]
    public string CaptchaId { get; set; }

    /// <summary>
    /// �����켣
    /// </summary>
    [Required(ErrorMessage = "����ɰ�ȫ��֤")]
    public SlideTrack Track { get; set; }
}
