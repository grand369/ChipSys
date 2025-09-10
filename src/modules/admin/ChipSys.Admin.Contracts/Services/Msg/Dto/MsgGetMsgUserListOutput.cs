namespace ChipSys.Admin.Services.Msg.Dto;

/// <summary>
/// ��Ϣ�û��б�
/// </summary>
public class MsgGetMsgUserListOutput
{
    /// <summary>
    /// ����Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// �ֻ���
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// �Ƿ��Ѷ�
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// �Ѷ�ʱ��
    /// </summary>
    public DateTime? ReadTime { get; set; }
}
