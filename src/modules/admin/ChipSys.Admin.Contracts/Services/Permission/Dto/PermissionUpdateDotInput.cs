using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Permission.Dto;

/// <summary>
/// 修改权限点
/// </summary>
public class PermissionUpdateDotInput : PermissionAddDotInput
{
    /// <summary>
    /// 权限Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择权限点")]
    public long Id { get; set; }
}
