using FreeSql;
using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Entities;
using ChipSys.Admin.Domain.User;
using ChipSys.Admin.Domain.Org;
using ChipSys.Admin.Domain.TenantPkg;
using ChipSys.Admin.Domain.Pkg;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Domain.Tenant;

/// <summary>
/// �⻧
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "tenant", OldName = DbConsts.TableOldNamePrefix + "tenant")]
public partial class TenantEntity : EntityBase
{
    /// <summary>
    /// ��Ȩ�û�
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// �û�
    /// </summary>
    [NotGen]
    public UserEntity User { get; set; }

    /// <summary>
    /// ��Ȩ����
    /// </summary>
    public long OrgId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [NotGen]
    public OrgEntity Org { get; set; }

    /// <summary>
    /// �⻧����
    /// </summary>
    public TenantType? TenantType { get; set; } = Tenant.TenantType.Tenant;

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 300)]
    public string Domain { get; set; }

    /// <summary>
    /// ���ݿ�ע���
    /// </summary>
    [Column(StringLength = 50)]
    public string DbKey { get; set; }

    /// <summary>
    /// ���ݿ�
    /// </summary>
    [Column(MapType = typeof(int?))]
    public DataType? DbType { get; set; }

    /// <summary>
    /// �����ַ���
    /// </summary>
    [Column(StringLength = 500)]
    public string ConnectionString { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Enabled { get; set; } = true;

    /// <summary>
    /// ˵��
    /// </summary>
    [Column(StringLength = 500)]
    public string Description { get; set; }

    /// <summary>
    /// �ײ��б�
    /// </summary>
    [NotGen]
    [Navigate(ManyToMany = typeof(TenantPkgEntity))]
    public ICollection<PkgEntity> Pkgs { get; set; }
}
