using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Msg.Dto;

/// <summary>
/// �޸�
/// </summary>
public partial class MsgUpdateInput : MsgAddInput
{
    /// <summary>
    /// ��ϢId
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ����Ϣ")]
    public long Id { get; set; }
}
