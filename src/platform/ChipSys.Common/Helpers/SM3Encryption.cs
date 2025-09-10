using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities.Encoders;
using System.Text;

namespace ChipSys.Common.Helpers;

/// <summary>
/// �ṩ����SM3�㷨�Ĺ�ϣ�����HMAC���ܡ�
/// </summary>
public static class SM3Encryption
{
    /// <summary>
    /// ����������ݵ�SM3��ϣֵ�������Ʊ�ʾ����
    /// </summary>
    /// <param name="data">Ҫ�����ϣ�����ݣ��ַ�����ʽ����</param>
    /// <returns>SM3��ϣֵ�Ķ��������顣</returns>
    public static byte[] ComputeSM3Hash(string data)
    {
        var msg = Encoding.UTF8.GetBytes(data); // ʹ��UTF8���뽫�ַ���ת��Ϊ�ֽ�����
        var sm3 = new SM3Digest();
        sm3.BlockUpdate(msg, 0, msg.Length);
        var md = new byte[sm3.GetDigestSize()]; // SM3�㷨�����Ĺ�ϣֵ��С
        sm3.DoFinal(md, 0);
        return md;
    }

    /// <summary>
    /// ����������ݵ�SM3��ϣֵ��ʮ�����Ʊ�ʾ����
    /// </summary>
    /// <param name="data">Ҫ�����ϣ�����ݣ��ַ�����ʽ����</param>
    /// <returns>SM3��ϣֵ��ʮ�������ַ�����</returns>
    public static string ComputeSM3HashHex(string data)
    {
        return Hex.ToHexString(ComputeSM3Hash(data));
    }
    /// <summary>
    /// ����������ݵ�SM3��ϣֵ��ʮ�����Ʊ�ʾ����
    /// </summary>
    /// <param name="data">Ҫ�����ϣ�����ݣ��ַ�����ʽ����</param>
    /// <returns>SM3��ϣֵ��ʮ�������ַ�����</returns>
    public static string ComputeSM3HashBase64(string data)
    {
        return Base64.ToBase64String(ComputeSM3Hash(data));
    }

    /// <summary>
    /// ����������ݺ���Կ��HMAC-SM3ֵ�������Ʊ�ʾ����
    /// </summary>
    /// <param name="data">Ҫ����HMAC�����ݣ��ַ�����ʽ����</param>
    /// <param name="key">HMAC��Կ���ַ�����ʽ����</param>
    /// <returns>HMAC-SM3ֵ�Ķ��������顣</returns>
    public static byte[] ComputeHMacSM3(string data, string key)
    {
        var msg = Encoding.UTF8.GetBytes(data);
        var keyBytes = Encoding.UTF8.GetBytes(key);

        var keyParameter = new KeyParameter(keyBytes);
        var sm3 = new SM3Digest();
        var mac = new HMac(sm3); // ����Կ���Ӵ��㷨
        mac.Init(keyParameter);
        mac.BlockUpdate(msg, 0, msg.Length);
        var result = new byte[mac.GetMacSize()];
        mac.DoFinal(result, 0);
        return result;
    }

    /// <summary>
    /// ����������ݺ���Կ��HMAC-SM3ֵ��ʮ�����Ʊ�ʾ����
    /// </summary>
    /// <param name="data">Ҫ����HMAC�����ݣ��ַ�����ʽ����</param>
    /// <param name="key">HMAC��Կ���ַ�����ʽ����</param>
    /// <returns>HMAC-SM3ֵ��ʮ�������ַ�����</returns>
    public static string ComputeHMACSM3Hex(string data, string key)
    {
        return Hex.ToHexString(ComputeHMacSM3(data, key));
    }
  /// <summary>
    /// ����������ݺ���Կ��HMAC-SM3ֵ��Base64��ʾ����
    /// </summary>
    /// <param name="data">Ҫ����HMAC�����ݣ��ַ�����ʽ����</param>
    /// <param name="key">HMAC��Կ���ַ�����ʽ����</param>
    /// <returns>HMAC-SM3ֵ��Base64�ַ�����</returns>
    public static string ComputeHMACSM3Base64(string data, string key)
    {
        return Base64.ToBase64String(ComputeHMacSM3(data, key));
    }
}
