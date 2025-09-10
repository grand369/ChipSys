using ChipSys.Admin.Domain.Doc;

namespace ChipSys.Admin.Services.Doc.Dto;

/// <summary>
/// ��ӷ���
/// </summary>
public class DocAddGroupInput
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
    public string Label { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ��
    /// </summary>
    public bool? Opened { get; set; }
}
