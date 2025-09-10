namespace ChipSys.Admin.Services.Api.Dto;

/// <summary>
/// ���
/// </summary>
public class ApiAddInput
{
    /// <summary>
    /// ����ģ��
    /// </summary>
	public long? ParentId { get; set; }

    /// <summary>
    /// �ӿ�����
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// �ӿڵ�ַ
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// �ӿ��ύ����
    /// </summary>
    public string HttpMethods { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; }
}
