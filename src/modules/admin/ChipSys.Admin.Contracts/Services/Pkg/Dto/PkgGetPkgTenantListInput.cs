namespace ChipSys.Admin.Services.Pkg.Dto;

/// <summary>
/// �ײ��⻧�б�����
/// </summary>
public partial class PkgGetPkgTenantListInput
{
    /// <summary>
    /// �⻧��
    /// </summary>
    public string TenantName { get; set; }

    /// <summary>
    /// �ײ�Id
    /// </summary>
    public long? PkgId { get; set; }
}
