using ChipSys.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Contracts.Domain.ChipMgr.ProductCategory;

namespace ChipSys.Admin.Contracts.Domain.ChipMgr.Product;

/// <summary>
/// 产品
/// </summary>
[Table(Name = DbConsts.ChipTableNamePrefix + "product", OldName = DbConsts.ChipTableOldNamePrefix + "product")]
[Index("idx_{tablename}_01", nameof(Code) + "," + nameof(Brand), false)]
public partial class ProductEntity : EntityTenantWithData
{
    /// <summary>
    /// 分类Id
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// 分类
    /// </summary>
    [NotGen]
    public CategoryEntity Category { get; set; }
    /// <summary>
    /// 产品名称
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }
    /// <summary>
    /// 产品单位
    /// </summary>
    [Column(StringLength = 50)]
    public string Unit { get; set; }
    /// <summary>
    /// 产品编码/型号
    /// </summary>
    [Column(StringLength = 50)]
    public string Code { get; set; }
    /// <summary>
    /// 状态
    /// </summary>
    public int Status { get; set; }
    /// <summary>
    /// 品牌
    /// </summary>
    [Column(StringLength = 100)]
    public string Brand { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [Column(StringLength = 500)]
    public string Description { get; set; }

    /// <summary>
    /// 制造商
    /// </summary>
    [Column(StringLength = 200)]
    public string Manufacturer { get; set; }

    /// <summary>
    /// 产品型号
    /// </summary>
    [Column(StringLength = 200)]
    public string Model { get; set; }
    /// <summary>
    /// 产品参数
    /// </summary>
    [Column(StringLength = 200)]
    public string? Specification { get; set; }
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// 供应商ID
    /// </summary>
    public long? SupplierId { get; set; }
}