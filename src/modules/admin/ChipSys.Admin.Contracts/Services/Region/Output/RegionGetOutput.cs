namespace ChipSys.Admin.Services.Region;

/// <summary>
/// ����
/// </summary>
public class RegionGetOutput : RegionUpdateInput
{
    /// <summary>
    /// �ϼ�Id�б�
    /// </summary>
    public List<long> ParentIdList { get; set; }
}
