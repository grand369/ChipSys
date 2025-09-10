using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Email.Events;

/// <summary>
/// �ʼ�����
/// </summary>
public class EmailSingleSendEvent
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
    /// ������
    /// </summary>
    public Models.EmailModel? FromEmail { get; set; }

    /// <summary>
    /// �ռ���
    /// </summary>
    [Required(ErrorMessage = "�������ռ���")]
    public Models.EmailModel ToEmail { get; set; }

    /// <summary>
    /// �ʼ�����
    /// </summary>
    [Required(ErrorMessage = "�������ʼ�����")]
    public string Subject { get; set; }

    /// <summary>
    /// �ʼ�����
    /// </summary>
    [Required(ErrorMessage = "�������ʼ�����")]
    public string Body { get; set; }
}
