namespace ChipSys.Admin.Core.Auth;

/// <summary>
/// ������Ϣ
/// </summary>
public class TokenInfo
{
    private string _accessToken;

    /// <summary>
    /// ��������
    /// </summary>
    public string AccessToken
    {
        get => _accessToken;
        set
        {
            _accessToken = value;
        }
    }

    /// <summary>
    /// �������ƵĹ���ʱ��
    /// </summary>
    public DateTime AccessTokenExpiresAt { get; set; }

    /// <summary>
    /// �������Ƶ��������ڣ�����Ϊ��λ��
    /// </summary>
    public int AccessTokenLifeTime { get; set; }

    /// <summary>
    /// ˢ������
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// ˢ�����ƵĹ���ʱ��
    /// </summary>
    public DateTime RefreshTokenExpiresAt { get; set; }

    /// <summary>
    /// ˢ�����Ƶ��������ڣ�����Ϊ��λ��
    /// </summary>
    public int RefreshTokenLifeTime { get; set; }

    /// <summary>
    /// ����������Ϣʱ���
    /// </summary>
    public long Timestamp { get; set; }
}
