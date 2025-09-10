using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Org.Input;

/// <summary>
/// �޸�
/// </summary>
public class OrgUpdateInput : OrgAddInput
{
    /// <summary>
    /// ����Id
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ����")]
    public long Id { get; set; }
}
