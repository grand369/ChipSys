namespace ChipSys.Admin.Services.View.Dto;

/// <summary>
/// ��ͼ�б�
/// </summary>
public class ViewGetListOutput
{
    /// <summary>
    /// ��ͼId
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// ��ͼ����
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// ƽ̨
    /// </summary>
    public string Platform { get; set; }

    /// <summary>
    /// ��ͼ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ��ͼ����
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// ��ͼ·��
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Cache { get; set; } = true;

    /// <summary>
    /// ����
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    public string Description { get; set; }
}
