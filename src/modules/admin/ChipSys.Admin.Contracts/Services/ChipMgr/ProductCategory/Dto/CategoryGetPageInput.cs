using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.ProductCategory.Dto;

/// <summary>
/// 产品分类分页查询
/// </summary>
public class CategoryGetPageInput : PageInput<CategoryGetPageInput>
{
    /// <summary>
    /// 上级分类Id
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 分类名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 分类编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool? Enabled { get; set; }
}
