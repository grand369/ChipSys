namespace ChipSys.Admin.Services.SiteMsg.Dto;

/// <summary>
/// վ����Ϣ��ҳ����
/// </summary>
public partial class SiteMsgGetPageInput
{
    /// <summary>
    /// �Ƿ��Ѷ�
    /// </summary>
    public bool? IsRead { get; set; }

    /// <summary>
    /// ����Id
    /// </summary>
    public long? TypeId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Title { get; set; }
}
