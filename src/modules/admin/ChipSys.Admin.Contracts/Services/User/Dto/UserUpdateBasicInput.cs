using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// ���»�����Ϣ
/// </summary>
public class UserUpdateBasicInput
{
    /// <summary>
    /// ����
    /// </summary>
    [Required(ErrorMessage = "����������")]
    public string Name { get; set; }

    /// <summary>
    /// �ǳ�
    /// </summary>
    public string NickName { get; set; }
}
