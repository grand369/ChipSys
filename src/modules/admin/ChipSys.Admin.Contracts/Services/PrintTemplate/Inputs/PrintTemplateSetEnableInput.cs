namespace ChipSys.Admin.Services.PrintTemplate.Inputs;

/// <summary>
/// ��������
/// </summary>
public class PrintTemplateSetEnableInput
{
    /// <summary>
    /// ��ӡģ��Id
    /// </summary>
    public long PrintTemplateId { get; set; }

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool Enabled { get; set; }
}
