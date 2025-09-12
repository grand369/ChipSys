using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.Supplier.Dto;

/// <summary>
/// 上传清单项目修改
/// </summary>
public class UploadListItemUpdateInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "请选择要修改的上传清单项目")]
    public long Id { get; set; }

    /// <summary>
    /// 清单Id
    /// </summary>
    [Required(ErrorMessage = "请选择清单")]
    public long ListId { get; set; }

    /// <summary>
    /// 原始编码
    /// </summary>
    [StringLength(50, ErrorMessage = "原始编码长度不能超过50个字符")]
    public string RawCode { get; set; }

    /// <summary>
    /// 原始品牌
    /// </summary>
    [StringLength(50, ErrorMessage = "原始品牌长度不能超过50个字符")]
    public string RawBrand { get; set; }

    /// <summary>
    /// 原始描述
    /// </summary>
    [StringLength(200, ErrorMessage = "原始描述长度不能超过200个字符")]
    public string RawDescription { get; set; }

    /// <summary>
    /// 匹配状态
    /// </summary>
    [StringLength(20, ErrorMessage = "匹配状态长度不能超过20个字符")]
    public string MatchStatus { get; set; } = "Unmatched";

    /// <summary>
    /// 匹配到的产品Id
    /// </summary>
    public long? MatchedProductId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
