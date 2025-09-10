using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Email.Events;

/// <summary>
/// ������֤��
/// </summary>
public class EmailSendCodeEvent
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
    /// �ռ���
    /// </summary>
    [Required(ErrorMessage = "�������ռ���")]
    public Models.EmailModel ToEmail { get; set; }

    /// <summary>
    /// ��֤��
    /// </summary>
    public string Code { get; set; }
}
