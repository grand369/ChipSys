using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Domain.User;

namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// Ìí¼Ó»áÔ±
/// </summary>
public class UserAddMemberInput: UserMemberFormInput
{
    /// <summary>
    /// ÃÜÂë
    /// </summary>
    [Required(ErrorMessage = "ÇëÊäÈëÃÜÂë")]
    public string Password { get; set; }

    /// <summary>
    /// ×´Ì¬
    /// </summary>
    public UserStatus Status { get; set; }
}
