namespace ChipSys.Admin.Domain.Dict.Dto;

/// <summary>
/// �ֵ��ҳ����
/// </summary>
public partial class DictGetPageInput
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
