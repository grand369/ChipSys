namespace ChipSys.Admin.Core.Attributes;

/// <summary>
/// ����Guid����
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class OrderGuidAttribute : Attribute
{
    public bool Enable { get; set; } = true;
}
