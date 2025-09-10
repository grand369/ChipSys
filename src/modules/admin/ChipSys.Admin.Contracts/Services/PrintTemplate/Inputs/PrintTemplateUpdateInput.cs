using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.PrintTemplate.Inputs;

/// <summary>
/// 修改
/// </summary>
public partial class PrintTemplateUpdateInput : PrintTemplateAddInput
{
    /// <summary>
    /// 打印模板Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择打印模板")]
    public long Id { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public long Version { get; set; }
}
