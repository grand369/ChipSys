using System.Text;

namespace ChipSys.Common.Helpers;

/// <summary>
/// �ַ���������
/// </summary>
public class StringHelper
{
    private static readonly string _chars = "0123456789";
    private static readonly char[] _constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

    /// <summary>
    /// ��������ַ�����Ĭ��32λ
    /// </summary>
    /// <param name="length">���������</param>
    /// <returns></returns>
    public static string GenerateRandom(int length = 32)
    {
        var newRandom = new StringBuilder();
        var rd = new Random();
        for (int i = 0; i < length; i++)
        {
            newRandom.Append(_constant[rd.Next(_constant.Length)]);
        }
        return newRandom.ToString();
    }

    /// <summary>
    /// �������6λ��
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string GenerateRandomNumber(int length = 6)
    {
        var random = new Random();
        return new string(Enumerable.Repeat(_chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
