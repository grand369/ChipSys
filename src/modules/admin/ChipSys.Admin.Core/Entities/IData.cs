namespace ChipSys.Admin.Core.Entities;

/// <summary>
/// ����Ȩ�޽ӿ�
/// </summary>
public interface IData
{
    /// <summary>
    /// ӵ����Id
    /// </summary>
    long? OwnerId { get; set; }

    /// <summary>
    /// ӵ���߲���Id
    /// </summary>
    long? OwnerOrgId { get; set; }

    /// <summary>
    /// ӵ���߲�������
    /// </summary>
    string OwnerOrgName { get; set; }
}
