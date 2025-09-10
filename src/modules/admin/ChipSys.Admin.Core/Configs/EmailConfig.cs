namespace ChipSys.Admin.Core.Configs;

/// <summary>
/// �ʼ�����
/// </summary>
public class EmailConfig
{
    public static class Models
    {
        /// <summary>
        /// ����
        /// </summary>
        public class EmailModel
        {
            /// <summary>
            /// ����
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// ��ַ
            /// </summary>
            public string Address { get; set; }
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// �˿�
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// ����SSL
    /// </summary>
    public bool UseSsl { get; set; }

    /// <summary>
    /// �����˺�
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// ������
    /// </summary>
    public Models.EmailModel FromEmail { get; set; }

    /// <summary>
    /// �ռ���
    /// </summary>
    public Models.EmailModel ToEmail { get; set; }
}
