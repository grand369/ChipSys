using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.PrintTemplate.Inputs;

/// <summary>
/// �޸�
/// </summary>
public partial class PrintTemplateUpdateInput : PrintTemplateAddInput
{
    /// <summary>
    /// ��ӡģ��Id
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ���ӡģ��")]
    public long Id { get; set; }

    /// <summary>
    /// �汾
    /// </summary>
    public long Version { get; set; }
}
