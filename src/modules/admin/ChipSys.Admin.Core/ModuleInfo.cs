using System.Reflection;

namespace ChipSys.Admin.Core;

/// <summary>
/// ģ����Ϣ
/// </summary>
public class ModuleInfo
{
    /// <summary>
    /// ��������
    /// </summary>
    public Assembly Assembly { get; set; }

    /// <summary>
    /// �������ļ�����
    /// </summary>
    public Type LocalizerType { get; set; }
}
