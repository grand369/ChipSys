namespace ChipSys.Admin.Core.Attributes;

/// <summary>
/// Http�ӿڿͻ�����Լ
/// </summary>
[AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
public sealed class HttpClientContractAttribute: Attribute
{
    /// <summary>
    /// ģ����
    /// </summary>
    public string ModuleName { get; set; }

    public HttpClientContractAttribute(string moduleName)
    {
        ModuleName = moduleName;
    }
}
