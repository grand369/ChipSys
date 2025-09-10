using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Services.Supplier.Dto;

/// <summary>
/// 上传清单项目分页查询
/// </summary>
public class UploadListItemGetPageInput : PageInput<UploadListItemGetPageInput>
{
    /// <summary>
    /// 清单Id
    /// </summary>
    public long? ListId { get; set; }

    /// <summary>
    /// 原始编码
    /// </summary>
    public string RawCode { get; set; }

    /// <summary>
    /// 原始品牌
    /// </summary>
    public string RawBrand { get; set; }

    /// <summary>
    /// 匹配状态
    /// </summary>
    public string MatchStatus { get; set; }

    /// <summary>
    /// 匹配到的产品Id
    /// </summary>
    public long? MatchedProductId { get; set; }
}
