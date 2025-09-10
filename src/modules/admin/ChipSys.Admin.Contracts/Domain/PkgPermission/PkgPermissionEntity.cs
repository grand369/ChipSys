using ChipSys.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ChipSys.Admin.Domain.Permission;
using ChipSys.Admin.Domain.Pkg;

namespace ChipSys.Admin.Domain.PkgPermission;

/// <summary>
/// �ײ�Ȩ��
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "pkg_permission", OldName = DbConsts.TableOldNamePrefix + "pkg_permission")]
[Index("idx_{tablename}_01", nameof(Platform) + "," + nameof(PkgId) + "," + nameof(PermissionId), true)]
public class PkgPermissionEntity : EntityAdd
{
    /// <summary>
    /// ƽ̨
    /// </summary>
    public string Platform { get; set; }

    /// <summary>
    /// �ײ�Id
    /// </summary>
	[Column(IsPrimary = true)]
    public long PkgId { get; set; }

    /// <summary>
    /// �ײ�
    /// </summary>
    public PkgEntity Pkg { get; set; }

    /// <summary>
    /// Ȩ��Id
    /// </summary>
	[Column(IsPrimary = true)]
    public long PermissionId { get; set; }

    /// <summary>
    /// Ȩ��
    /// </summary>
    public PermissionEntity Permission { get; set; }
}
