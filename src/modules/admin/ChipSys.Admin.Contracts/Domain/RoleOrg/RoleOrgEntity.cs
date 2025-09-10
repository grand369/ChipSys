using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Entities;
using ChipSys.Admin.Domain.Org;
using ChipSys.Admin.Domain.Role;

namespace ChipSys.Admin.Domain;

/// <summary>
/// ��ɫ����
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "role_org", OldName = DbConsts.TableOldNamePrefix + "role_org")]
[Index("idx_{tablename}_01", nameof(RoleId) + "," + nameof(OrgId), true)]
public partial class RoleOrgEntity : EntityAdd
{
    /// <summary>
    /// ��ɫId
    /// </summary>
	[Column(IsPrimary = true)]
    public long RoleId { get; set; }

    /// <summary>
    /// ��ɫ
    /// </summary>
    public RoleEntity Role { get; set; }

    /// <summary>
    /// ����Id
    /// </summary>
	[Column(IsPrimary = true)]
    public long OrgId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public OrgEntity Org { get; set; }
}
