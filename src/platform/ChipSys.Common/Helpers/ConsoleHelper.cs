namespace ChipSys.Common.Helpers;

public static class ConsoleHelper
{
    private static void WriteColorLine(string str, ConsoleColor color)
    {
        ConsoleColor currentForeColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(str);
        Console.ForegroundColor = currentForeColor;
    }

    /// <summary>
    /// ��ӡ������Ϣ
    /// </summary>
    /// <param name="str">����ӡ���ַ���</param>
    /// <param name="color">��Ҫ��ӡ����ɫ</param>
    public static void WriteErrorLine(this string str, ConsoleColor color = ConsoleColor.Red)
    {
        WriteColorLine(str, color);
    }

    /// <summary>
    /// ��ӡ������Ϣ
    /// </summary>
    /// <param name="str">����ӡ���ַ���</param>
    /// <param name="color">��Ҫ��ӡ����ɫ</param>
    public static void WriteWarningLine(this string str, ConsoleColor color = ConsoleColor.Yellow)
    {
        WriteColorLine(str, color);
    }

    /// <summary>
    /// ��ӡ������Ϣ
    /// </summary>
    /// <param name="str">����ӡ���ַ���</param>
    /// <param name="color">��Ҫ��ӡ����ɫ</param>
    public static void WriteInfoLine(this string str, ConsoleColor color = ConsoleColor.White)
    {
        WriteColorLine(str, color);
    }

    /// <summary>
    /// ��ӡ�ɹ�����Ϣ
    /// </summary>
    /// <param name="str">����ӡ���ַ���</param>
    /// <param name="color">��Ҫ��ӡ����ɫ</param>
    public static void WriteSuccessLine(this string str, ConsoleColor color = ConsoleColor.Green)
    {
        WriteColorLine(str, color);
    }
}
