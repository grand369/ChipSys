using ChipSys.Admin.Domain.Tenant;
using ChipSys.Admin.Domain.User;

namespace ChipSys.Admin.Services.Auth.Dto;

/// <summary>
/// ��¼��Ϣ
/// </summary>
public class AuthLoginOutput
{
    /// <summary>
    /// ����Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// �˺�
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// �û�����
    /// </summary>
    public UserType Type { get; set; }

    /// <summary>
    /// �⻧Id
    /// </summary>
    public long? TenantId { get; set; }

    /// <summary>
    /// ����Id
    /// </summary>
    public long? OrgId { get; set; }

    /// <summary>
    /// �⻧��Ϣ
    /// </summary>
    public AuthLoginTenantModel Tenant { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; }
}

/// <summary>
/// �⻧��Ϣ
/// </summary>
public class AuthLoginTenantModel
{
    /// <summary>
    /// �⻧����
    /// </summary>
    public TenantType? TenantType { get; set; }

    /// <summary>
    /// ���ݿ�ע���
    /// </summary>
    public string DbKey { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; }
}
