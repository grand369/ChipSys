namespace ChipSys.Plugin.Lazy.SlideCaptcha.Core;

public class ValidateResult
{
    public ValidateResultType Result { get; set; }
    public string Message { get; set; }

    public static ValidateResult Success()
    {
        return new ValidateResult { Result = ValidateResultType.Success, Message = "�ɹ�" };
    }

    public static ValidateResult Fail()
    {
        return new ValidateResult { Result = ValidateResultType.ValidateFail, Message = "��֤ʧ��" };
    }

    public static ValidateResult Timeout()
    {
        return new ValidateResult { Result = ValidateResultType.Timeout, Message = "��֤��ʱ" };
    }

    public enum ValidateResultType
    {
        Success = 0,
        ValidateFail = 1,
        Timeout = 2
    }
}
