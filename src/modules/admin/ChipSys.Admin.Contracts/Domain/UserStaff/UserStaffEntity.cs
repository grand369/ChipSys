using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Admin.Domain.UserStaff;

/// <summary>
/// �û�Ա��
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "user_staff", OldName = DbConsts.TableOldNamePrefix + "user_staff")]
public partial class UserStaffEntity : EntityTenant
{
    /// <summary>
    /// ְλ
    /// </summary>
    public string Position { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 20)]
    public string JobNumber { get; set; }

    /// <summary>
    /// �Ա�
    /// </summary>
    [Column(MapType = typeof(int?))]
    public Sex? Sex { get; set; }

    /// <summary>
    /// ��ְʱ��
    /// </summary>
    public DateTime? EntryTime { get; set; }

    /// <summary>
    /// ��ҵ΢����Ƭ
    /// </summary>
    [Column(StringLength = 500)]
    public string WorkWeChatCard { get; set; }

    /// <summary>
    /// ���˼��
    /// </summary>
    [Column(StringLength = 500)]
    public string Introduce { get; set; }
}
