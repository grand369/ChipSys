using ChipSys.Admin.Domain.Region;

namespace ChipSys.Admin.Services.Region;

/// <summary>
/// ������ҳ����
/// </summary>
public class RegionGetPageInput
{
    /// <summary>
    /// �ϼ�Id
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public RegionLevel? Level { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool? Hot { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool? Enabled { get; set; }
}
