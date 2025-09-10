using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Msg.Dto;

/// <summary>
/// �����Ϣ�û��б�
/// </summary>
public class MsgAddMsgUserListInput
{
    /// <summary>
    /// ��Ϣ
    /// </summary>
    [Required(ErrorMessage = "��ѡ����Ϣ")]
    public long MsgId { get; set; }

    /// <summary>
    /// �û�
    /// </summary>
    public long[] UserIds { get; set; }
}
