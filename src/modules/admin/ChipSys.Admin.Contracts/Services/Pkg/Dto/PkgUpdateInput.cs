using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Pkg.Dto;

/// <summary>
/// ÐÞ¸Ä
/// </summary>
public partial class PkgUpdateInput : PkgAddInput
{
    /// <summary>
    /// Ì×²ÍId
    /// </summary>
    [Required]
    [ValidateRequired("ÇëÑ¡ÔñÌ×²Í")]
    public long Id { get; set; }
}
