namespace ChipSys.Admin.Services.Permission.Dto;

/// <summary>
/// ��ѯ�б�
/// </summary>
public class PermissionGetListInput
{
    /// <summary>
    /// ƽ̨
    /// </summary>
    public string Platform { get; set; }

    /// <summary>
    /// ·�ɵ�ַ
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Ȩ������
    /// </summary>
    public string Label { get; set; }
}
