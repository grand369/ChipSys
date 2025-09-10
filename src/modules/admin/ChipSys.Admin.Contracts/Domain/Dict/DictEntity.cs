using ChipSys.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ChipSys.Admin.Domain.DictType;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Domain.Dict;

/// <summary>
/// �����ֵ�
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "dict", OldName = DbConsts.TableOldNamePrefix + "dict")]
[Index("idx_{tablename}_01", nameof(DictTypeId) + "," + nameof(Name), true)]
public partial class DictEntity : EntityBase, IChilds<DictEntity>
{
    /// <summary>
    /// �ϼ�Id
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// �ϼ��ֵ�
    /// </summary>
    [NotGen]
    public DictEntity Parent { get; set; }

    /// <summary>
    /// �Ӽ��б�
    /// </summary>
    [Navigate(nameof(ParentId))]
    public List<DictEntity> Childs { get; set; }

    /// <summary>
    /// �ֵ�����Id
    /// </summary>
    [Column(OldName = "DictionaryTypeId")]
    public long DictTypeId { get; set; }

    /// <summary>
    /// �ֵ�����
    /// </summary>
    [NotGen]
    public DictTypeEntity DictType { get; set; }

    /// <summary>
    /// �ֵ�����
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }

    /// <summary>
    /// �ֵ����
    /// </summary>
    [Column(StringLength = 50)]
    public string Code { get; set; }

    /// <summary>
    /// �ֵ�ֵ
    /// </summary>
    [Column(StringLength = 50)]
    public string Value { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    [Column(StringLength = 500)]
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Enabled { get; set; } = true;

    /// <summary>
    /// ����
    /// </summary>
	public int Sort { get; set; }
}
