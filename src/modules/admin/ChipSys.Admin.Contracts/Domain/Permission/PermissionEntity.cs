using ChipSys.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ChipSys.Admin.Domain.Api;
using ChipSys.Admin.Domain.View;
using ChipSys.Admin.Domain.PermissionApi;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Domain.Permission;

/// <summary>
/// Ȩ��
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "permission", OldName = DbConsts.TableOldNamePrefix + "permission")]
[Index("idx_{tablename}_01", nameof(Platform) + "," + nameof(ParentId) + "," + nameof(Label), true)]
public partial class PermissionEntity : EntityBase, IChilds<PermissionEntity>
{
    /// <summary>
    /// ƽ̨
    /// </summary>
    [Column(StringLength = 20)]
    public string Platform { get; set; }

    /// <summary>
    /// �����ڵ�
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// Ȩ������
    /// </summary>
    [Column(StringLength = 50)]
    public string Label { get; set; }

    /// <summary>
    /// Ȩ�ޱ���
    /// </summary>
    [Column(StringLength = 500)]
    public string Code { get; set; }

    /// <summary>
    /// Ȩ������
    /// </summary>
    [Column(MapType = typeof(int), CanUpdate = false)]
    public PermissionType Type { get; set; }

    /// <summary>
    /// ��ͼId
    /// </summary>
    public long? ViewId { get; set; }

    /// <summary>
    /// ��ͼ
    /// </summary>
    [NotGen]
    public ViewEntity View { get; set; }

    /// <summary>
    /// ·������
    /// </summary>
    [Column(StringLength = 100)]
    public string Name { get; set; }

    /// <summary>
    /// ·�ɵ�ַ
    /// </summary>
    [Column(StringLength = 500)]
    public string Path { get; set; }

    /// <summary>
    /// �ض����ַ
    /// </summary>
    [Column(StringLength = 500)]
    public string Redirect { get; set; }

    /// <summary>
    /// ͼ��
    /// </summary>
    [Column(StringLength = 100)]
    public string Icon { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Hidden { get; set; } = false;

    /// <summary>
    /// չ������
    /// </summary>
    public bool Opened { get; set; }

    /// <summary>
    /// ���´���
    /// </summary>
    public bool NewWindow { get; set; } = false;

    /// <summary>
    /// ��������
    /// </summary>
    public bool External { get; set; } = false;

    /// <summary>
    /// �Ƿ񻺴�
    /// </summary>
    public bool IsKeepAlive { get; set; } = true;

    /// <summary>
    /// �Ƿ�̶�
    /// </summary>
    public bool IsAffix { get; set; } = false;

    /// <summary>
    /// ���ӵ�ַ
    /// </summary>
    [Column(StringLength = 500)]
    public string Link { get; set; }

    /// <summary>
    /// �Ƿ���Ƕ����
    /// </summary>
    public bool IsIframe { get; set; } = false;

    /// <summary>
    /// �Ƿ�ϵͳȨ��
    /// </summary>
    public bool IsSystem { get; set; } = false;

    /// <summary>
    /// ����
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 200)]
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; } = true;

    [NotGen]
    [Navigate(ManyToMany = typeof(PermissionApiEntity))]
    public ICollection<ApiEntity> Apis { get; set; }

    [Navigate(nameof(ParentId))]
    public List<PermissionEntity> Childs { get; set; }
}
