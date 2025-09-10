using FreeSql.DataAnnotations;
using ChipSys.Admin.Domain.PermissionApi;
using ChipSys.Admin.Domain.Permission;
using ChipSys.Admin.Core.Entities;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Domain.Api;

/// <summary>
/// �ӿڹ���
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "api", OldName = DbConsts.TableOldNamePrefix + "api")]
[Index("idx_{tablename}_01", nameof(ParentId) + "," + nameof(Path), true)]
public partial class ApiEntity : EntityBase, IChilds<ApiEntity>
{
    /// <summary>
    /// ����ģ��
    /// </summary>
	public long ParentId { get; set; }

    /// <summary>
    /// �ӿ�����
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }

    /// <summary>
    /// �ӿ�����
    /// </summary>
    [Column(StringLength = 500)]
    public string Label { get; set; }

    /// <summary>
    /// �ӿڵ�ַ
    /// </summary>
    [Column(StringLength = 500)]
    public string Path { get; set; }

    /// <summary>
    /// �ӿ��ύ����
    /// </summary>
    [Column(StringLength = 50)]
    public string HttpMethods { get; set; }

    /// <summary>
    /// ���ýӿ���־
    /// </summary>
    public bool EnabledLog { get; set; } = true;

    /// <summary>
    /// �����������
    /// </summary>
    public bool EnabledParams { get; set; } = false;

    /// <summary>
    /// ������Ӧ���
    /// </summary>
    public bool EnabledResult { get; set; } = false;

    /// <summary>
    /// ˵��
    /// </summary>
    [Column(StringLength = 500)]
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; } = true;

    [Navigate(nameof(ParentId))]
    public List<ApiEntity> Childs { get; set; }

    [NotGen]
    [Navigate(ManyToMany = typeof(PermissionApiEntity))]
    public ICollection<PermissionEntity> Permissions { get; set; }
}
