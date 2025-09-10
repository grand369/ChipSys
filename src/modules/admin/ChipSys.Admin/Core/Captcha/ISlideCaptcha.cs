using ChipSys.Plugin.Lazy.SlideCaptcha.Core.Validator;
using ChipSys.Plugin.Lazy.SlideCaptcha.Core;

namespace ChipSys.Admin.Core.Captcha
{
    public interface ISlideCaptcha
    {
        /// <summary>
        /// У��
        /// </summary>
        /// <param name="captchaId">��֤��id</param>
        /// <param name="slideTrack">�����켣</param>
        /// <param name="removeIfSuccess">У��ɹ�ʱ�Ƿ��Ƴ�����(���ڶ����֤)</param>
        /// <returns></returns>
        ValidateResult Validate(string captchaId, SlideTrack slideTrack, bool removeIfSuccess = true);
    }
}
