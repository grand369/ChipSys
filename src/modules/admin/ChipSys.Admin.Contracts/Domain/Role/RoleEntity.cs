using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Entities;
using ChipSys.Admin.Domain.Permission;
using ChipSys.Admin.Domain.User;
using ChipSys.Admin.Domain.UserRole;
using ChipSys.Admin.Domain.RolePermission;
using ChipSys.Admin.Domain.Org;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Domain.Role;

/// <summary>
/// ��ɫ
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "role", OldName = DbConsts.TableOldNamePrefix + "role")]
[Index("idx_{tablename}_01", $"{nameof(TenantId)},{nameof(ParentId)},{nameof(Name)}", true)]
public partial class RoleEntity : EntityTenant
{
    /// <summary>
    /// ����Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// �Ӽ��б�
    /// </summary>
    [Navigate(nameof(ParentId))]
    public List<RoleEntity> Childs { get; set; }

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
    /// ��ɫ����
    /// </summary>
    [Column(MapType = typeof(int), CanUpdate = false)]
    public RoleType Type { get; set; }

    /// <summary>
    /// ���ݷ�Χ
    /// </summary>
    [Column(MapType = typeof(int))]
    public DataScope DataScope { get; set; } = DataScope.All;

    /// <summary>
    /// ˵��
    /// </summary>
    [Column(StringLength = 200)]
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Hidden { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public int Sort { get; set; }

    /// <summary>
    /// �û��б�
    /// </summary>
    [NotGen]
    [Navigate(ManyToMany = typeof(UserRoleEntity))]
    public ICollection<UserEntity> Users { get; set; }

    /// <summary>
    /// �����б�
    /// </summary>
    [NotGen]
    [Navigate(ManyToMany = typeof(RoleOrgEntity))]
    public ICollection<OrgEntity> Orgs { get; set; }

    /// <summary>
    /// Ȩ���б�
    /// </summary>
    [NotGen]
    [Navigate(ManyToMany = typeof(RolePermissionEntity))]
    public ICollection<PermissionEntity> Permissions { get; set; }
}
