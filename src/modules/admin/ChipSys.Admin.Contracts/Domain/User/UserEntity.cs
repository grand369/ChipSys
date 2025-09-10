using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Entities;
using ChipSys.Admin.Domain.Tenant;
using ChipSys.Admin.Domain.Role;
using ChipSys.Admin.Domain.UserRole;
using ChipSys.Admin.Domain.UserStaff;
using ChipSys.Admin.Domain.Org;
using ChipSys.Admin.Domain.UserOrg;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Domain.User;

/// <summary>
/// �û�
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "user", OldName = DbConsts.TableOldNamePrefix + "user")]
[Index("idx_{tablename}_01", nameof(UserName), true)]
[Index("idx_{tablename}_02", nameof(Mobile))]
[Index("idx_{tablename}_03", nameof(Email))]
public partial class UserEntity : EntityTenant
{
    [NotGen]
    public TenantEntity Tenant { get; set; }

    /// <summary>
    /// �˺�
    /// </summary>
    [Column(StringLength = 60)]
    public string UserName { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 200)]
    public string Password { get; set; }

    /// <summary>
    /// �����������
    /// </summary>
    [Column(MapType = typeof(int?))]
    public PasswordEncryptType? PasswordEncryptType { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 60)]
    public string Name { get; set; }

    /// <summary>
    /// �ֻ���
    /// </summary>
    [Column(StringLength = 20)]
    public string Mobile { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 100)]
    public string Email { get; set; }

    /// <summary>
    /// ��������Id
    /// </summary>
    public long OrgId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [NotGen]
    public OrgEntity Org { get; set; }

    /// <summary>
    /// ֱ������Id
    /// </summary>
    public long? ManagerUserId { get; set; }

    /// <summary>
    /// ֱ������
    /// </summary>
    [NotGen]
    public UserEntity ManagerUser { get; set; }

    /// <summary>
    /// �ǳ�
    /// </summary>
    [Column(StringLength = 60)]
    public string NickName { get; set; }

    /// <summary>
    /// ͷ��
    /// </summary>
    [Column(StringLength = 500)]
    public string Avatar { get; set; }

    /// <summary>
    /// �û�״̬
    /// </summary>
    [Column(MapType = typeof(int?))]
    public UserStatus? Status { get; set; }

    /// <summary>
    /// �û�����
    /// </summary>
    [Column(MapType = typeof(int))]
    public UserType Type { get; set; } = UserType.DefaultUser;

    /// <summary>
    /// ����¼ʱ��
    /// </summary>
    [Column(StringLength = 100)]
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// ����¼IP
    /// </summary>
    [Column(StringLength = 100)]
    public string LastLoginIP { get; set; }

    /// <summary>
    /// ����¼����
    /// </summary>
    [Column(StringLength = 100)]
    public string LastLoginCountry { get; set; }

    /// <summary>
    /// ����¼ʡ��
    /// </summary>
    [Column(StringLength = 100)]
    public string LastLoginProvince { get; set; }

    /// <summary>
    /// ����¼����
    /// </summary>
    [Column(StringLength = 100)]
    public string LastLoginCity { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// ��ɫ�б�
    /// </summary>
    [NotGen]
    [Navigate(ManyToMany = typeof(UserRoleEntity))]
    public ICollection<RoleEntity> Roles { get; set; }

    /// <summary>
    /// �����б�
    /// </summary>
    [NotGen]
    [Navigate(ManyToMany = typeof(UserOrgEntity))]
    public ICollection<OrgEntity> Orgs { get; set; }

    /// <summary>
    /// Ա��
    /// </summary>
    [NotGen]
    [Navigate(nameof(Id))]
    public UserStaffEntity Staff { get; set; }
}
