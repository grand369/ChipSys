using System.ComponentModel;

namespace ChipSys.Common.Files;

/// <summary>
/// �ļ���С��λ
/// </summary>
public enum FileSizeUnit
{
    /// <summary>
    /// �ֽ�
    /// </summary>
    [Description("B")]
    Byte,

    /// <summary>
    /// K�ֽ�
    /// </summary>
    [Description("KB")]
    K,

    /// <summary>
    /// M�ֽ�
    /// </summary>
    [Description("MB")]
    M,

    /// <summary>
    /// G�ֽ�
    /// </summary>
    [Description("GB")]
    G
}
