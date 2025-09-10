using ChipSys.Admin.Domain.Region;

namespace ChipSys.Admin.Services.Region;

/// <summary>
/// ������ҳ��Ӧ
/// </summary>
public class RegionGetPageOutput
{
    /// <summary>
    /// ����Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// �ϼ�Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ���
    /// </summary>
    public string ShortName { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public RegionLevel Level { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// ƴ��
    /// </summary>
    public string Pinyin { get; set; }

    /// <summary>
    /// ƴ������ĸ
    /// </summary>
    public string PinyinFirst { get; set; }

    /// <summary>
    /// פ��
    /// </summary>
    public string Capital { get; set; }

    /// <summary>
    /// �˿ڣ����ˣ�
    /// </summary>
    public int? Population { get; set; }

    /// <summary>
    /// �����ƽ��ǧ�ף�
    /// </summary>
    public int? Area { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string AreaCode { get; set; }

    /// <summary>
    /// �ʱ�
    /// </summary>
    public string ZipCode { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public int? Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Hot { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; }
}
