namespace ChipSys.Admin.Services.Dict.Dto;

/// <summary>
/// ×Öµä·ÖÒ³ÏìÓ¦
/// </summary>
public class DictGetPageOutput
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
    /// ×ÖµäÖµ
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// ÆôÓÃ
    /// </summary>
	public bool Enabled { get; set; }

    /// <summary>
    /// ÅÅĞò
    /// </summary>
    public int Sort { get; set; }
}
