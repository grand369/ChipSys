using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Doc.Dto;

/// <summary>
/// ���·���
/// </summary>
public class DocUpdateGroupInput : DocAddGroupInput
{
    /// <summary>
    /// ���
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ�����")]
    public long Id { get; set; }
}
