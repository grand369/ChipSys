using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using System.Text;

namespace ChipSys.Common.Helpers;
/// <summary>
/// ���ܹ����࣬�ṩ����SM2�㷨�ļ��ܺͽ��ܹ��ܡ�  
/// </summary>
public class SM2Encryption
{
    /// <summary>
    /// ��ȡSM2���߲���
    /// </summary>
    private static readonly X9ECParameters CurveParams = ECNamedCurveTable.GetByName("sm2p256v1");
    /// <summary>
    /// �����
    /// </summary>
    private static readonly SecureRandom secureRandom = new SecureRandom();
    /// <summary>  
    /// ����SM2��Կ�ԣ���Կ��ʹ��Base64���б���  
    /// </summary>  
    /// <param name="privateKey">�����˽Կ��Base64���룩</param>  
    /// <param name="publicKey">����Ĺ�Կ��Base64���룩</param>  
    public static void GenerateSM2KeyPair(out string privateKey, out string publicKey)
    {
        try
        {
            // ����SM2��Կ���������Ĳ���  
            ECKeyGenerationParameters keyGenParams = new ECKeyGenerationParameters(new ECDomainParameters(CurveParams), secureRandom);

            // ��������ʼ��SM2��Կ��������  
            ECKeyPairGenerator generator = new ECKeyPairGenerator();
            generator.Init(keyGenParams);

            // ������Կ��  
            AsymmetricCipherKeyPair keyPair = generator.GenerateKeyPair();

            // ��ȡ˽Կ����  
            ECPrivateKeyParameters privateKeyParams = (ECPrivateKeyParameters)keyPair.Private;
            // ˽Կ����˽Կֵת��Ϊ�޷����ֽ����鲢����ΪBase64�ַ���  
            privateKey = Base64.ToBase64String(privateKeyParams.D.ToByteArrayUnsigned());

            // ��ȡ��Կ����  
            ECPublicKeyParameters publicKeyParams = (ECPublicKeyParameters)keyPair.Public;
            // ��Կ������Կ�����Ϊѹ����ʽ�������Ҫ��������ΪBase64�ַ���  
            // ע�⣺SM2��Կͨ��ʹ��δѹ����ʽ������ʹ��δѹ����ʽ  
            publicKey = Base64.ToBase64String(publicKeyParams.Q.GetEncoded(false)); // false ��ʾδѹ����ʽ  
        }
        catch (Exception ex)
        {
            // �����쳣��������Լ�¼��־���׳���������쳣  
            throw new Exception("Error generating SM2 key pair.", ex);
        }
    }

    /// <summary>  
    /// SM2 ��Կ����  
    /// </summary>  
    /// <param name="message">�����ܵ���Ϣ</param>  
    /// <param name="publicKey">SM2��Կ��Base64���룩</param>  
    /// <returns>���ܺ�����ģ�Base64���룩</returns>  
    public static string Encrypt(string message, string publicKey)
    {
        try
        {
            // ���빫Կ  
            byte[] keyBytes = Base64.Decode(publicKey);

            ECPoint q = CurveParams.Curve.DecodePoint(keyBytes);
            //������Կ����
            ECDomainParameters domainParams = new ECDomainParameters(CurveParams);
            ECPublicKeyParameters pubKeyParams = new ECPublicKeyParameters("EC", q, domainParams);

            // ����SM2�������沢��ʼ��  
            SM2Engine sm2Engine = new SM2Engine();
            sm2Engine.Init(true, new ParametersWithRandom(pubKeyParams,secureRandom));

            // ��ԭʼ����ת��Ϊ�ֽ�����  
            byte[] dataBytes = Encoding.UTF8.GetBytes(message);

            // ִ�м��ܲ���  
            // ע�⣺SM2����ͨ�����ڼ��̶ܹ����ȵ����ݿ飨����ECCiphertext�����������Ǽ�����Ϣ�����ʺ�ֱ�Ӽ���  
            byte[] encryptedData = sm2Engine.ProcessBlock(dataBytes, 0, dataBytes.Length);

            // �����ܽ��ת��ΪBase64�ַ���  
            return Base64.ToBase64String(encryptedData);
        }
        catch (Exception ex)
        {
            // �����쳣�����繫Կ����ʧ�ܻ���ܲ�������  
            throw new Exception("Error encrypting message with SM2 public key.", ex);
        }
    }

    /// <summary>  
    /// ʹ��SM2˽Կ������Ϣ  
    /// </summary>  
    /// <param name="ciphertext">�����ܵ����ģ�Base64���룩</param>  
    /// <param name="privateKey">SM2˽Կ��Base64���룩</param>  
    /// <returns>���ܺ������</returns>  
    public static string Decrypt(string ciphertext, string privateKey)
    {
        try
        {
            // ����˽Կ  
            byte[] keyBytes = Base64.Decode(privateKey);
            BigInteger d = new BigInteger(1, keyBytes);

            // ��ȡSM2���߲���  
            ECDomainParameters domainParams = new ECDomainParameters(CurveParams);

            // ����˽Կ����  
            ECPrivateKeyParameters privateKeyParams = new ECPrivateKeyParameters(d, domainParams);

            // ����SM2�������沢��ʼ��  
            SM2Engine sm2Engine = new SM2Engine();
            sm2Engine.Init(false, privateKeyParams);

            // ��������  
            byte[] encryptedData = Base64.Decode(ciphertext);

            // ִ�н��ܲ���  
            byte[] decryptedData = sm2Engine.ProcessBlock(encryptedData, 0, encryptedData.Length);

            // �����ܽ��ת��Ϊ�ַ���  
            return Encoding.UTF8.GetString(decryptedData);
        }
        catch (Exception ex)
        {
            // �����쳣������˽Կ����ʧ�ܡ����Ľ���ʧ�ܻ���ܲ�������  
            throw new Exception("Error decrypting ciphertext with SM2 private key.", ex);
        }
    }
}



