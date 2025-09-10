using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.DictType.Dto;

/// <summary>
/// �ֵ�����
/// </summary>
public class DictTypeAddInput
{
    /// <summary>
    /// �ϼ�Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// �ֵ���������
    /// </summary>
    [Required(ErrorMessage = "�������ֵ���������")]
    public string Name { get; set; }

    /// <summary>
    /// �ֵ����ͱ���
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Enabled { get; set; }

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool IsTree { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public int Sort { get; set; }
}
