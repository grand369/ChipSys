namespace ChipSys.Common.Extensions;

public static class DateTimeExtension
{
    /// <summary>
    /// ʱ�����ʼ����
    /// </summary>
    public static readonly DateTime TimestampStart = new(1970, 1, 1, 0, 0, 0, 0);

    /// <summary>
    /// ת��Ϊʱ���
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="milliseconds">�Ƿ�ʹ�ú���</param>
    /// <returns></returns>
    public static long ToTimestamp(this DateTime dateTime, bool milliseconds = false)
    {
        var timestamp = dateTime.ToUniversalTime() - TimestampStart;
        return (long)(milliseconds ? timestamp.TotalMilliseconds : timestamp.TotalSeconds);
    }

    /// <summary>
    /// ��ȡ�ܼ�
    /// </summary>
    /// <param name="datetime"></param>
    /// <returns></returns>
    public static string GetWeekName(this DateTime datetime)
    {
        var day = (int)datetime.DayOfWeek;
        var week = new string[] { "����", "��һ", "�ܶ�", "����", "����", "����", "����" };
        return week[day];
    }
}
