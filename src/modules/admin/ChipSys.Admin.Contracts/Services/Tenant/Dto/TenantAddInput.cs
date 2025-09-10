using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Tenant.Dto;

/// <summary>
/// ���
/// </summary>
public class TenantAddInput
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
    /// �ֻ�����
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// �����ַ
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Domain { get; set; }

    /// <summary>
    /// ���ݿ�ע���
    /// </summary>
    public string DbKey { get; set; }

    /// <summary>
    /// ���ݿ�
    /// </summary>
    public FreeSql.DataType? DbType { get; set; }

    /// <summary>
    /// �����ַ���
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Enabled { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    public string Description { get; set; }
}
