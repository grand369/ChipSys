namespace ChipSys.Admin.Core.Attributes;

/// <summary>
/// ѩ���㷨����
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class SnowflakeAttribute : Attribute
{
    public bool Enable { get; set; } = true;
}
