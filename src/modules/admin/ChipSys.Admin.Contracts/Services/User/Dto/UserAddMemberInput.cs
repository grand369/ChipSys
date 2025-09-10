using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Domain.User;

namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// ��ӻ�Ա
/// </summary>
public class UserAddMemberInput: UserMemberFormInput
{
    /// <summary>
    /// ����
    /// </summary>
    [Required(ErrorMessage = "����������")]
    public string Password { get; set; }

    /// <summary>
    /// ״̬
    /// </summary>
    public UserStatus Status { get; set; }
}
