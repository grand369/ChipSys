using ChipSys.Admin.Domain.Role;

namespace ChipSys.Admin.Services.Role.Dto;

/// <summary>
/// ��ɫ�б���Ӧ
/// </summary>
public class RoleGetListOutput
{
    /// <summary>
    /// ����
    /// </summary>
    public long Id { get; set; }

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
    /// ��ɫ����
    /// </summary>
    public RoleType Type { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Description { get; set; }
}
