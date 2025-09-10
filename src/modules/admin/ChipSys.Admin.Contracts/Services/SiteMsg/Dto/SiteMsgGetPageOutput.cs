namespace ChipSys.Admin.Services.SiteMsg.Dto;

/// <summary>
/// վ����Ϣ��ҳ��Ӧ
/// </summary>
public class SiteMsgGetPageOutput
{
    /// <summary>
    /// ΨһId
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// ��ϢId
    /// </summary>
    public long MsgId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// ����Id
    /// </summary>
    public long TypeId { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// �Ƿ��Ѷ�
    /// </summary>
    public bool? IsRead { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public virtual DateTime? ReceivedTime { get; set; }
}
