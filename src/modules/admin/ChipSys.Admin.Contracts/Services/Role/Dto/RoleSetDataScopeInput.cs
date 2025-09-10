using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;
using ChipSys.Admin.Domain.Role;

namespace ChipSys.Admin.Services.Role.Dto;

/// <summary>
/// �������ݷ�Χ
/// </summary>
public class RoleSetDataScopeInput
{
    /// <summary>
    /// ��ɫId
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ���ɫ")]
    public long RoleId { get; set; }

    /// <summary>
    /// ���ݷ�Χ
    /// </summary>
    public DataScope DataScope { get; set; }

    /// <summary>
    /// ָ������
    /// </summary>
    public long[] OrgIds { get; set; }
}
