using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Entities;
using ChipSys.Admin.Domain.Tenant;
using ChipSys.Admin.Domain.Pkg;

namespace ChipSys.Admin.Domain.TenantPkg;

/// <summary>
/// ×â»§Ì×²Í
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "tenant_pkg", OldName = DbConsts.TableOldNamePrefix + "tenant_pkg")]
[Index("idx_{tablename}_01", nameof(TenantId) + "," + nameof(PkgId), true)]
public class TenantPkgEntity : EntityAdd
{
    /// <summary>
    /// ×â»§Id
    /// </summary>
    [Column(IsPrimary = true)]
    public long TenantId { get; set; }

    public TenantEntity Tenant { get; set; }

    /// <summary>
    /// Ì×²ÍId
    /// </summary>
    [Column(IsPrimary = true)]
    public long PkgId { get; set; }

    public PkgEntity Pkg { get; set; }
}
