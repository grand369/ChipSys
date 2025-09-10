using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Msg.Dto;

/// <summary>
/// 修改
/// </summary>
public partial class MsgUpdateInput : MsgAddInput
{
    /// <summary>
    /// 消息Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择消息")]
    public long Id { get; set; }
}
