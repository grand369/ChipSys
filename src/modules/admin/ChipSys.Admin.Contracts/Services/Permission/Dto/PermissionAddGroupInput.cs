namespace ChipSys.Admin.Services.Permission.Dto;

/// <summary>
/// ��������
/// </summary>
public class PermissionAddGroupInput
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
    /// ����·�ɵ�ַ
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// �ض����ַ
    /// </summary>
    public string Redirect { get; set; }

    /// <summary>
    /// Ȩ������
    /// </summary>
    public string Label { get; set; }

    ///// <summary>
    ///// ˵��
    ///// </summary>
    //public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Hidden { get; set; } = false;

    /// <summary>
    /// ͼ��
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// չ��
    /// </summary>
    public bool Opened { get; set; } = true;

    /// <summary>
    /// ����
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; }
}
