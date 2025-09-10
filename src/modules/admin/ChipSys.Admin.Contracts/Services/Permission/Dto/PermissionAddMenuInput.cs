namespace ChipSys.Admin.Services.Permission.Dto;

/// <summary>
/// ��Ӳ˵�
/// </summary>
public class PermissionAddMenuInput
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
    /// ��ͼ
    /// </summary>
    public long? ViewId { get; set; }

    /// <summary>
    /// ·������
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ·�ɵ�ַ
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Ȩ������
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Hidden { get; set; } = false;

    /// <summary>
    /// ͼ��
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// ���´���
    /// </summary>
    public bool NewWindow { get; set; } = false;

    /// <summary>
    /// ��������
    /// </summary>
    public bool External { get; set; } = false;

    /// <summary>
    /// �Ƿ񻺴�
    /// </summary>
    public bool IsKeepAlive { get; set; } = true;

    /// <summary>
    /// �Ƿ�̶�
    /// </summary>
    public bool IsAffix { get; set; } = false;

    /// <summary>
    /// ���ӵ�ַ
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    /// �Ƿ���Ƕ����
    /// </summary>
    public bool IsIframe { get; set; } = false;

    /// <summary>
    /// �Ƿ�ϵͳȨ��
    /// </summary>
    public bool IsSystem { get; set; } = false;

    /// <summary>
    /// ����
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; }
}
