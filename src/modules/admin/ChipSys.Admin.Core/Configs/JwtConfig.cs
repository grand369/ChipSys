namespace ChipSys.Admin.Core.Configs;

/// <summary>
/// Jwt����
/// </summary>
public class JwtConfig
{
    /// <summary>
    /// ������
    /// </summary>
    public string Issuer { get; set; } = "admin.core";

    /// <summary>
    /// ������
    /// </summary>
    public string Audience { get; set; } = "admin.core";

    /// <summary>
    /// ��Կ
    /// </summary>
    public string SecurityKey { get; set; }

    /// <summary>
    /// ��Ч��(����)
    /// </summary>
    public int Expires { get; set; } = 120;

    /// <summary>
    /// ˢ����Ч��(����)
    /// </summary>
    public int RefreshExpires { get; set; } = 480;
}
