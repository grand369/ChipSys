using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Doc.Dto;

/// <summary>
/// ���²˵�
/// </summary>
public class DocUpdateMenuInput : DocAddMenuInput
{
    /// <summary>
    /// ���
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ��˵�")]
    public long Id { get; set; }
}
