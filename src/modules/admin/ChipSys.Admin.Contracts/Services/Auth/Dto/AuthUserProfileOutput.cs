using System.Text.Json.Serialization;

namespace ChipSys.Admin.Services.Auth.Dto;

/// <summary>
/// �û�������Ϣ
/// </summary>
public class AuthUserProfileOutput
{
    /// <summary>
    /// �˺�
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// �ֻ���
    /// </summary>
    [JsonIgnore]
    public string Mobile { get; set; }

    /// <summary>
    /// �ǳ�
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// ͷ��
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// ��ҵ
    /// </summary>
    public string CorpName { get; set; }

    /// <summary>
    /// ְλ
    /// </summary>
    public string Position { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    public string DeptName { get; set; }

    /// <summary>
    /// ˮӡ�İ�
    /// </summary>
    public string WatermarkText { get; set; }
}
