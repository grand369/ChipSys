using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.DictType.Dto;

/// <summary>
/// �޸�
/// </summary>
public class DictTypeUpdateInput : DictTypeAddInput
{
    /// <summary>
    /// ����Id
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ�������ֵ�����")]
    public long Id { get; set; }
}
