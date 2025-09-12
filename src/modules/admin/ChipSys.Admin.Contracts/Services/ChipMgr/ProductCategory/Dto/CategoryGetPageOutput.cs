namespace ChipSys.Admin.Contracts.Services.ChipMgr.ProductCategory.Dto;

/// <summary>
/// 产品分类分页查询输出
/// </summary>
public class CategoryGetPageOutput : CategoryGetOutput
{
    /// <summary>
    /// 上级分类名称
    /// </summary>
    public string ParentName { get; set; }
}
