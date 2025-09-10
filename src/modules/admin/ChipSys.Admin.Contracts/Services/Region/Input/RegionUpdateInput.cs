using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Region;


/// <summary>
/// �޸�
/// </summary>
public class RegionUpdateInput : RegionAddInput
{
    /// <summary>
    /// ����Id
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ�����")]
    public long Id { get; set; }
}
