using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Dict.Dto;

/// <summary>
/// �޸�
/// </summary>
public class DictUpdateInput : DictAddInput
{
    /// <summary>
    /// ����Id
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ�������ֵ�")]
    public long Id { get; set; }
}
