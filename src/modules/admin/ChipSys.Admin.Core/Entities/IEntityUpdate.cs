namespace ChipSys.Admin.Core.Entities;

/// <summary>
/// �޸Ľӿ�
/// </summary>
public interface IEntityUpdate
{
    /// <summary>
    /// �޸���Id
    /// </summary>
    long? ModifiedUserId { get; set; }

    /// <summary>
    /// �޸���
    /// </summary>
    string ModifiedUserName { get; set; }

    /// <summary>
    /// �޸�������
    /// </summary>
    string ModifiedUserRealName { get; set; }

    /// <summary>
    /// �޸�ʱ��
    /// </summary>
    DateTime? ModifiedTime { get; set; }
}
