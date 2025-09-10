using ChipSys.Admin.Domain.Msg;

namespace ChipSys.Admin.Services.Msg.Dto;

/// <summary>
/// ��Ϣ��ҳ��Ӧ
/// </summary>
public class MsgGetPageOutput
{
    /// <summary>
    /// ��ϢId
    /// </summary>
    public long Id { get; set; }

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
    /// ��Ϣ״̬
    /// </summary>
    public MsgStatusEnum Status { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public virtual DateTime? CreatedTime { get; set; }
}
