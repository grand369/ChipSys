namespace ChipSys.Admin.Services.Region;

/// <summary>
/// ��������
/// </summary>
public class RegionSetEnableInput
{
    /// <summary>
    /// ����Id
    /// </summary>
    public long RegionId { get; set; }

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool Enabled { get; set; }
}
