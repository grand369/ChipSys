using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.MsgType.Dto;

/// <summary>
/// �޸�
/// </summary>
public partial class MsgTypeUpdateInput : MsgTypeAddInput
{
    /// <summary>
    /// ��Ϣ����Id
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ����Ϣ����")]
    public long Id { get; set; }
}
