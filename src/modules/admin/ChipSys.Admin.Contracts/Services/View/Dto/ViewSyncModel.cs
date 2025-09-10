namespace ChipSys.Admin.Services.View.Dto;

/// <summary>
/// ��ͼͬ��ģ��
/// </summary>
public class ViewSyncModel
{
    /// <summary>
    /// ��ͼ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ��ַ
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// ��ͼ����
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Cache { get; set; } = true;
}
