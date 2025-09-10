using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Doc.Dto;

/// <summary>
/// �����ĵ�����
/// </summary>
public class DocUpdateContentInput
{
    /// <summary>
    /// ���
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ���ĵ�")]
    public long Id { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Html
    /// </summary>
    public string Html { get; set; }
}
