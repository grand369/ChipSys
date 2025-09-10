using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Services.Product.Dto;

/// <summary>
/// 产品供应商关系分页查询
/// </summary>
public class ProductSupplierGetPageInput : PageInput<ProductSupplierGetPageInput>
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public long? ProductId { get; set; }

    /// <summary>
    /// 供应商Id
    /// </summary>
    public long? SupplierId { get; set; }

    /// <summary>
    /// 产品编码
    /// </summary>
    public string ProductCode { get; set; }

    /// <summary>
    /// 产品品牌
    /// </summary>
    public string ProductBrand { get; set; }

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; }

    /// <summary>
    /// 货币
    /// </summary>
    public string Currency { get; set; }
}
