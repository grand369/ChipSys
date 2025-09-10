using ChipSys.Admin.Core.Entities;
using FreeSql.DataAnnotations;

namespace ChipSys.Admin.Domain.MsgType;

/// <summary>
/// ��Ϣ����
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "msg_type")]
[Index("idx_{tablename}_01", $"{nameof(ParentId)},{nameof(Name)}", true)]
public partial class MsgTypeEntity : EntityBase
{
    /// <summary>
    /// ����Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// �Ӽ��б�
    /// </summary>
    [Navigate(nameof(ParentId))]
    public List<MsgTypeEntity> Childs { get; set; }

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
    /// ����
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// ����
    /// </summary>
	public int Sort { get; set; }
}
