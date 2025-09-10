using ChipSys.Admin.Domain.UserStaff;

namespace ChipSys.Admin.Domain.User;

/// <summary>
/// Ա�����
/// </summary>
public class StaffAddInput
{
    /// <summary>
    /// ����
    /// </summary>
    public string JobNumber { get; set; }

    /// <summary>
    /// ְλ
    /// </summary>
    public string Position { get; set; }

    /// <summary>
    /// �Ա�
    /// </summary>
    public Sex? Sex { get; set; }

    /// <summary>
    /// ��ְʱ��
    /// </summary>
    public DateTime? EntryTime { get; set; }

    /// <summary>
    /// ��ҵ΢����Ƭ
    /// </summary>
    public string WorkWeChatCard { get; set; }

    /// <summary>
    /// ���˼��
    /// </summary>
    public string Introduce { get; set; }
}
