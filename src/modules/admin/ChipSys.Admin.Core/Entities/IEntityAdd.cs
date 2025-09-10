namespace ChipSys.Admin.Core.Entities;

/// <summary>
/// ��ӽӿ�
/// </summary>
public interface IEntityAdd
{
    /// <summary>
    /// �������û�Id
    /// </summary>
    long? CreatedUserId { get; set; }

    /// <summary>
    /// ������
    /// </summary>
    string CreatedUserName { get; set; }

    /// <summary>
    /// ����������
    /// </summary>
    string CreatedUserRealName { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    DateTime? CreatedTime { get; set; }
}
