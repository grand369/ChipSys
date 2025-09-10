using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Services.Product.Dto;

/// <summary>
/// 产品分页查询
/// </summary>
public class ProductGetPageInput : PageInput<ProductGetPageInput>
{
    /// <summary>
    /// 分类Id
    /// </summary>
    public long? CategoryId { get; set; }

    /// <summary>
    /// 产品编码/型号
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 品牌
    /// </summary>
    public string Brand { get; set; }

    /// <summary>
    /// 制造商
    /// </summary>
    public string Manufacturer { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool? Enabled { get; set; }
}
