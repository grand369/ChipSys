using ChipSys.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ChipSys.Admin.Domain.Permission;
using ChipSys.Admin.Domain.Api;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Domain.PermissionApi;

/// <summary>
/// Ȩ�޽ӿ�
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "permission_api", OldName = DbConsts.TableOldNamePrefix + "permission_api")]
[Index("idx_{tablename}_01", nameof(PermissionId) + "," + nameof(ApiId), true)]
public class PermissionApiEntity : EntityAdd
{
    /// <summary>
    /// Ȩ��Id
    /// </summary>
	[Column(IsPrimary = true)]
    public long PermissionId { get; set; }

    /// <summary>
    /// Ȩ��
    /// </summary>
    [NotGen]
    public PermissionEntity Permission { get; set; }

    /// <summary>
    /// �ӿ�Id
    /// </summary>
	[Column(IsPrimary = true)]
    public long ApiId { get; set; }

    /// <summary>
    /// �ӿ�
    /// </summary>
    [NotGen]
    public ApiEntity Api { get; set; }
}
