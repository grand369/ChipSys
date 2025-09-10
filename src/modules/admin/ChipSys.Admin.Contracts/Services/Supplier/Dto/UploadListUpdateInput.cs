using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Supplier.Dto;

/// <summary>
/// 上传清单修改
/// </summary>
public class UploadListUpdateInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "请选择要修改的上传清单")]
    public long Id { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    [Required(ErrorMessage = "请选择用户")]
    public long UserId { get; set; }

    /// <summary>
    /// 文件名
    /// </summary>
    [StringLength(200, ErrorMessage = "文件名长度不能超过200个字符")]
    public string? FileName { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [StringLength(20, ErrorMessage = "状态长度不能超过20个字符")]
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
