using ChipSys.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Contracts.Domain.ChipMgr.ProductCategory;

namespace ChipSys.Admin.Contracts.Domain.ChipMgr.Product;

/// <summary>
/// ��Ʒ
/// </summary>
[Table(Name = DbConsts.ChipTableNamePrefix + "product", OldName = DbConsts.ChipTableOldNamePrefix + "product")]
[Index("idx_{tablename}_01", nameof(Code) + "," + nameof(Brand), false)]
public partial class ProductEntity : EntityBase
{
    /// <summary>
    /// ����Id
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [NotGen]
    public CategoryEntity Category { get; set; }

    /// <summary>
    /// ��Ʒ����/�����
    /// </summary>
    [Column(StringLength = 50)]
    public string Code { get; set; }

    /// <summary>
    /// Ʒ��
    /// </summary>
    [Column(StringLength = 100)]
    public string Brand { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 500)]
    public string Description { get; set; }

    /// <summary>
    /// ������
    /// </summary>
    [Column(StringLength = 200)]
    public string Manufacturer { get; set; }

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// ����
    /// </summary>
    public int Sort { get; set; }
}
