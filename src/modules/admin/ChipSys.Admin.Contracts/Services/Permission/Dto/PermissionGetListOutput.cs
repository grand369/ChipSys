using ChipSys.Admin.Domain.Permission;

namespace ChipSys.Admin.Services.Permission.Dto;

/// <summary>
/// Ȩ���б�
/// </summary>
public class PermissionGetListOutput
{
    /// <summary>
    /// Ȩ��Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// ƽ̨
    /// </summary>
    public string Platform { get; set; }

    /// <summary>
    /// �����ڵ�
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// Ȩ������
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// Ȩ������
    /// </summary>
    public PermissionType Type { get; set; }

    /// <summary>
    ///·�ɵ�ַ
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// �ض����ַ
    /// </summary>
    public string Redirect { get; set; }

    /// <summary>
    /// ��ͼ��ַ
    /// </summary>
    public string ViewPath { get; set; }

    /// <summary>
    /// ���ӵ�ַ
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    /// �ӿ�·��
    /// </summary>
    public string ApiPaths { get; set; }

    /// <summary>
    /// ͼ��
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// չ��
    /// </summary>
    public bool Opened { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public int? Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// ����
    /// </summary>
	public bool Enabled { get; set; }
}
