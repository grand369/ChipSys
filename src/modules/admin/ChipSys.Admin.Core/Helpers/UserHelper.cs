using ChipSys.Admin.Core.Attributes;
using ChipSys.Common.Helpers;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Resources;

namespace ChipSys.Admin.Core.Helpers;

/// <summary>
/// 用户帮助类
/// </summary>
[InjectSingleton]
public class UserHelper(AdminCoreLocalizer adminCoreLocalizer)
{
    /// <summary>
    /// 检查密码
    /// </summary>
    /// <param name="password"></param>
    public void CheckPassword(string password)
    {
        if (!PasswordHelper.Verify(password))
        {
            throw ResultOutput.Exception(adminCoreLocalizer["密码为字母+数字+可选特殊字符，长度在6-16之间"]);
        }
    }
}
