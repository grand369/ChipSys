using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Services.Supplier.Dto;

/// <summary>
/// 上传清单列表查询
/// </summary>
public class UploadListGetListInput : QueryInput
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }
}
