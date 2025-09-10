using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Domain.User;

namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// �û���
/// </summary>
public class UserFormInput
{
    /// <summary>
    /// �û�Id
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
    [Required(ErrorMessage = "����������")]
    public string Name { get; set; }

    /// <summary>
    /// �ֻ���
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// ��ɫIds
    /// </summary>
    public virtual long[] RoleIds { get; set; }

    /// <summary>
    /// ֱ������Id
    /// </summary>
    public long? ManagerUserId { get; set; }

    /// <summary>
    /// ֱ����������
    /// </summary>
    public string ManagerUserName { get; set; }

    /// <summary>
    /// Ա��
    /// </summary>
    [Required]
    public StaffAddInput Staff { get; set; }
}
