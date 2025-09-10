using ChipSys.Admin.Core.Attributes;
using ChipSys.Common.Helpers;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Resources;

namespace ChipSys.Admin.Core.Helpers;

/// <summary>
/// �û�������
/// </summary>
[InjectSingleton]
public class UserHelper(AdminCoreLocalizer adminCoreLocalizer)
{
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="password"></param>
    public void CheckPassword(string password)
    {
        if (!PasswordHelper.Verify(password))
        {
            throw ResultOutput.Exception(adminCoreLocalizer["����Ϊ��ĸ+����+��ѡ�����ַ���������6-16֮��"]);
        }
    }
}
