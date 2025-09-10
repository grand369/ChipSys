using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// ��Ա��
/// </summary>
public class UserMemberFormInput
{
    /// <summary>
    /// ��ԱId
    /// </summary>
    public virtual long Id { get; set; }

    /// <summary>
    /// �˺�
    /// </summary>
    [Required(ErrorMessage = "�������˺�")]
    public string UserName { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// �ֻ���
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Email { get; set; }
}
