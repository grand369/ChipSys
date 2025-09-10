namespace ChipSys.Admin.Services.PrintTemplate.Outputs;

/// <summary>
/// ��ҳ��Ӧ
/// </summary>
public class PrintTemplateGetPageOutput
{
    /// <summary>
    /// ��ӡģ��Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Enabled { get; set; } = true;

    /// <summary>
    /// ����
    /// </summary>
	public int Sort { get; set; }

    /// <summary>
    /// �汾
    /// </summary>
    public long Version { get; set; }
}
