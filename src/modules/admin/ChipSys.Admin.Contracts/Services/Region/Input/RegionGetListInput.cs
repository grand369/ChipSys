namespace ChipSys.Admin.Services.Region;

/// <summary>
/// �����б�����
/// </summary>
public class RegionGetListInput
{
    /// <summary>
    /// �ϼ�Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool? Hot { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool? Enabled { get; set; }
}
