namespace ChipSys.Admin.Services.Api.Dto;

/// <summary>
/// ���������������
/// </summary>
public class ApiSetEnableParamsInput
{
    /// <summary>
    /// �ӿ�Id
    /// </summary>
    public long ApiId { get; set; }

    /// <summary>
    /// �Ƿ������������
    /// </summary>
    public bool EnabledParams { get; set; }
}
