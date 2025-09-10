using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Entities;
using ChipSys.Admin.Domain.Org;
using ChipSys.Admin.Domain.User;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Domain.UserOrg;

/// <summary>
/// �û���������
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "user_org", OldName = DbConsts.TableOldNamePrefix + "user_org")]
[Index("idx_{tablename}_01", nameof(UserId) + "," + nameof(OrgId), true)]
public partial class UserOrgEntity : EntityUpdate
{
    /// <summary>
    /// �û�Id
    /// </summary>
    [Column(IsPrimary = true)]
    public long UserId { get; set; }

    /// <summary>
    /// �û�
    /// </summary>
    [NotGen]
    public UserEntity User { get; set; }

    /// <summary>
    /// ����Id
    /// </summary>
    [Column(IsPrimary = true)]
    public long OrgId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [NotGen]
    public OrgEntity Org { get; set; }

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool IsManager { get; set; } = false;
}
