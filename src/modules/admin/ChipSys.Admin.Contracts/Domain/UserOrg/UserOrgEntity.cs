using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Entities;
using ChipSys.Admin.Domain.Org;
using ChipSys.Admin.Domain.User;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Domain.UserOrg;

/// <summary>
/// 用户所属部门
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "user_org", OldName = DbConsts.TableOldNamePrefix + "user_org")]
[Index("idx_{tablename}_01", nameof(UserId) + "," + nameof(OrgId), true)]
public partial class UserOrgEntity : EntityUpdate
{
    /// <summary>
    /// 用户Id
    /// </summary>
    [Column(IsPrimary = true)]
    public long UserId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    [NotGen]
    public UserEntity User { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    [Column(IsPrimary = true)]
    public long OrgId { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    [NotGen]
    public OrgEntity Org { get; set; }

    /// <summary>
    /// 是否主管
    /// </summary>
    public bool IsManager { get; set; } = false;
}
