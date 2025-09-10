using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using System.Security.Cryptography;
using System.Text;

namespace ChipSys.Common.Helpers;

/// <summary>  
/// ���ܹ����࣬�ṩ����SM4�㷨�ļ��ܺͽ��ܹ��ܡ�  
/// </summary>  
public static class SM4Encryption
{

    /// <summary>  
    /// ʹ��SM4�㷨��ָ�����Ľ��м��ܡ�  
    /// </summary>  
    /// <param name="msg">�����ܵ����ġ�</param>  
    /// <param name="key">���ڼ��ܵ���Կ��</param>  
    /// <param name="iv">��ʼ������������ECBģʽ���Դ���null��</param>  
    /// <param name="mode">����ģʽ��Ĭ��Ϊ"ECB"��</param>  
    /// <param name="isHex">�Ƿ񷵻�ʮ�����Ƹ�ʽ�����ģ�Ĭ��Ϊfalse������Base64��ʽ����</param>  
    /// <returns>���ܺ�����ģ���ʽΪʮ�����ƻ�Base64��</returns>  
    /// <exception cref="ArgumentNullException">��msg��keyΪ��ʱ�׳���</exception>  
    /// <exception cref="ArgumentException">��mode����֧��ʱ�׳���</exception>  
    /// <exception cref="CryptographicException">�����ܹ����з�������ʱ�׳���</exception>  
    public static string Encrypt(string msg, string key, string iv, string mode = "ECB", bool isHex = false)
    {
        // ���ܲ���  
        // ��֤�������  
        if (string.IsNullOrEmpty(msg))
            throw new ArgumentNullException(nameof(msg), "Message cannot be null or empty.");
        if (key == null || key.Length == 0)
            throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");
        if (iv == null && mode != "ECB")
            throw new ArgumentNullException(nameof(iv), "IV is required for modes other than ECB.");
        byte[] plainTextData = Encoding.UTF8.GetBytes(msg);
        var cipher = new SM4Engine();
        byte[] nonce = new byte[16];
        Array.Copy(Encoding.UTF8.GetBytes(iv), 0, nonce, 0, Math.Min(iv.Length, nonce.Length));
        PaddedBufferedBlockCipher cipherMode;
        switch (mode)
        {
            case "ECB":
                cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), new Pkcs7Padding());
                break;
            case "CBC":
                cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), new Pkcs7Padding());
                break;
            // ... [����ģʽ�����] ...  
            default:
                throw new ArgumentException("Unsupported mode: " + mode);
        }

        byte[] keyBytes = Encoding.UTF8.GetBytes(key); // �������Կ��ʵ����Ӧ��ʹ�ð�ȫ�ķ�ʽ���ɺʹ洢  
        KeyParameter keyParam = ParameterUtilities.CreateKeyParameter("SM4", keyBytes);
        ICipherParameters keyParamIV = new ParametersWithIV(keyParam, nonce);

        cipherMode.Init(true, mode == "ECB" ? keyParam : keyParamIV);

        byte[] cipherTextData = new byte[cipherMode.GetOutputSize(plainTextData.Length)];
        int length1 = cipherMode.ProcessBytes(plainTextData, 0, plainTextData.Length, cipherTextData, 0);
        cipherMode.DoFinal(cipherTextData, length1);
        return isHex == true ? Hex.ToHexString(cipherTextData) : Convert.ToBase64String(cipherTextData);
    }
    /// <summary>  
    /// ʹ��SM4�㷨��ָ�����Ľ��н��ܡ�  
    /// </summary>  
    /// <param name="encryptMsg">�����ܵ����ģ�������ʮ�����ƻ�Base64��ʽ��</param>  
    /// <param name="key">���ڽ��ܵ���Կ��</param>  
    /// <param name="iv">��ʼ������������ECBģʽ���Դ���null��</param>  
    /// <param name="mode">����ģʽ������ȷ������ʱʹ�õ�ģʽ��Ĭ��Ϊ"ECB"��</param>  
    /// <param name="isHex">�Ƿ�����������ʮ�����Ƹ�ʽ��Ĭ��Ϊfalse����ʾBase64��ʽ����</param>  
    /// <returns>���ܺ�����ġ�</returns>  
    /// <exception cref="ArgumentNullException">��key��encryptMsgΪ��ʱ�׳���</exception>  
    /// <exception cref="ArgumentException">��mode����֧�ֻ�iv���ڷ�ECBģʽΪ��ʱ�׳���</exception>  
    /// <exception cref="CryptographicException">�����ܹ����з�������ʱ�׳���</exception>  
    public static string Decrypt(string encryptMsg, string key, string iv, string mode = "ECB", bool isHex = false)
    {
        // ���ܲ���  
        // ��֤�������  
        if (string.IsNullOrEmpty(encryptMsg))
            throw new ArgumentNullException(nameof(encryptMsg), "Message cannot be null or empty.");
        if (key == null || key.Length == 0)
            throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");
        if (iv == null && mode != "ECB")
            throw new ArgumentNullException(nameof(iv), "IV is required for modes other than ECB.");
        byte[] cipherTextData;
        if (isHex)
        {
            cipherTextData = Hex.Decode(encryptMsg);
        }
        else
        {
            cipherTextData = Convert.FromBase64String(encryptMsg);
        }
        var cipher = new SM4Engine();
        byte[] nonce = new byte[16];
        Array.Copy(Encoding.UTF8.GetBytes(iv), 0, nonce, 0, Math.Min(iv.Length, nonce.Length));
        PaddedBufferedBlockCipher cipherMode;
        switch (mode)
        {
            case "ECB":
                cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), new Pkcs7Padding());
                break;
            case "CBC":
                cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), new Pkcs7Padding());
                break;
            // ... [����ģʽ�����] ...  
            default:
                throw new ArgumentException("Unsupported mode: " + mode);
        }

        byte[] keyBytes = Encoding.UTF8.GetBytes(key); // �������Կ��ʵ����Ӧ��ʹ�ð�ȫ�ķ�ʽ���ɺʹ洢  
        KeyParameter keyParam = ParameterUtilities.CreateKeyParameter("SM4", keyBytes);
        ICipherParameters keyParamIV = new ParametersWithIV(keyParam, nonce);
        // ���ܲ���  

        cipherMode.Init(false, mode == "ECB" ? keyParam : keyParamIV);

        byte[] decryptedData = new byte[cipherMode.GetOutputSize(cipherTextData.Length)];
        int length2 = cipherMode.ProcessBytes(cipherTextData, 0, cipherTextData.Length, decryptedData, 0);
        cipherMode.DoFinal(decryptedData, length2);

        // ��ӡ���ܺ������  
        string decryptedMsg = Encoding.UTF8.GetString(decryptedData);
        return decryptedMsg;
    }

    /// <summary>  
    /// ʹ��SM4�㷨��ָ�����Ľ��м��ܡ�  
    /// </summary>  
    /// <param name="msg">�����ܵ����ġ�</param>  
    /// <param name="key">���ڼ��ܵ���Կ���ֽ�������ʽ����</param>  
    /// <param name="iv">��ʼ���������ֽ�������ʽ��������ECBģʽ���Դ���null��</param>  
    /// <param name="mode">����ģʽ��Ĭ��Ϊ"ECB"��</param>  
    /// <param name="isHex">�Ƿ񷵻�ʮ�����Ƹ�ʽ�����ģ�Ĭ��Ϊfalse������Base64��ʽ����</param>  
    /// <returns>���ܺ�����ģ���ʽΪʮ�����ƻ�Base64��</returns>  
    /// <exception cref="ArgumentNullException">��msg��keyΪ��ʱ�׳���</exception>  
    /// <exception cref="ArgumentException">��mode����֧��ʱ�׳���</exception>  
    /// <exception cref="CryptographicException">�����ܹ����з�������ʱ�׳���</exception>  
    public static string Encrypt(string msg, byte[] key, byte[] iv, string mode = "ECB", bool isHex = false)
    {
        // ��֤�������  
        if (string.IsNullOrEmpty(msg))
            throw new ArgumentNullException(nameof(msg), "Message cannot be null or empty.");
        if (key == null || key.Length == 0)
            throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");
        if (iv == null && mode != "ECB")
            throw new ArgumentNullException(nameof(iv), "IV is required for modes other than ECB.");

        // ������ת��Ϊ�ֽ�����  
        byte[] plainTextData = Encoding.UTF8.GetBytes(msg);

        var cipher = new SM4Engine();
        PaddedBufferedBlockCipher cipherMode;

        switch (mode)
        {
            case "ECB":
                cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), new Pkcs7Padding());
                break;
            case "CBC":
                cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), new Pkcs7Padding());
                break;
            // ... [����ģʽ�����] ...  
            default:
                throw new ArgumentException("Unsupported mode: " + mode);
        }

        // ������Կ����  
        KeyParameter keyParam = ParameterUtilities.CreateKeyParameter("SM4", key);
        ICipherParameters parameters = mode == "ECB" ? keyParam : new ParametersWithIV(keyParam, iv);

        // ��ʼ��������  
        cipherMode.Init(true, parameters);

        // ׼�����������  
        byte[] cipherTextData = new byte[cipherMode.GetOutputSize(plainTextData.Length)];

        try
        {
            // ִ�м��ܲ���  
            int length1 = cipherMode.ProcessBytes(plainTextData, 0, plainTextData.Length, cipherTextData, 0);
            cipherMode.DoFinal(cipherTextData, length1);

            // �������󷵻ؼ��ܺ�����ݸ�ʽ��ʮ�����ƻ�Base64��  
            return isHex ? Hex.ToHexString(cipherTextData) : Convert.ToBase64String(cipherTextData);
        }
        catch (Exception ex)
        {
            // ������ܹ����з������쳣  
            throw new CryptographicException("Encryption failed.", ex);
        }
    }
    /// <summary>  
    /// ʹ��SM4�㷨��ָ�����Ľ��н��ܡ�  
    /// </summary>  
    /// <param name="encryptMsg">�����ܵ����ģ�������ʮ�����ƻ�Base64��ʽ��</param>  
    /// <param name="key">���ڽ��ܵ���Կ���ֽ�������ʽ����</param>  
    /// <param name="iv">��ʼ���������ֽ�������ʽ��������ECBģʽ���Դ���null��</param>  
    /// <param name="mode">����ģʽ������ȷ������ʱʹ�õ�ģʽ��Ĭ��Ϊ"ECB"��</param>  
    /// <param name="isHex">�Ƿ�����������ʮ�����Ƹ�ʽ��Ĭ��Ϊfalse����ʾBase64��ʽ����</param>  
    /// <returns>���ܺ�����ġ�</returns>  
    /// <exception cref="ArgumentNullException">��key��encryptMsgΪ��ʱ�׳���</exception>  
    /// <exception cref="ArgumentException">��mode����֧�ֻ�iv���ڷ�ECBģʽΪ��ʱ�׳���</exception>  
    /// <exception cref="CryptographicException">�����ܹ����з�������ʱ�׳���</exception>  
    public static string Decrypt(string encryptMsg, byte[] key, byte[] iv, string mode = "ECB", bool isHex = false)
    {
        // ��֤�������  
        if (string.IsNullOrEmpty(encryptMsg))
            throw new ArgumentNullException(nameof(encryptMsg), "Message cannot be null or empty.");
        if (key == null || key.Length == 0)
            throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");
        if (iv == null && mode != "ECB")
            throw new ArgumentNullException(nameof(iv), "IV is required for modes other than ECB.");

        // ת������Ϊ�ֽ�����  
        byte[] cipherTextData;
        if (isHex)
        {
            cipherTextData = Hex.Decode(encryptMsg);
        }
        else
        {
            cipherTextData = Convert.FromBase64String(encryptMsg);
        }

        var cipher = new SM4Engine();
        PaddedBufferedBlockCipher cipherMode;

        switch (mode)
        {
            case "ECB":
                cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), new Pkcs7Padding());
                break;
            case "CBC":
                cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), new Pkcs7Padding());
                break;
            // ... [����ģʽ�����] ...  
            default:
                throw new ArgumentException("Unsupported mode: " + mode);
        }

        // ������Կ����  
        KeyParameter keyParam = ParameterUtilities.CreateKeyParameter("SM4", key);
        ICipherParameters parameters = mode == "ECB" ? keyParam : new ParametersWithIV(keyParam, iv);

        // ��ʼ��������  
        cipherMode.Init(false, parameters);

        // ׼�����������  
        byte[] decryptedData = new byte[cipherMode.GetOutputSize(cipherTextData.Length)];

        try
        {
            // ִ�н��ܲ���  
            int length1 = cipherMode.ProcessBytes(cipherTextData, 0, cipherTextData.Length, decryptedData, 0);
            cipherMode.DoFinal(decryptedData, length1);

            // ת�����ܺ���ֽ�����Ϊ�ַ���  
            string decryptedMsg = Encoding.UTF8.GetString(decryptedData);
            return decryptedMsg;
        }
        catch (Exception ex)
        {
            // ������ܹ����з������쳣  
            throw new CryptographicException("Decryption failed.", ex);
        }
    }
}
