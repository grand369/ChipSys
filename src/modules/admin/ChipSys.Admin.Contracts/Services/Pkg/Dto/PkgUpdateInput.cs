using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Pkg.Dto;

/// <summary>
/// �޸�
/// </summary>
public partial class PkgUpdateInput : PkgAddInput
{
    /// <summary>
    /// �ײ�Id
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ���ײ�")]
    public long Id { get; set; }
}
