using ChipSys.Admin.Domain.Doc;

namespace ChipSys.Admin.Services.Doc.Dto;

/// <summary>
/// �ĵ��б�
/// </summary>
public class DocListOutput
{
    /// <summary>
    /// ���
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// �����ڵ�
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Label { get; set; }

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
    public string Description { get; set; }

    /// <summary>
    /// ���
    /// </summary>
    public bool? Opened { get; set; }
}
