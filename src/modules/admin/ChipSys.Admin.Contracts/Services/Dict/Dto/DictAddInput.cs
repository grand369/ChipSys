using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Dict.Dto;

/// <summary>
/// ����ֵ�
/// </summary>
public class DictAddInput
{
    /// <summary>
    /// �ϼ�Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// �ֵ�����Id
    /// </summary>
    [ValidateRequired(ErrorMessage = "��ѡ���ֵ�����")]
    public long DictTypeId { get; set; }

    /// <summary>
    /// �ֵ�����
    /// </summary>
    [Required(ErrorMessage = "�������ֵ�����")]
    public string Name { get; set; }

    /// <summary>
    /// �ֵ����
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// �ֵ�ֵ
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Enabled { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public int? Sort { get; set; }
}
