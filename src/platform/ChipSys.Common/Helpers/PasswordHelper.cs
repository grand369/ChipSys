using System.Text.RegularExpressions;

namespace ChipSys.Common.Helpers;

/// <summary>
/// ���������
/// </summary>
public partial class PasswordHelper
{
   
    // ��֤�����������ʽ  
    public static readonly string PasswordRegex = @"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d!@#$%^&.*]{6,16}$";

    /// <summary>  
    /// ��֤�����Ƿ����Ҫ��  
    /// </summary>  
    /// <param name="input"></param>  
    /// <returns></returns>  
    public static bool Verify(string input)
    {
        if (input.IsNull())
        {
            return false;
        }

        return Regex.IsMatch(input, PasswordRegex);
    }
}
