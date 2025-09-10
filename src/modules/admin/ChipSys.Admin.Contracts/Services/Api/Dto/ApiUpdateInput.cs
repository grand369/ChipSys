using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.Api.Dto;

/// <summary>
/// �޸�
/// </summary>
public partial class ApiUpdateInput : ApiAddInput
{
    /// <summary>
    /// �ӿ�Id
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ��ӿ�")]
    public long Id { get; set; }
}
