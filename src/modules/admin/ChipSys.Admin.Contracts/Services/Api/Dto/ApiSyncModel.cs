namespace ChipSys.Admin.Services.Api.Dto;

/// <summary>
/// �ӿ�ͬ��ģ��
/// </summary>
public class ApiSyncModel
{
    /// <summary>
    /// �ӿ�����
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// �ӿڵ�ַ
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// ����·��
    /// </summary>
    public string ParentPath { get; set; }

    /// <summary>
    /// �ӿ��ύ����
    /// </summary>
    public string HttpMethods { get; set; }
}
