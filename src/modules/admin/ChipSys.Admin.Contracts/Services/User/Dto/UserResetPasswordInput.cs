using ChipSys.Admin.Core.Entities;

namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// ��������
/// </summary>
public class UserResetPasswordInput : Entity
{
    /// <summary>
    /// ����
    /// </summary>
    public string Password { get; set; }
}
