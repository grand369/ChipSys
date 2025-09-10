namespace ChipSys.Admin.Services.Role.Dto;

/// <summary>
/// ��ɫ��ҳ��Ӧ
/// </summary>
public class RoleGetPageOutput
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
    public bool Hidden { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public DateTime? CreatedTime { get; set; }
}
