using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Services.Product.Dto;

/// <summary>
/// 产品供应商关系列表查询
/// </summary>
public class ProductSupplierGetListInput : QueryInput
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
