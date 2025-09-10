namespace ChipSys.Admin.Core.Attributes;

/// <summary>
/// �ӿڷ���
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public class ApiGroupAttribute : Attribute
{
    /// <summary>
    /// �Ƿ񲻷���
    /// </summary>
    public bool NonGroup { get; set; }

    /// <summary>
    /// ���������б�
    /// </summary>
    public string[] GroupNames { get; set; }

    public ApiGroupAttribute(params string[] groupNames)
    {
        GroupNames = groupNames;
    }
}
