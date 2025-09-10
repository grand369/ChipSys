using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;
using ChipSys.Admin.Domain.MsgType;

namespace ChipSys.Admin.Domain.Msg;

/// <summary>
/// ��Ϣ
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "msg")]
public partial class MsgEntity : EntityBase
{
    /// <summary>
    /// ����
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = -1)]
    public string Content { get; set; }

    /// <summary>
    /// ����Id
    /// </summary>
    public long TypeId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [NotGen]
    [Navigate(nameof(TypeId))]
    public MsgTypeEntity Type { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// ��Ϣ״̬
    /// </summary>
    public MsgStatusEnum Status { get; set; }
}
