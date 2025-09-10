namespace ChipSys.Admin.Services.Api.Dto;

/// <summary>
/// ö��
/// </summary>
public class ApiGetEnumsOutput
{
    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// ����
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// ѡ���б�
    /// </summary>
    public List<Models.Options> Options { get; set; }

    public static class Models
    {
        /// <summary>
        /// ѡ��
        /// </summary>
        public class Options
        {
            /// <summary>
            /// ����
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// ����
            /// </summary>
            public string Desc { get; set; }

            /// <summary>
            /// ֵ
            /// </summary>
            public long Value { get; set; }
        }
    }
}
