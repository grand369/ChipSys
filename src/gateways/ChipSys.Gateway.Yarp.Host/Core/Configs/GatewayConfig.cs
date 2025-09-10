namespace ChipSys.Gateway.Yarp.Host.Core.Configs;

/// <summary>
/// ��������
/// </summary>
public class GatewayConfig
{
    public static class Models
    {
        /// <summary>
        /// ģ����Ϣ
        /// </summary>
        public class ModuleInfo
        {
            /// <summary>
            /// ����
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// ·��
            /// </summary>
            public string Url { get; set; }
        }

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
    /// ģ���б�
    /// </summary>
    public List<Models.ModuleInfo> ModuleList { get; set; }

    /// <summary>
    /// �����������
    /// </summary>
    public Models.HealthChecksConfig HealthChecks { get; set; } = new Models.HealthChecksConfig();
}
