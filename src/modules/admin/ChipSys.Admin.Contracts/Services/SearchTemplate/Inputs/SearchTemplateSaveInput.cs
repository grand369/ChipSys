namespace ChipSys.Admin.Services.SearchTemplate.Inputs;

/// <summary>
/// ��������
/// </summary>
public class SearchTemplateSaveInput
{
    /// <summary>
    /// ģ��Id
    /// </summary>
    public long ModuleId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ģ��
    /// </summary>
    public string Template { get; set; }

    /// <summary>
    /// �汾
    /// </summary>
    public long Version { get; set; }
}
