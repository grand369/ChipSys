namespace ChipSys.Admin.Core.Configs;

/// <summary>
/// Զ�̹��̵�������
/// </summary>
public class RpcConfig
{
    public static class Models
    {
        /// <summary>
        /// HttpԶ������
        /// </summary>
        public class HttpModel
        {
            /// <summary>
            /// ����
            /// </summary>
            public bool Enable { get; set; } = true;

            private string[] _assemblyNames;

            /// <summary>
            /// ��������
            /// </summary>
            public string[] AssemblyNames
            {
                get => _assemblyNames;
                set
                {
                    _assemblyNames = value;
                    if (!value.Contains("ChipSys.Admin.Contracts"))
                    {
                        _assemblyNames = [.. _assemblyNames, "ChipSys.Admin.Contracts"];
                    }
                }
            }
        }

        /// <summary>
        /// GrpcԶ������
        /// </summary>
        public class GrpcModel
        {
            /// <summary>
            /// ����
            /// </summary>
            public bool Enable { get; set; } = true;

            private string[] _assemblyNames;

            /// <summary>
            /// ��������
            /// </summary>
            public string[] AssemblyNames
            {
                get => _assemblyNames;
                set
                {
                    _assemblyNames = value;
                    if (!value.Contains("ChipSys.Admin.Core"))
                    {
                        _assemblyNames = [.. _assemblyNames, "ChipSys.Admin.Core"];
                    }
                }
            }

            /// <summary>
            /// ����˳�������?
            /// </summary>
            public string[] ServerAssemblyNames { get; set; }
        }

        /// <summary>
        /// ��ַ
        /// </summary>
        public class Endpoint
        {
            /// <summary>
            /// ģ������
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Http��ַ
            /// </summary>
            public string HttpUrl { get; set; }

            /// <summary>
            /// Grpc��ַ
            /// </summary>
            public string GrpcUrl { get; set; }
        }
    }

    /// <summary>
    /// HttpԶ������
    /// </summary>
    public Models.HttpModel Http { get; set; } = new Models.HttpModel();

    /// <summary>
    /// GrpcԶ������
    /// </summary>
    public Models.GrpcModel Grpc { get; set; } = new Models.GrpcModel();

    /// <summary>
    /// ��ַ�б�
    /// </summary>
    public List<Models.Endpoint> Endpoints { get; set; }
}
