namespace ChipSys.Admin.Services.DictType.Dto;

/// <summary>
/// ×ÖµäÀàĞÍ·ÖÒ³ÏìÓ¦
/// </summary>
public class DictTypeGetPageOutput
{
    /// <summary>
    /// Ö÷¼üId
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// ×ÖµäÃû³Æ
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ×Öµä±àÂë
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// ÆôÓÃ
    /// </summary>
	public bool Enabled { get; set; }

    /// <summary>
    /// ÅÅĞò
    /// </summary>
    public int Sort { get; set; }
}
