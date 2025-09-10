using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Pkg.Dto;

/// <summary>
/// ����ײ��⻧�б�
/// </summary>
public class PkgAddPkgTenantListInput
{
    /// <summary>
    /// �ײ�
    /// </summary>
    [Required(ErrorMessage = "��ѡ���ײ�")]
    public long PkgId { get; set; }

    /// <summary>
    /// �⻧�б�
    /// </summary>
    public long[] TenantIds { get; set; }
}
