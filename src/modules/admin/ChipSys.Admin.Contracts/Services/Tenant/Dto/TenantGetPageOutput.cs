using System.Text.Json.Serialization;
using ChipSys.Admin.Domain.Pkg;

namespace ChipSys.Admin.Services.Tenant.Dto;

/// <summary>
/// �⻧��ҳ��Ӧ
/// </summary>
public class TenantGetPageOutput
{
    /// <summary>
    /// ����
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// ��ҵ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ��ҵ����
    /// </summary>
    public string Code { get; set; }

    [JsonIgnore]
    public ICollection<PkgEntity> Pkgs { get; set; }

    /// <summary>
    /// �ײ�
    /// </summary>
    public string[] PkgNames { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string RealName { get; set; }

    /// <summary>
    /// �˺�
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// �ֻ�����
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// �����ַ
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// ���ݿ�
    /// </summary>
    [JsonIgnore]
    public FreeSql.DataType? DbType { get; set; }

    /// <summary>
    /// ���ݿ�����
    /// </summary>
    public string DbTypeName => DbType?.ToDescriptionOrString();

    /// <summary>
    /// ����
    /// </summary>
	public bool Enabled { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public DateTime? CreatedTime { get; set; }
}
