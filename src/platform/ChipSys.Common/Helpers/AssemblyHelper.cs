using System.Reflection;

namespace ChipSys.Common.Helpers;

/// <summary>
/// ���򼯰�����
/// </summary>
public class AssemblyHelper
{
    /// <summary>
    /// ���ݳ��������жϳ����Ƿ����
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <returns></returns>
    public static bool Exists(string assemblyName)
    {
        try
        {
            Assembly.Load(assemblyName);
            return true;
        }
        catch (FileNotFoundException)
        {
            return false;
        }
    }

    /// <summary>
    /// ���ݳ��������б��ȡ����
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <returns></returns>
    public static Assembly GetAssembly(string assemblyName)
    {
        Assembly assembly = null;

        if (assemblyName.IsNull())
        {
            return assembly;
        }

        try
        {
            assembly = Assembly.Load(assemblyName);
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("���򼯲����ڣ�" + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("���س���ʱ����" + ex.Message);
        }

        return assembly;
    }

    /// <summary>
    /// ���ݳ��������б��ȡ�����б�
    /// </summary>
    /// <param name="assemblyNames"></param>
    /// <returns></returns>
    public static Assembly[] GetAssemblyList(string[] assemblyNames)
    {
        List<Assembly> assemblies = [];

        if (!(assemblyNames?.Length > 0))
        {
            return [.. assemblies];
        }

        foreach (var assemblyName in assemblyNames)
        {
            var assembly = GetAssembly(assemblyName);
            if (assembly != null)
            {
                assemblies.Add(assembly);
            }
        }

        return [.. assemblies];
    }
}
