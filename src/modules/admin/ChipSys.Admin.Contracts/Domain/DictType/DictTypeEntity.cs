using ChipSys.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Domain.Dict;

namespace ChipSys.Admin.Domain.DictType;

/// <summary>
/// �����ֵ�����
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "dict_type", OldName = DbConsts.TableOldNamePrefix + "dict_type")]
[Index("idx_{tablename}_01", nameof(Name), true)]
public class DictTypeEntity : EntityBase, IChilds<DictTypeEntity>
{
    /// <summary>
    /// �ϼ�Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// �ϼ�����
    /// </summary>
    [NotGen]
    public DictEntity Parent { get; set; }

    /// <summary>
    /// �Ӽ��б�
    /// </summary>
    [Navigate(nameof(ParentId))]
    public List<DictTypeEntity> Childs { get; set; }

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
    /// ����
    /// </summary>
    [Column(StringLength = 500)]
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Enabled { get; set; } = true;

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool IsTree { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public int Sort { get; set; }
}
