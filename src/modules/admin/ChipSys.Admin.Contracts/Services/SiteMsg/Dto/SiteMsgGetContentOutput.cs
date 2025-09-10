namespace ChipSys.Admin.Services.SiteMsg.Dto;

/// <summary>
/// ��Ϣ����
/// </summary>
public class SiteMsgGetContentOutput
{
    /// <summary>
    /// ��ϢId
    /// </summary>
    public long MsgId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public virtual DateTime? ReceivedTime { get; set; }

    /// <summary>
    /// �Ƿ��Ѷ�
    /// </summary>
    public bool? IsRead { get; set; }
}
