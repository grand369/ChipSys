using System.Text.Json.Serialization;

namespace ChipSys.Admin.Services.Auth.Dto;

/// <summary>
/// �û��˵�
/// </summary>
public class AuthUserMenuOutput
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
    /// ·�ɵ�ַ
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// ·������
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ��ͼ��ַ
    /// </summary>
    public string ViewPath { get; set; }

    /// <summary>
    /// �ض����ַ
    /// </summary>
    public string Redirect { get; set; }

    /// <summary>
    /// Ȩ������
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// ͼ��
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// ��
    /// </summary>
    public bool? Opened { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// ���´���
    /// </summary>
    public bool? NewWindow { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    public bool? External { get; set; }

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
    /// ����
    /// </summary>
    [JsonIgnore]
    public int? Sort { get; set; }
}
