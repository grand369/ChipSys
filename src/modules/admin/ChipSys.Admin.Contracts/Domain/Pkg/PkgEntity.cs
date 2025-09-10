using ChipSys.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ChipSys.Admin.Domain.Permission;
using ChipSys.Admin.Domain.Tenant;
using ChipSys.Admin.Domain.TenantPkg;
using ChipSys.Admin.Domain.PkgPermission;

namespace ChipSys.Admin.Domain.Pkg;

/// <summary>
/// �ײ�
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "pkg", OldName = DbConsts.TableOldNamePrefix + "pkg")]
[Index("idx_{tablename}_01", $"{nameof(ParentId)},{nameof(Name)}", true)]
public partial class PkgEntity : EntityBase
{
    /// <summary>
    /// ����Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// �Ӽ��б�
    /// </summary>
    [Navigate(nameof(ParentId))]
    public List<PkgEntity> Childs { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 50)]
    public string Code { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    [Column(StringLength = 200)]
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Enabled { get; set; } = true;

    /// <summary>
    /// ����
    /// </summary>
	public int Sort { get; set; }

    /// <summary>
    /// �⻧�б�
    /// </summary>
    [Navigate(ManyToMany = typeof(TenantPkgEntity))]
    public ICollection<TenantEntity> Tenants { get; set; }

    /// <summary>
    /// Ȩ���б�
    /// </summary>
    [Navigate(ManyToMany = typeof(PkgPermissionEntity))]
    public ICollection<PermissionEntity> Permissions { get; set; }
}
