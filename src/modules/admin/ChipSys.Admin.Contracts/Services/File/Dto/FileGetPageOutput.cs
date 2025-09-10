namespace ChipSys.Admin.Services.Dto;

/// <summary>
/// �ļ���ҳ��Ӧ
/// </summary>
public class FileGetPageOutput
{
    /// <summary>
    /// �ļ�Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// OSS��Ӧ��
    /// </summary>
    public string ProviderName { get; set; }

    /// <summary>
    /// �洢Ͱ����
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// �ļ�Ŀ¼
    /// </summary>
    public string FileDirectory { get; set; }

    /// <summary>
    /// �ļ�Guid
    /// </summary>
    public Guid FileGuid { get; set; }

    /// <summary>
    /// �ļ���
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// �ļ���չ��
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// �ļ���С��ʽ��
    /// </summary>
    public string SizeFormat { get; set; }

    /// <summary>
    /// ���ӵ�ַ
    /// </summary>
    public string LinkUrl { get; set; }

    /// <summary>
    /// ������
    /// </summary>
    public string CreatedUserName { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// �޸���
    /// </summary>
    public string ModifiedUserName { get; set; }

    /// <summary>
    /// �޸�ʱ��
    /// </summary>
    public DateTime? ModifiedTime { get; set; }
}
