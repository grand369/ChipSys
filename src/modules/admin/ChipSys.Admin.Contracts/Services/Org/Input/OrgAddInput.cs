namespace ChipSys.Admin.Services.Org.Input;

/// <summary>
/// Ìí¼Ó
/// </summary>
public class OrgAddInput
{
    /// <summary>
    /// ¸¸¼¶
    /// </summary>
	public long ParentId { get; set; }

    /// <summary>
    /// Ãû³Æ
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ±àÂë
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Öµ
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// ÆôÓÃ
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// ÅÅĞò
    /// </summary>
	public int Sort { get; set; }

    /// <summary>
    /// ÃèÊö
    /// </summary>
    public string Description { get; set; }
}
