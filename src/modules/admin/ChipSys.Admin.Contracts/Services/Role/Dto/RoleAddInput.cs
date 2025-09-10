using System.Text.Json.Serialization;
using ChipSys.Admin.Domain.Org;
using ChipSys.Admin.Domain.Role;

namespace ChipSys.Admin.Services.Role.Dto;

/// <summary>
/// ���
/// </summary>
public class RoleAddInput
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
    /// ��ɫ����
    /// </summary>
    public RoleType Type { get; set; }

    /// <summary>
    /// ���ݷ�Χ
    /// </summary>
    public DataScope DataScope { get; set; } = DataScope.All;

    /// <summary>
    /// ָ������
    /// </summary>
    public long[] OrgIds { get; set; }

    /// <summary>
    /// �����б�
    /// </summary>
    [JsonIgnore]
    public ICollection<OrgEntity> Orgs { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public int Sort { get; set; }
}
