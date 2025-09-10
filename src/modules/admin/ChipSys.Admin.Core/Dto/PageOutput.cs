namespace ChipSys.Admin.Core.Dto;

/// <summary>
/// ��ҳ��Ϣ���
/// </summary>
public class PageOutput<T>
{
    /// <summary>
    /// ��������
    /// </summary>
    public long Total { get; set; } = 0;

    /// <summary>
    /// ����
    /// </summary>
    public IList<T> List { get; set; }
}
