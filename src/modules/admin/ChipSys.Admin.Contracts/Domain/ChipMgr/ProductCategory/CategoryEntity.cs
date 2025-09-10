using ChipSys.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Product;

namespace ChipSys.Admin.Contracts.Domain.ChipMgr.ProductCategory;

/// <summary>
/// ��Ʒ����
/// </summary>
[Table(Name = DbConsts.ChipTableNamePrefix + "category", OldName = DbConsts.ChipTableOldNamePrefix + "category")]
[Index("idx_{tablename}_01", nameof(Name), false)]
public partial class CategoryEntity : EntityBase, IChilds<CategoryEntity>
{
    /// <summary>
    /// �ϼ�����Id
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// �ϼ�����
    /// </summary>
    [NotGen]
    public CategoryEntity Parent { get; set; }

    /// <summary>
    /// �ӷ���
    /// </summary>
    [Navigate(nameof(ParentId))]
    public List<CategoryEntity> Childs { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    [Column(StringLength = 100)]
    public string Name { get; set; }

    /// <summary>
    /// �������
    /// </summary>
    [Column(StringLength = 50)]
    public string Code { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 500)]
    public string Description { get; set; }

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// ����
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// ��Ʒ�б�
    /// </summary>
    [Navigate(nameof(ProductEntity.CategoryId))]
    public List<ProductEntity> Products { get; set; }
}
