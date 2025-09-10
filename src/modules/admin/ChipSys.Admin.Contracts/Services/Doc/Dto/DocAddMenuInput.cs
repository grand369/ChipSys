using ChipSys.Admin.Domain.Doc;

namespace ChipSys.Admin.Services.Doc.Dto;

/// <summary>
/// ��Ӳ˵�
/// </summary>
public class DocAddMenuInput
{
    /// <summary>
    /// �����ڵ�
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public DocType Type { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    public string Description { get; set; }
}
