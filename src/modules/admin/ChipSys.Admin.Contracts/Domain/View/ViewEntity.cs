using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Admin.Domain.View;

/// <summary>
/// ��ͼ����
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "view", OldName = DbConsts.TableOldNamePrefix + "view")]
[Index("idx_{tablename}_01", nameof(Platform) + "," + nameof(ParentId) + "," + nameof(Label), true)]
public partial class ViewEntity : EntityBase, IChilds<ViewEntity>
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
    /// ��ͼ����
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }

    /// <summary>
    /// ��ͼ����
    /// </summary>
    [Column(StringLength = 500)]
    public string Label { get; set; }

    /// <summary>
    /// ��ͼ·��
    /// </summary>
    [Column(StringLength = 500)]
    public string Path { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    [Column(StringLength = 500)]
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Cache { get; set; } = true;

    /// <summary>
    /// ����
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; } = true;

    [Navigate(nameof(ParentId))]
    public List<ViewEntity> Childs { get; set; }
}
