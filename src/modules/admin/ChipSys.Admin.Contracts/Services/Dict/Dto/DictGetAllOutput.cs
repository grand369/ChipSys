namespace ChipSys.Admin.Services.Dict.Dto;

/// <summary>
/// �ֵ��б���Ӧ
/// </summary>
public class DictGetAllOutput
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
    /// �ֵ�����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// �ֵ����
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// �ֵ�ֵ
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
}
