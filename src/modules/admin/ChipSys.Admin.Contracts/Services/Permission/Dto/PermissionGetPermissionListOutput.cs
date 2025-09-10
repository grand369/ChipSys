namespace ChipSys.Admin.Services.Permission.Dto;

/// <summary>
/// Ȩ���б�
/// </summary>
public class PermissionGetPermissionListOutput
{
    /// <summary>
    /// Ȩ��Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// �����ڵ�
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// Ȩ������
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// ����ʾ
    /// </summary>
    public bool Row { get; set; }

    /// <summary>
    /// Ȩ���б�
    /// </summary>
    public List<PermissionGetPermissionListOutput> Children { get; set; } = new List<PermissionGetPermissionListOutput>();
}
