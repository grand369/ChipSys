namespace ChipSys.Admin.Services.Email.Events;

/// <summary>
/// ���ŵ����¼�
/// </summary>
public class SmsSingleSendEvent
{
    /// <summary>
    /// �ֻ���
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    public string Text { get; set; }
}
