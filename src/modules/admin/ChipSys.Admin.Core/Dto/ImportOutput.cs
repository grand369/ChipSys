namespace ChipSys.Admin.Core.Dto;

/// <summary>
/// ������Ϣ���
/// </summary>
public class ImportOutput
{
    /// <summary>
    /// ��������
    /// </summary>
    public long Total { get; set; } = 0;

    /// <summary>
    /// ������
    /// </summary>
    public long InsertCount { get; set; } = 0;

    /// <summary>
    /// ������
    /// </summary>
    public long UpdateCount { get; set; } = 0;
}
