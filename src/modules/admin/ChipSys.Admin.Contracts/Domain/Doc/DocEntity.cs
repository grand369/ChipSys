using ChipSys.Admin.Core.Entities;
using FreeSql.DataAnnotations;

namespace ChipSys.Admin.Domain.Doc;

/// <summary>
/// �ĵ�
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "doc", OldName = DbConsts.TableOldNamePrefix + "document")]
[Index("idx_{tablename}_01", nameof(ParentId) + "," + nameof(Label) + "," + nameof(TenantId), true)]
public partial class DocEntity : EntityTenant
{
    /// <summary>
    /// �����ڵ�
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 50)]
    public string Label { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(MapType = typeof(int), CanUpdate = false)]
    public DocType Type { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 500)]
    public string Name { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = -1)]
    public string Content { get; set; }

    /// <summary>
    /// Html
    /// </summary>
    [Column(StringLength = -1)]
    public string Html { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Enabled { get; set; } = true;

    /// <summary>
    /// ����
    /// </summary>
    public bool? Opened { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public int? Sort { get; set; } = 0;

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 100)]
    public string Description { get; set; }
}
