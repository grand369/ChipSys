using System.ComponentModel;
using System.Reflection;

namespace ChipSys.Common.Extensions;

/// <summary>
/// ¿‡–Õ¿©’π
/// </summary>
public static class TypeExtensions
{
    public static string ToDescription(this Type type)
    {
        var desc = type?.GetCustomAttribute<DescriptionAttribute>(false);
        return desc?.Description;
    }
}
