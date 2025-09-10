using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Consts;

namespace ChipSys.Admin.Services.Pkg.Dto;

/// <summary>
/// �����ײ�Ȩ��
/// </summary>
public class PkgSetPkgPermissionsInput
{
    /// <summary>
    /// ƽ̨
    /// </summary>
    public string Platform { get; set; } = AdminConsts.WebName;

    [Required(ErrorMessage = "�ײͲ���Ϊ��")]
    public long PkgId { get; set; }

    [Required(ErrorMessage = "Ȩ�޲���Ϊ��")]
    public List<long> PermissionIds { get; set; }
}
