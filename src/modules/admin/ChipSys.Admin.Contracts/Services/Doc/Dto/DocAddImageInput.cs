namespace ChipSys.Admin.Services.Doc.Dto;

/// <summary>
/// ���ͼƬ
/// </summary>
public class DocAddImageInput
{
    /// <summary>
    /// �û�Id
    /// </summary>
    public long DocumentId { get; set; }

    /// <summary>
    /// ����·��
    /// </summary>
    public string Url { get; set; }
}
