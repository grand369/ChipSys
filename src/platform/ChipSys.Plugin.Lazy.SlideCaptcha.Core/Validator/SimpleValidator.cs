namespace ChipSys.Plugin.Lazy.SlideCaptcha.Core.Validator;

/// <summary>
/// ��ʹ��BaseValidator��λ��У��
/// </summary>
public class SimpleValidator : BaseValidator, IValidator
{
    public override bool ValidateCore(SlideTrack slideTrack, CaptchaValidateData captchaValidateData)
    {
        return true;
    }
}
