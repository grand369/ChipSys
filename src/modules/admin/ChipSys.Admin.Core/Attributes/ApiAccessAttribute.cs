namespace ChipSys.Admin.Core.Attributes;

/// <summary>
/// �ӿڷ���
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ApiAccessAttribute : Attribute
{
    /// <summary>
    /// Ĭ�� false�� ��������һ���ɷ��ʡ������� true ȫ������ɷ���
    /// </summary>
    public bool All { get; set; } = false;

    /// <summary>
    /// Ȩ�޵�
    /// </summary>
    public string[] Codes { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="codes">Ȩ�޵�</param>
    public ApiAccessAttribute(params string[] codes)
    {
        Codes = codes;
    }
}
