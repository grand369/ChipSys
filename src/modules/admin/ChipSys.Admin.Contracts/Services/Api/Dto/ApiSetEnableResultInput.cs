namespace ChipSys.Admin.Services.Api.Dto;

/// <summary>
/// ���������������
/// </summary>
public class ApiSetEnableResultInput
{
    /// <summary>
    /// �ӿ�Id
    /// </summary>
    public long ApiId { get; set; }

    /// <summary>
    /// �Ƿ�������Ӧ���
    /// </summary>
    public bool EnabledResult { get; set; }
}
