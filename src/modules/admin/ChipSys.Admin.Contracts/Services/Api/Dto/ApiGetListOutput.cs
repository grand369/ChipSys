namespace ChipSys.Admin.Services.Api.Dto;

/// <summary>
/// �ӿ��б�
/// </summary>
public class ApiGetListOutput
{
    /// <summary>
    /// �ӿ�Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// �ӿڸ���
    /// </summary>
	public long? ParentId { get; set; }

    /// <summary>
    /// �ӿ�����
    /// </summary>
    public string Name { get; set; }

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
    /// ���ò�����־
    /// </summary>
    public bool EnabledLog { get; set; }

    /// <summary>
    /// �����������
    /// </summary>
    public bool EnabledParams { get; set; }

    /// <summary>
    /// ������Ӧ���
    /// </summary>
    public bool EnabledResult { get; set; }

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
