using OnceMi.AspNetCore.OSS;

namespace ChipSys.Admin.Core.Configs;

/// <summary>
/// OSS����
/// </summary>
public class OSSOptions
{
    /// <summary>
    /// �ļ��洢��Ӧ��
    /// </summary>
    public OSSProvider Provider { get; set; } = OSSProvider.Minio;
    /// <summary>
    /// ����
    /// </summary>
    public string Endpoint { get; set; }
    /// <summary>
    /// �˺�
    /// </summary>
    public string AccessKey { get; set; }
    /// <summary>
    /// ����
    /// </summary>
    public string SecretKey { get; set; }
    /// <summary>
    /// ����
    /// </summary>
    public string Region { get; set; }
    /// <summary>
    /// �ỰToken
    /// </summary>
    public string SessionToken { get; set; }
    /// <summary>
    /// ����Https
    /// </summary>
    public bool IsEnableHttps { get; set; }
    /// <summary>
    /// ���û���
    /// </summary>
    public bool IsEnableCache { get; set; }
    /// <summary>
    /// �洢Ͱ
    /// </summary>
    public string BucketName { get; set; } = "admin";
    /// <summary>
    /// �ļ���ַ
    /// </summary>
    public string Url { get; set; }
    /// <summary>
    /// �ļ�Md5��
    /// </summary>
    public bool Md5 { get; set; } = false;
    /// <summary>
    /// ����
    /// </summary>
    public bool Enable { get; set; } = false;
}

/// <summary>
/// �����ϴ�����
/// </summary>
public class LocalUploadConfig
{
    /// <summary>
    /// �ϴ�Ŀ¼
    /// </summary>
    public string Directory { get; set; } = "upload";

    /// <summary>
    /// ����Ŀ¼
    /// </summary>
    public string DateTimeDirectory { get; set; } = "yyyy/MM/dd";

    /// <summary>
    /// �ļ�Md5��
    /// </summary>
    public bool Md5 { get; set; } = false;

    /// <summary>
    /// �ļ���С
    /// </summary>
    public long MaxSize { get; set; } = 104857600;

    /// <summary>
    /// �����ļ���չ���б�
    /// </summary>
    public string[] IncludeExtension { get; set; }

    /// <summary>
    /// �ų��ļ���չ���б�
    /// </summary>
    public string[] ExcludeExtension { get; set; }
}

/// <summary>
/// OSS����
/// </summary>
public class OSSConfig
{
    /// <summary>
    /// �����ϴ�����
    /// </summary>
    public LocalUploadConfig LocalUploadConfig { get; set; }

    /// <summary>
    /// �ļ��洢��Ӧ��
    /// </summary>
    public OSSProvider Provider { get; set; } = OSSProvider.Minio;

    /// <summary>
    /// OSS�����б�
    /// </summary>
    public List<OSSOptions> OSSConfigs { get; set; }
}
