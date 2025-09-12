namespace ChipSys.Admin.Contracts.Services.ChipMgr.Product.Dto;

/// <summary>
/// 产品列表查询输出
/// </summary>
public class ProductGetListOutput : ProductGetOutput
{
    /// <summary>
    /// 分类名称
    /// </summary>
    public string CategoryName { get; set; }
}
