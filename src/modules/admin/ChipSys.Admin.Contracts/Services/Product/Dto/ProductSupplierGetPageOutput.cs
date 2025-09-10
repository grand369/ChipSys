namespace ChipSys.Admin.Services.Product.Dto;

/// <summary>
/// 产品供应商关系分页查询输出
/// </summary>
public class ProductSupplierGetPageOutput : ProductSupplierGetOutput
{
    /// <summary>
    /// 产品编码
    /// </summary>
    public string ProductCode { get; set; }

    /// <summary>
    /// 产品品牌
    /// </summary>
    public string ProductBrand { get; set; }

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; }

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; }
}
