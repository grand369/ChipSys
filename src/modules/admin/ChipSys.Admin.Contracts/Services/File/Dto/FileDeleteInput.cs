using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Dto;

/// <summary>
/// ɾ��
/// </summary>
public class FileDeleteInput
{
    /// <summary>
    /// �ļ�Id
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ���ļ�")]
    public long Id { get; set; }
}
