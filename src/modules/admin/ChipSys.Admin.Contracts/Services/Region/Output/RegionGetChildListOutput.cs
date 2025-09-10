using ChipSys.Admin.Domain.Region;

namespace ChipSys.Admin.Services.Region;

/// <summary>
/// �¼��б�
/// </summary>
public class RegionGetChildListOutput
{
    /// <summary>
    /// ����Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public RegionLevel Level { get; set; }

    /// <summary>
    /// ƴ��
    /// </summary>
    public string Pinyin { get; set; }

    /// <summary>
    /// ƴ������ĸ
    /// </summary>
    public string PinyinFirst { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Hot { get; set; }

    /// <summary>
    /// Ҷ�ӽڵ�
    /// </summary>
    public bool Leaf { get => Level >= RegionLevel.Vilage; }
}
