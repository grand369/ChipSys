using FreeSql.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Admin.Domain.PrintTemplate;

/// <summary>
/// ��ӡģ��
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "print_template")]
[Index("idx_{tablename}_01", $"{nameof(TenantId)},{nameof(Name)}", true)]
[Index("idx_{tablename}_01", $"{nameof(TenantId)},{nameof(Code)}", true)]
public partial class PrintTemplateEntity : EntityVersion, ITenant
{
    /// <summary>
    /// �⻧Id
    /// </summary>
    [Description("�⻧Id")]
    [Column(Position = 2, CanUpdate = false)]
    [JsonPropertyOrder(-20)]
    public virtual long? TenantId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 50)]
    public string Code { get; set; }

    /// <summary>
    /// ģ��
    /// </summary>
    [Column(StringLength = -1)]
    public string Template { get; set; }

    /// <summary>
    /// ��ӡ����
    /// </summary>
    [Column(StringLength = -1)]
    public string PrintData { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    [Column(StringLength = 200)]
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Enabled { get; set; } = true;

    /// <summary>
    /// ����
    /// </summary>
	public int Sort { get; set; }
}
