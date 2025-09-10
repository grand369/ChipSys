using ChipSys.Admin.Domain.Msg;

namespace ChipSys.Admin.Services.Msg.Dto;

/// <summary>
/// ���
/// </summary>
public class MsgAddInput
{
    /// <summary>
    /// ����
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Content { get; set; }

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
}
