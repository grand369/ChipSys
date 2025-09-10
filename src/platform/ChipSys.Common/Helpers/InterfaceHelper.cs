using System.Reflection;

namespace ChipSys.Common.Helpers;

/// <summary>
/// ½Ó¿Ú°ïÖúÀà
/// </summary>
public class InterfaceHelper
{
    public static string[] GetPropertyNames<T>() where T : class
    {
        Type interfaceType = typeof(T);

        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException($"{interfaceType.FullName} is not an interface type.");
        }

        PropertyInfo[] properties = interfaceType.GetProperties();

        string[] propertyNames = properties.Select(p => p.Name).ToArray();

        return propertyNames;
    }
}
