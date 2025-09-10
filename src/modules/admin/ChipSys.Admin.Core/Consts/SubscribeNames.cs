using System.ComponentModel;

namespace ChipSys.Admin.Core.Consts;

/// <summary>
/// ������
/// </summary>
public class SubscribeNames
{
    /// <summary>
    /// ���ŵ���
    /// </summary>
    [Description("���ŵ���")]
    public const string SmsSingleSend = "ChipSys.Admin.sms:singleSend";

    /// <summary>
    /// ������֤�뷢��
    /// </summary>
    [Description("������֤�뷢��")]
    public const string SmsSendCode = "ChipSys.Admin.sms:sendCode";

    /// <summary>
    /// �ʼ�����
    /// </summary>
    [Description("�ʼ�����")]
    public const string EmailSingleSend = "ChipSys.Admin.email:singleSend";

    /// <summary>
    /// ������֤�뷢��
    /// </summary>
    [Description("������֤�뷢��")]
    public const string EmailSendCode = "ChipSys.Admin.email:sendCode";

    /// <summary>
    /// �û�����ת��
    /// </summary>
    [Description("�û�����ת��")]
    public const string UserOrgChange = "ChipSys.Admin.user.orgChange";

    /// <summary>
    /// ��¼��־���
    /// </summary>
    [Description("��¼��־���")]
    public const string LoginLogAdd = "ChipSys.Admin.loginLog.add";

    /// <summary>
    /// ������־���
    /// </summary>
    [Description("������־���")]
    public const string OperationLogAdd = "ChipSys.Admin.operationLog.add";
}
