namespace ChipSys.Admin.Domain.Role;

/// <summary>
/// ���ݷ�Χ
/// </summary>
public enum DataScope
{
    /// <summary>
    /// ȫ��
    /// </summary>
    All = 1,

    /// <summary>
    /// �����ź��¼�����
    /// </summary>
    DeptWithChild = 2,

    /// <summary>
    /// ������
    /// </summary>
    Dept = 3,

    /// <summary>
    /// ��������
    /// </summary>
    Self = 4,

    /// <summary>
    /// ָ������
    /// </summary>
    Custom = 5
}
