namespace ChipSys.DynamicApi.Attributes;

/// <summary>
/// ����ʽ���������
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class NonFormatResultAttribute : Attribute
{
}
