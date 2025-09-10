using System.Security.Cryptography;
using System.Text;
using ChipSys.Common.Extensions;

namespace ChipSys.Common.Helpers;

/// <summary>
/// Des�ӽ���
/// </summary>
public class DesEncrypt
{
    private const string Key = "desenc1!";

    /// <summary>
    /// DES+Base64����
    /// <para>����ECB��PKCS7</para>
    /// </summary>
    /// <param name="encryptString">�����ַ���</param>
    /// <param name="key">��Կ</param>
    /// <returns></returns>
    public static string Encrypt(string encryptString, string key = null)
    {
        return Encrypt(encryptString, key, false, true);
    }

    /// <summary>
    /// DES+Base64����
    /// <para>����ECB��PKCS7</para>
    /// </summary>
    /// <param name="decryptString">�����ַ���</param>
    /// <param name="key">��Կ</param>
    /// <returns></returns>
    public static string Decrypt(string decryptString, string key = null)
    {
        return Decrypt(decryptString, key, false);
    }

    /// <summary>
    /// DES+16���Ƽ���
    /// <para>����ECB��PKCS7</para>
    /// </summary>
    /// <param name="encryptString">�����ַ���</param>
    /// <param name="key">��Կ</param>
    /// <param name="lowerCase">�Ƿ�Сд</param>
    /// <returns></returns>
    public static string Encrypt4Hex(string encryptString, string key = null, bool lowerCase = false)
    {
        return Encrypt(encryptString, key, true, lowerCase);
    }

    /// <summary>
    /// DES+16���ƽ���
    /// <para>����ECB��PKCS7</para>
    /// </summary>
    /// <param name="decryptString">�����ַ���</param>
    /// <param name="key">��Կ</param>
    /// <returns></returns>
    public static string Decrypt4Hex(string decryptString, string key = null)
    {
        return Decrypt(decryptString, key, true);
    }

    /// <summary>
    /// DES����
    /// </summary>
    /// <param name="encryptString"></param>
    /// <param name="key"></param>
    /// <param name="hex"></param>
    /// <param name="lowerCase"></param>
    /// <returns></returns>
    private static string Encrypt(string encryptString, string key, bool hex, bool lowerCase = false)
    {
        if (encryptString.IsNull())
            return null;
        if (key.IsNull())
            key = Key;
        if (key.Length < 8)
            throw new ArgumentException("��Կ����Ϊ8λ", nameof(key));

        var keyBytes = Encoding.UTF8.GetBytes(key[..8]);
        var inputByteArray = Encoding.UTF8.GetBytes(encryptString);

        var des = DES.Create();
        des.Mode = CipherMode.ECB;
        des.Key = keyBytes;
        des.Padding = PaddingMode.PKCS7;

        using var stream = new MemoryStream();
        var cStream = new CryptoStream(stream, des.CreateEncryptor(), CryptoStreamMode.Write);
        cStream.Write(inputByteArray, 0, inputByteArray.Length);
        cStream.FlushFinalBlock();

        var bytes = stream.ToArray();
        return hex ? bytes.ToHex(lowerCase) : bytes.ToBase64();
    }

    /// <summary>
    /// DES����
    /// </summary>
    /// <param name="decryptString"></param>
    /// <param name="key"></param>
    /// <param name="hex"></param>
    /// <returns></returns>
    private static string Decrypt(string decryptString, string key, bool hex)
    {
        if (decryptString.IsNull())
            return null;
        if (key.IsNull())
            key = Key;
        if (key.Length < 8)
            throw new ArgumentException("��Կ����Ϊ8λ", nameof(key));

        var keyBytes = Encoding.UTF8.GetBytes(key[..8]);
        var inputByteArray = hex ? decryptString.HexToBytes() : Convert.FromBase64String(decryptString);

        var des = DES.Create();
        des.Mode = CipherMode.ECB;
        des.Key = keyBytes;
        des.Padding = PaddingMode.PKCS7;

        using var mStream = new MemoryStream();
        var cStream = new CryptoStream(mStream, des.CreateDecryptor(), CryptoStreamMode.Write);
        cStream.Write(inputByteArray, 0, inputByteArray.Length);
        cStream.FlushFinalBlock();
        return Encoding.UTF8.GetString(mStream.ToArray());
    }
}
