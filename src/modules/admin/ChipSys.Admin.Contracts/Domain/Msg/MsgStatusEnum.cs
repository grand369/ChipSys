namespace ChipSys.Admin.Domain.Msg;

/// <summary>
/// ��Ϣ״̬
/// </summary>
public enum MsgStatusEnum
{
    /// <summary>
    /// �ݸ�
    /// </summary>
    Draft = 1,

    /// <summary>
    /// �ѷ���
    /// </summary>
    Published = 2,

    /// <summary>
    /// ��ʱ����
    /// </summary>
    Scheduled = 3,

    /// <summary>
    /// �ѳ���
    /// </summary>
    Revoked = 4,

    /// <summary>
    /// �ѹ鵵
    /// </summary>
    Archived = 5,
}
