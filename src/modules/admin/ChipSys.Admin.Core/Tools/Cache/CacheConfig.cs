namespace ChipSys.Admin.Tools.Cache;

/// <summary>
/// ��������
/// </summary>
public class CacheConfig
{
    /// <summary>
    /// ��������
    /// </summary>
    public CacheType Type { get; set; } = CacheType.Memory;

    /// <summary>
    /// ������������
    /// </summary>
    public CacheType TypeRateLimit { get; set; } = CacheType.Memory;

    /// <summary>
    /// Redis����
    /// </summary>
    public RedisConfig Redis { get; set; } = new RedisConfig();
}

/// <summary>
/// Redis����
/// </summary>
public class RedisConfig
{
    /// <summary>
    /// �����ַ���
    /// </summary>
    public string ConnectionString { get; set; } = "127.0.0.1:6379,password=,defaultDatabase=0";

    /// <summary>
    /// ���������ַ���
    /// </summary>
    public string ConnectionStringRateLimit { get; set; } = "127.0.0.1:6379,password=,defaultDatabase=0";
}
