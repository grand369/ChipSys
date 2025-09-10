using ChipSys.Admin.Domain.Role;

namespace ChipSys.Admin.Domain.User.Dto;

public class DataPermissionOutput
{
    /// <summary>
    /// ����Id
    /// </summary>
    public long OrgId { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    public string OrgName { get; set; }
    /// <summary>
    /// �����б�
    /// </summary>
    public List<long> OrgIds { get; set; }

    /// <summary>
    /// ���ݷ�Χ
    /// </summary>
    public DataScope DataScope { get; set; } = DataScope.Self;
}
