namespace ChipSys.Admin.Core.Records;

/// <summary>
/// λ����Ϣ
/// </summary>
public record LocationInfo
{
    /// <summary>
    /// ����
    /// </summary>
    public string Country { get; init; }
    /// <summary>
    /// ʡ��
    /// </summary>
    public string Province { get; init; }

    /// <summary>
    /// ����
    /// </summary>
    public string City { get; init; }

    /// <summary>
    /// ���������
    /// </summary>
    public string Isp { get; init; }

    /// <summary>
    /// ת����ַ��Ϣ
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static LocationInfo Parse(string input)
    {
        var parts = input.Split('|');

        if (parts.Length >= 5)
        {
            return new LocationInfo
            {
                Country = parts[0] != "0" ? parts[0] : "",
                Province = parts[2] != "0" ? parts[2] : "",
                City = parts[3] != "0" ? parts[3] : "",
                Isp = parts[4] != "0" ? parts[4] : "",
            };
        }

        return new LocationInfo();
    }
}
