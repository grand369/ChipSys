using ChipSys.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ChipSys.Admin.Domain.UserStaff;
using ChipSys.Admin.Domain.User;
using ChipSys.Admin.Domain.Role;
using ChipSys.Admin.Domain.UserOrg;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Domain.Org;

/// <summary>
/// ��֯�ܹ�
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "org", OldName = DbConsts.TableOldNamePrefix + "org")]
[Index("idx_{tablename}_01", $"{nameof(TenantId)},{nameof(ParentId)},{nameof(Name)}", true)]
public partial class OrgEntity : EntityTenant, IChilds<OrgEntity>
{
    /// <summary>
    /// ����
    /// </summary>
	public long ParentId { get; set; }

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
    /// ֵ
    /// </summary>
    [Column(StringLength = 50)]
    public string Value { get; set; }

    /// <summary>
    /// ��Ա��
    /// </summary>
    public int MemberCount { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Enabled { get; set; } = true;

    /// <summary>
    /// ����
    /// </summary>
	public int Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 500)]
    public string Description { get; set; }

    /// <summary>
    /// Ա���б�
    /// </summary>
    [NotGen]
    [Navigate(ManyToMany = typeof(UserOrgEntity))]
    public ICollection<UserStaffEntity> Staffs { get; set; }

    /// <summary>
    /// �û��б�
    /// </summary>
    [NotGen]
    [Navigate(ManyToMany = typeof(UserOrgEntity))]
    public ICollection<UserEntity> Users { get; set; }

    /// <summary>
    /// ��ɫ�б�
    /// </summary>
    [NotGen]
    [Navigate(ManyToMany = typeof(RoleOrgEntity))]
    public ICollection<RoleEntity> Roles { get; set; }

    /// <summary>
    /// �Ӽ��б�
    /// </summary>
    [Navigate(nameof(ParentId))]
    public List<OrgEntity> Childs { get; set; }
}
