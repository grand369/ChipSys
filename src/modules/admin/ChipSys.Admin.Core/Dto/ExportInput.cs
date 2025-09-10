namespace ChipSys.Admin.Core.Dto;

/// <summary>
/// ������Ϣ����
/// </summary>
public class ExportInput: QueryInput
{
    /// <summary>
    /// �ļ���
    /// </summary>
    public string FileName { get; set; }
}

/// <summary>
/// ������Ϣ����
/// </summary>
/// <typeparam name="T">��������</typeparam>
public class ExportInput<T>: ExportInput
{
    /// <summary>
    /// ��ѯ����
    /// </summary>
    public virtual T Filter { get; set; }
}
