using System.ComponentModel;

namespace ChipSys.Admin.Domain.UserStaff;

/// <summary>
/// �Ա�
/// </summary>
public enum Sex
{
    /// <summary>
    /// δ֪
    /// </summary>
    [Description("δ֪")]
    Unknown = 0,

    /// <summary>
    /// ��
    /// </summary>
    [Description("��")]
    Male = 1,

    /// <summary>
    /// Ů
    /// </summary>
    [Description("Ů")]
    Female = 2
}
