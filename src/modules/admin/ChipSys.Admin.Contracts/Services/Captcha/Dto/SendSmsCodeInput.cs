using System.ComponentModel.DataAnnotations;
using ChipSys.Plugin.Lazy.SlideCaptcha.Core.Validator;

namespace ChipSys.Admin.Services.Captcha.Dto;

/// <summary>
/// ���Ͷ�����֤��
/// </summary>
public class SendSmsCodeInput
{
    /// <summary>
    /// �ֻ���
    /// </summary>
    [Required(ErrorMessage = "�������ֻ���")]
    public string Mobile { get; set; }

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
