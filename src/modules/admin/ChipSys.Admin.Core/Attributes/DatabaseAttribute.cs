namespace ChipSys.Admin.Core.Attributes;

/// <summary>
/// ���ݿ�
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class DatabaseAttribute : Attribute
{
    /// <summary>
    /// ���ݿ�����
    /// </summary>
    public string Name { get; set; }

    public DatabaseAttribute(string name)
    {
        Name = name;
    }
}
