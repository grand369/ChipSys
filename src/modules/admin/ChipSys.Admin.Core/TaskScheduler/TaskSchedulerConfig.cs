namespace ChipSys.Admin.Tools.TaskScheduler;

/// <summary>
/// �����������
/// </summary>
public class TaskSchedulerConfig
{
    public static class Models
    {
        /// <summary>
        /// �澯�ʼ�
        /// </summary>
        public class AlerEmail
        {
            /// <summary>
            /// �Ƿ�����
            /// </summary>
            public bool Enable { get; set; }

            /// <summary>
            /// �ʼ���ַ
            /// </summary>
            public string Address { get; set; }
        }

        /// <summary>
        /// ����������Ϣ
        /// </summary>
        public class ProcessStartInfoModel
        {
            /// <summary>
            /// Ӧ�ó���
            /// </summary>
            public string FileName { get; set; }

            /// <summary>
            /// ����Ŀ¼
            /// </summary>
            public string WorkingDirectory { get; set; }
        }

        /// <summary>
        /// ģ����Ϣ
        /// </summary>
        public class Modules
        {
            /// <summary>
            /// ģ������
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Http�����ַ
            /// </summary>
            public string HttpUrl { get; set; }

            /// <summary>
            /// Grpc�����ַ
            /// </summary>
            public string GrpcUrl { get; set; }
        }
    }

    /// <summary>
    /// ģ����Ϣ�б�
    /// </summary>
    public List<Models.Modules> Modules { get; set; } = [];

    /// <summary>
    /// ����������Ϣ
    /// </summary>
    public Models.ProcessStartInfoModel ProcessStartInfo { get; set; }

    /// <summary>
    /// �澯�ʼ�
    /// </summary>
    public Models.AlerEmail AlerEmail { get; set; }
}
