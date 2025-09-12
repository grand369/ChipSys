using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.ProductCategory.Dto;

/// <summary>
/// 产品分类添加
/// </summary>
public class CategoryAddInput
{
    /// <summary>
    /// 上级分类Id
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 分类名称
    /// </summary>
    [Required(ErrorMessage = "请输入分类名称")]
    [StringLength(100, ErrorMessage = "分类名称长度不能超过100个字符")]
    public string Name { get; set; }

    /// <summary>
    /// 分类编码
    /// </summary>
    [StringLength(50, ErrorMessage = "分类编码长度不能超过50个字符")]
    public string Code { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [StringLength(500, ErrorMessage = "描述长度不能超过500个字符")]
    public string Description { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}
