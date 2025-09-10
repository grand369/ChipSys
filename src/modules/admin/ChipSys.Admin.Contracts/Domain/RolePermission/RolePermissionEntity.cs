using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Entities;
using ChipSys.Admin.Domain.Role;
using ChipSys.Admin.Domain.Permission;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Domain.RolePermission;

/// <summary>
/// ��ɫȨ��
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "role_permission", OldName = DbConsts.TableOldNamePrefix + "role_permission")]
[Index("idx_{tablename}_01", nameof(Platform) + "," + nameof(RoleId) + "," + nameof(PermissionId), true)]
public class RolePermissionEntity : EntityAdd
{
    /// <summary>
    /// ƽ̨
    /// </summary>
    public string Platform { get; set; }

    /// <summary>
    /// ��ɫId
    /// </summary>
	[Column(IsPrimary = true)]
    public long RoleId { get; set; }

    /// <summary>
    /// Ȩ��Id
    /// </summary>
	public long PermissionId { get; set; }

    /// <summary>
    /// ��ɫ
    /// </summary>
    [NotGen]
    [Column(IsPrimary = true)]
    public RoleEntity Role { get; set; }

    /// <summary>
    /// Ȩ��
    /// </summary>
    [NotGen]
    public PermissionEntity Permission { get; set; }
}
