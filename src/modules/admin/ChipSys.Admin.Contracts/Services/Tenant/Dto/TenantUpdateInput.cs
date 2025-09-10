using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Tenant.Dto;

/// <summary>
/// �޸�
/// </summary>
public partial class TenantUpdateInput : TenantAddInput
{
    /// <summary>
    /// �⻧Id
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ���⻧")]
    public override long Id { get; set; }
}
