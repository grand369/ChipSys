namespace ChipSys.Admin.Core.Dto;

/// <summary>
/// ����ʽ
/// </summary>
public enum SortOrder
{
    Asc,
    Desc,
}

/// <summary>
/// ����
/// </summary>
public class SortInput
{
    /// <summary>
    /// ��������
    /// </summary>
    public string PropName { get; set; }

    /// <summary>
    /// ����ʽ
    /// </summary>
    public SortOrder? Order { get; set; }

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool? IsAscending => Order.HasValue && Order.Value == SortOrder.Asc;
}
