namespace ChipSys.Admin.Services.Permission.Dto;

/// <summary>
/// ���Ȩ�޵�
/// </summary>
public class PermissionAddDotInput
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
    /// �����ӿ�
    /// </summary>
    public List<long> ApiIds { get; set; }

    /// <summary>
    /// Ȩ������
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// Ȩ�ޱ���
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// ˵��
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// ͼ��
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; }
}
