using Microsoft.AspNetCore.Http;

namespace ChipSys.Admin.Services.Doc.Dto;

/// <summary>
/// �ϴ�ͼƬ
/// </summary>
public class DocUploadImageInput
{
    /// <summary>
    /// �ϴ��ļ�
    /// </summary>
    public IFormFile File { get; set; }

    /// <summary>
    /// �ĵ����
    /// </summary>
    public long Id { get; set; }
}
