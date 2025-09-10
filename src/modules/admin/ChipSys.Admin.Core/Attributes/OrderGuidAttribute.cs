namespace ChipSys.Admin.Core.Attributes;

/// <summary>
/// ÅÅĞòGuidÌØĞÔ
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class OrderGuidAttribute : Attribute
{
    public bool Enable { get; set; } = true;
}
