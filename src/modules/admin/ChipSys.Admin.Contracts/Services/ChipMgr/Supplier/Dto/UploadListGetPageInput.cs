using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.Supplier.Dto;

/// <summary>
/// 上传清单分页查询
/// </summary>
public class UploadListGetPageInput : PageInput<UploadListGetPageInput>
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
