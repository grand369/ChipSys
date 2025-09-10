using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Entities;
using ChipSys.Admin.Domain.User;
using ChipSys.Admin.Domain.Role;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Domain.UserRole;

/// <summary>
/// �û���ɫ
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "user_role", OldName = DbConsts.TableOldNamePrefix + "user_role")]
[Index("idx_{tablename}_01", nameof(UserId) + "," + nameof(RoleId), true)]
public class UserRoleEntity : EntityAdd
{
    /// <summary>
    /// �û�Id
    /// </summary>
    [Column(IsPrimary = true)]
    public long UserId { get; set; }

    [NotGen]
    public UserEntity User { get; set; }

    /// <summary>
    /// ��ɫId
    /// </summary>
    [Column(IsPrimary = true)]
    public long RoleId { get; set; }

    [NotGen]
    public RoleEntity Role { get; set; }
}
