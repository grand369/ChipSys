namespace ChipSys.Admin.Core;

/// <summary>
/// Ӧ������
/// </summary>
public class AppSettings
{
    /// <summary>
    /// ʹ����������
    /// </summary>
    public bool UseConfigCenter { get; set; } = false;

    /// <summary>
    /// ��������·��
    /// </summary>
    public string ConfigCenterPath { get; set; } = "ConfigCenter";
}
