using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.Product.Dto;

/// <summary>
/// 产品修改
/// </summary>
public class ProductUpdateInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "请选择要修改的产品")]
    public long Id { get; set; }

    /// <summary>
    /// 分类Id
    /// </summary>
    [Required(ErrorMessage = "请选择产品分类")]
    public long CategoryId { get; set; }

    /// <summary>
    /// 产品编码/型号
    /// </summary>
    [Required(ErrorMessage = "请输入产品编码")]
    [StringLength(50, ErrorMessage = "产品编码长度不能超过50个字符")]
    public string Code { get; set; }

    /// <summary>
    /// 品牌
    /// </summary>
    [StringLength(100, ErrorMessage = "品牌长度不能超过100个字符")]
    public string Brand { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [StringLength(500, ErrorMessage = "描述长度不能超过500个字符")]
    public string Description { get; set; }

    /// <summary>
    /// 制造商
    /// </summary>
    [StringLength(200, ErrorMessage = "制造商长度不能超过200个字符")]
    public string Manufacturer { get; set; }

    /// <summary>
    /// 产品型号
    /// </summary>
    [StringLength(200, ErrorMessage = "产品型号长度不能超过200个字符")]
    public string Model { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}