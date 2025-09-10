using ChipSys.Plugin.Lazy.SlideCaptcha.Core.Validator;

namespace ChipSys.Plugin.Lazy.SlideCaptcha.Core;

public interface ICaptcha
{
    /// <summary>
    /// ������֤��
    /// </summary>
    /// <param name="captchaId">��֤��id</param>
    /// <returns></returns>
    CaptchaData Generate(string captchaId = null);

    /// <summary>
    /// У��
    /// </summary>
    /// <param name="captchaId">��֤��id</param>
    /// <param name="slideTrack">�����켣</param>
    /// <returns></returns>
    ValidateResult Validate(string captchaId, SlideTrack slideTrack);
}
