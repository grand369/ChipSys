namespace ChipSys.Admin.Services.Api.Dto;

/// <summary>
/// ��������������־
/// </summary>
public class ApiSetEnableLogInput
{
    /// <summary>
    /// �ӿ�Id
    /// </summary>
    public long ApiId { get; set; }

    /// <summary>
    /// �Ƿ������������
    /// </summary>
    public bool EnabledLog { get; set; }
}
