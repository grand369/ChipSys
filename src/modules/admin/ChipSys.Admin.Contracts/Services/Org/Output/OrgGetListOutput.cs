namespace ChipSys.Admin.Services.Org.Output;

/// <summary>
/// �����б�
/// </summary>
public class OrgGetListOutput
{
    /// <summary>
    /// ����Id
    /// </summary>
    public long Id { get; set; }

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
	public bool Enabled { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public int Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public DateTime? CreatedTime { get; set; }
}
