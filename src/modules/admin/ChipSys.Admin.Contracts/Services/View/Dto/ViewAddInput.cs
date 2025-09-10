namespace ChipSys.Admin.Services.View.Dto;

/// <summary>
/// ���
/// </summary>
public class ViewAddInput
{
    /// <summary>
    /// ƽ̨
    /// </summary>
    public string Platform { get; set; }

    /// <summary>
    /// �����ڵ�
    /// </summary>
	public long ParentId { get; set; }

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
    /// ˵��
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Cache { get; set; } = true;

    /// <summary>
    /// ����
    /// </summary>
    public int? Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; } = true;
}
