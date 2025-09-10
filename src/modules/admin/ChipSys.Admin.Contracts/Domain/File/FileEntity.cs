using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Entities;
using ChipSys.Admin.Core.Attributes;
using OnceMi.AspNetCore.OSS;

namespace ChipSys.Admin.Domain;

/// <summary>
/// �ļ�
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "file", OldName = DbConsts.TableOldNamePrefix + "file")]
public partial class FileEntity : EntityBase
{
    /// <summary>
    /// OSS��Ӧ��
    /// </summary>
    [Column(MapType = typeof(string), StringLength = 50)]
    public OSSProvider? Provider { get; set; }

    /// <summary>
    /// �洢Ͱ����
    /// </summary>
    [Column(StringLength = 200)]
    public string BucketName { get; set; }

    /// <summary>
    /// �ļ�Ŀ¼
    /// </summary>
    [Column(StringLength = 500)]
    public string FileDirectory { get; set; }

    /// <summary>
    /// �ļ�Guid
    /// </summary>
    [OrderGuid]
    public Guid FileGuid { get; set; }

    /// <summary>
    /// �����ļ���
    /// </summary>
    [Column(StringLength = 200)]
    public string SaveFileName { get; set; }

    /// <summary>
    /// �ļ���
    /// </summary>
    [Column(StringLength = 200)]
    public string FileName { get; set; }

    /// <summary>
    /// �ļ���չ��
    /// </summary>
    [Column(StringLength = 20)]
    public string Extension { get; set; }

    /// <summary>
    /// �ļ��ֽڳ���
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// �ļ���С��ʽ��
    /// </summary>
    [Column(StringLength = 50)]
    public string SizeFormat { get; set; }

    /// <summary>
    /// ���ӵ�ַ
    /// </summary>
    [Column(StringLength = 500)]
    public string LinkUrl { get; set; }

    /// <summary>
    /// md5�룬��ֹ�ϴ��ظ��ļ�
    /// </summary>
    [Column(StringLength = 50)]
    public string Md5 { get; set; } = string.Empty;
}
