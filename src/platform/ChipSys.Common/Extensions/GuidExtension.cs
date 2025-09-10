namespace ChipSys.Common.Extensions;

public static class GuidExtension
{
    /// <summary>
    /// �ж�Guid�Ƿ�Ϊ��
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static bool IsNull(this Guid s)
    {
        return s == Guid.Empty;
    }

    /// <summary>
    /// �ж�Guid�Ƿ�Ϊ��
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static bool NotNull(this Guid s)
    {
        return s != Guid.Empty;
    }
}
