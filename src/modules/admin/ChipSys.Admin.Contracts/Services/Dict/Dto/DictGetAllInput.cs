using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Domain.Dict.Dto;

/// <summary>
/// �ֵ��б�����
/// </summary>
public partial class DictGetAllInput: QueryInput
{
    /// <summary>
    /// �ֵ�����Id
    /// </summary>
    public long DictTypeId { get; set; }

    /// <summary>
    /// �ֵ�����
    /// </summary>
    public string Name { get; set; }
}
