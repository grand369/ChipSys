namespace ChipSys.Admin.Services.Product.Dto;

/// <summary>
/// 产品分页查询输出
/// </summary>
public class ProductGetPageOutput : ProductGetOutput
{
    /// <summary>
    /// 分类名称
    /// </summary>
    public string CategoryName { get; set; }
}
