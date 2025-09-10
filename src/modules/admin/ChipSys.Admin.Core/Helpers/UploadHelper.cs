using Microsoft.AspNetCore.Http;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Core.Helpers;

/// <summary>
/// �ļ��ϴ�������
/// </summary>
[InjectSingleton]
public class UploadHelper
{
    /// <summary>
    /// �����ļ�
    /// </summary>
    /// <param name="file"></param>
    /// <param name="filePath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task SaveAsync(IFormFile file, string filePath, CancellationToken cancellationToken = default)
    {
        using var stream = File.Create(filePath);
        await file.CopyToAsync(stream, cancellationToken);
    }
}
