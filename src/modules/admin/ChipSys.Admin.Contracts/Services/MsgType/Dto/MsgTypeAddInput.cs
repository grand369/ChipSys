namespace ChipSys.Admin.Services.MsgType.Dto;

/// <summary>
/// ���
/// </summary>
public class MsgTypeAddInput
{
    /// <summary>
    /// ����Id
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
    /// ����
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    public string Description { get; set; }
}
