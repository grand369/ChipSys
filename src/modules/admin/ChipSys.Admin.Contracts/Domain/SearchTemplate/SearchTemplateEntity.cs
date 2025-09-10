using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Entities;
namespace ChipSys.Admin.Domain.SearchTemplate;

/// <summary>
///  ��ѯģ��
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "search_template")]
[Index("idx_{tablename}_01", $"{nameof(CreatedUserId)},{nameof(ModuleId)},{nameof(Name)}", true)]
public partial class SearchTemplateEntity : EntityVersion
{
    /// <summary>
    /// ģ��Id
    /// </summary>
    public long ModuleId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }

    /// <summary>
    /// ģ��
    /// </summary>
    [Column(StringLength = -1)]
    public string Template { get; set; }
}
