using ChipSys.Admin.Core.Entities;

namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// ÖØÖÃÃÜÂë
/// </summary>
public class UserResetPasswordInput : Entity
{
    /// <summary>
    /// ÃÜÂë
    /// </summary>
    public string Password { get; set; }
}
