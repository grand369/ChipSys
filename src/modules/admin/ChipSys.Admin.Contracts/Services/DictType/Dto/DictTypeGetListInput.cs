using ChipSys.Admin.Core.Dto;
namespace ChipSys.Admin.Services.DictType.Dto;

/// <summary>
/// �ֵ������б�����
/// </summary>
public partial class DictTypeGetListInput: QueryInput
{
    /// <summary>
    /// �ֵ�����
    /// </summary>
    public string Name { get; set; }
}
