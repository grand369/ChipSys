using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.Product.Dto;

/// <summary>
/// 产品列表查询
/// </summary>
public class ProductGetListInput : QueryInput
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
    /// 产品型号
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool? Enabled { get; set; }
}