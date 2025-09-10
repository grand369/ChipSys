using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Dto;

/// <summary>
/// 删除
/// </summary>
public class FileDeleteInput
{
    /// <summary>
    /// 文件Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择文件")]
    public long Id { get; set; }
}
