using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.PrintTemplate.Inputs;

/// <summary>
/// �޸�ģ��
/// </summary>
public class PrintTemplateUpdateTemplateInput
{
    /// <summary>
    /// ��ӡģ��Id
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ���ӡģ��")]
    public long Id { get; set; }

    /// <summary>
    /// ģ��
    /// </summary>
    public string Template { get; set; }

    /// <summary>
    /// ��ӡ����
    /// </summary>
    public string PrintData { get; set; }

    /// <summary>
    /// �汾
    /// </summary>
    public long Version { get; set; }
}
