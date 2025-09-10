namespace ChipSys.Admin.Services.Org.Input;

/// <summary>
/// ���
/// </summary>
public class OrgAddInput
{
    /// <summary>
    /// ����
    /// </summary>
	public long ParentId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// ֵ
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// ����
    /// </summary>
	public int Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Description { get; set; }
}
