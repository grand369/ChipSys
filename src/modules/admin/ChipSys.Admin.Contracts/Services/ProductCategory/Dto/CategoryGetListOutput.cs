namespace ChipSys.Admin.Services.ProductCategory.Dto;

/// <summary>
/// 产品分类列表查询输出
/// </summary>
public class CategoryGetListOutput : CategoryGetOutput
{
    /// <summary>
    /// 上级分类名称
    /// </summary>
    public string ParentName { get; set; }
}
