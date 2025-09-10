namespace ChipSys.Admin.Services.Pkg.Dto;

/// <summary>
/// �ײͷ�ҳ��Ӧ
/// </summary>
public class PkgGetPageOutput
{
    /// <summary>
    /// ����
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
    public int Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Enabled { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public DateTime? CreatedTime { get; set; }
}
