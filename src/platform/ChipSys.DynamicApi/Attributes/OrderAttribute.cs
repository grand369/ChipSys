namespace ChipSys.DynamicApi.Attributes;

[Serializable]
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
public class OrderAttribute : Attribute
{
    /// <summary>
    /// ����ֵ
    /// </summary>
    public int Value { get; set; } = 0;

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="value">����ֵ</param>
    public OrderAttribute(int value)
    {
        Value = value;
    }
}
