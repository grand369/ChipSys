namespace ChipSys.Admin.Services.Tenant.Dto;

/// <summary>
/// ��������
/// </summary>
public class TenantSetEnableInput
{
    /// <summary>
    /// �⻧Id
    /// </summary>
    public long TenantId { get; set; }

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool Enabled { get; set; }
}
