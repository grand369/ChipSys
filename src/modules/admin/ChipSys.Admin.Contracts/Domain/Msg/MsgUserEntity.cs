using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Admin.Domain.Msg;

/// <summary>
/// ��Ϣ�û�
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "msg_user")]
public partial class MsgUserEntity : EntityBase
{
    /// <summary>
    /// ��ϢId
    /// </summary>
    public long MsgId { get; set; }

    /// <summary>
    /// ��Ϣ
    /// </summary>
    [NotGen]
    [Navigate(nameof(MsgId))]
    public MsgEntity Msg { get; set; }

    /// <summary>
    /// �û�Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// �û�
    /// </summary>
    [NotGen]
    [Navigate(nameof(UserId))]
    public MsgUserEntity User { get; set; }

    /// <summary>
    /// �Ƿ��Ѷ�
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// �Ѷ�ʱ��
    /// </summary>
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// �Ƿ���Ҫ
    /// </summary>
    public bool IsImportant { get; set; }
}
