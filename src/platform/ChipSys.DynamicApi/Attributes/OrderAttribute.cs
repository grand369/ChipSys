namespace ChipSys.DynamicApi.Attributes;

[Serializable]
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
public class OrderAttribute : Attribute
{
    /// <summary>
    /// ≈≈–Ú÷µ
    /// </summary>
    public int Value { get; set; } = 0;

    /// <summary>
    /// ≈≈–Ú…Ë÷√
    /// </summary>
    /// <param name="value">≈≈–Ú÷µ</param>
    public OrderAttribute(int value)
    {
        Value = value;
    }
}
