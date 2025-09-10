using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DotNetCore.CAP;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Captcha;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Services.Captcha.Dto;
using ChipSys.Admin.Services.Email.Events;
using ChipSys.Common.Helpers;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Plugin.Lazy.SlideCaptcha.Core;
using ChipSys.Plugin.Lazy.SlideCaptcha.Core.Validator;
using static ChipSys.Plugin.Lazy.SlideCaptcha.Core.ValidateResult;
using ChipSys.Admin.Resources;

namespace ChipSys.Admin.Services.Cache;

/// <summary>
/// ��֤�����
/// </summary>
[Order(210)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class CaptchaService : BaseService, IDynamicApi
{
    private readonly ICaptcha _captcha;
    private readonly ISlideCaptcha _slideCaptcha;
    private readonly ICapPublisher _capPublisher;
    private readonly AdminLocalizer _adminLocalizer;

    public CaptchaService(ICaptcha captcha, 
        ISlideCaptcha slideCaptcha, 
        ICapPublisher capPublisher,
        AdminLocalizer adminLocalizer)
    {
        _captcha = captcha;
        _slideCaptcha = slideCaptcha;
        _capPublisher = capPublisher;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="captchaId">��֤��id</param>
    /// <returns></returns>
    [AllowAnonymous]
    [NoOperationLog]
    public CaptchaData Generate(string captchaId = null)
    {
        return _captcha.Generate(captchaId);
    }

    /// <summary>
    /// ��֤
    /// </summary>
    /// <param name="captchaId">��֤��Id</param>
    /// <param name="track">�����켣</param>
    /// <returns></returns>
    [AllowAnonymous]
    [NoOperationLog]
    public ValidateResult CheckAsync([FromQuery] string captchaId, SlideTrack track)
    {
        if (captchaId.IsNull() || track == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["����ɰ�ȫ��֤"]);
        }

        return _slideCaptcha.Validate(captchaId, track, false);
    }

    /// <summary>
    /// ���Ͷ�����֤��
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<string> SendSmsCodeAsync(SendSmsCodeInput input)
    {
        if (input.Mobile.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["�������ֻ���"]);
        }

        if (input.CaptchaId.IsNull() || input.Track == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["����ɰ�ȫ��֤"]);
        }

        var validateResult = _captcha.Validate(input.CaptchaId, input.Track);
        if (validateResult.Result != ValidateResultType.Success)
        {
            throw ResultOutput.Exception(_adminLocalizer["��ȫ{0}", validateResult.Message]);
        }

        var codeId = input.CodeId.IsNull() ? Guid.NewGuid().ToString() : input.CodeId;
        var code = StringHelper.GenerateRandomNumber();
        await Cache.SetAsync(CacheKeys.GetSmsCodeKey(input.Mobile, codeId), code, TimeSpan.FromMinutes(5));

        //���Ͷ�����֤��
        await _capPublisher.PublishAsync(SubscribeNames.SmsSendCode, new
        {
            input.Mobile,
            Text = code
        });

        return codeId;
    }

    /// <summary>
    /// �����ʼ���֤��
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<string> SendEmailCodeAsync(SendEmailCodeInput input)
    {
        if (input.Email.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["�������ʼ���ַ"]);
        }

        if (input.CaptchaId.IsNull() || input.Track == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["����ɰ�ȫ��֤"]);
        }

        var validateResult = _captcha.Validate(input.CaptchaId, input.Track);
        if (validateResult.Result != ValidateResultType.Success)
        {
            throw ResultOutput.Exception(_adminLocalizer["��ȫ{0}", validateResult.Message]);
        }

        var codeId = input.CodeId.IsNull() ? Guid.NewGuid().ToString() : input.CodeId;
        var code = StringHelper.GenerateRandomNumber();
        await Cache.SetAsync(CacheKeys.GetEmailCodeKey(input.Email, codeId), code, TimeSpan.FromMinutes(5));

        //����������֤��
        await _capPublisher.PublishAsync(SubscribeNames.EmailSendCode, new EmailSendCodeEvent
        {
            ToEmail = new EmailSendCodeEvent.Models.EmailModel
            {
                Address = input.Email,
            },
            Code = code,
        });

        return codeId;
    }
}
