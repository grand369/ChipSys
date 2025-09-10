using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Tenant.Dto;

/// <summary>
/// ע��
/// </summary>
public class TenantRegInput
{
    /// <summary>
    /// �⻧Id
    /// </summary>
    public virtual long Id { get; set; }

    /// <summary>
    /// ��ҵ����
    /// </summary>
    [Required(ErrorMessage = "��������ҵ����")]
    public string Name { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// �ײ�Ids
    /// </summary>
    public virtual long[] PkgIds { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string RealName { get; set; }

    /// <summary>
    /// �˺�
    /// </summary>
    [Required(ErrorMessage = "�������˺�")]
    public string UserName { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// �ֻ���
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// �����ַ
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Enabled { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    public string Description { get; set; }
}
