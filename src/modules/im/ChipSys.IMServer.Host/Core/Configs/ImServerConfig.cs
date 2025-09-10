namespace ChipSys.IMServer.Host.Core.Configs;

/// <summary>
/// im���������
/// </summary>
public class ImServerConfig
{
    public static class Models
    {
        /// <summary>
        /// �����������
        /// </summary>
        public class HealthChecksConfig
        {
            /// <summary>
            /// ����
            /// </summary>
            public bool Enable { get; set; } = true;

            /// <summary>
            /// ����·��
            /// </summary>
            public string Path { get; set; } = "/health";
        }
    }

    /// <summary>
    /// Redis�ͻ��������ַ���
    /// </summary>
    public string RedisClientConnectionString { get; set; }

    /// <summary>
    /// ������б�
    /// </summary>
    public string[] Servers { get; set; }

    /// <summary>
    /// �����
    /// </summary>
    public string Server { get; set; }

    /// <summary>
    /// ����������ƣ�Ĭ��GB2312
    /// </summary>
    public string InputEncodingName { get; set; } = "GB2312";

    /// <summary>
    /// ����������ƣ�Ĭ��GB2312
    /// </summary>
    public string OutputEncodingName { get; set; } = "GB2312";

    /// <summary>
    /// �����������
    /// </summary>
    public Models.HealthChecksConfig HealthChecks { get; set; } = new Models.HealthChecksConfig();
}
