namespace ChipSys.Admin.Core.Configs;

/// <summary>
/// im����
/// </summary>
public class ImConfig
{
    /// <summary>
    /// ����
    /// </summary>
    public bool Enable { get; set; } = false;

    /// <summary>
    /// im��������Ⱥ��ַ
    /// </summary>
    public string[] Servers { get; set; }

    /// <summary>
    /// wsҵ��˵�ַ
    /// </summary>
    public string Server { get; set; }

    /// <summary>
    /// Redis�����ַ���
    /// </summary>
    public string RedisConnectionString { get; set; }
}
