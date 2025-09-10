using FreeSql.Internal.Model;

namespace ChipSys.Admin.Core.Dto;

/// <summary>
/// ��ѯ��Ϣ����
/// </summary>
public abstract class QueryInput
{
    /// <summary>
    /// �߼���ѯ����
    /// </summary>
    public virtual DynamicFilterInfo DynamicFilter { get; set; } = null;

    /// <summary>
    /// �����б�
    /// </summary>
    public virtual List<SortInput>? SortList { get; set; }
}
