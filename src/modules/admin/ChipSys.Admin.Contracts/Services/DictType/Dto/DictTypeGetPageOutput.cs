namespace ChipSys.Admin.Services.DictType.Dto;

/// <summary>
/// �ֵ����ͷ�ҳ��Ӧ
/// </summary>
public class DictTypeGetPageOutput
{
    /// <summary>
    /// ����Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// �ֵ�����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// �ֵ����
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Enabled { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public int Sort { get; set; }
}
