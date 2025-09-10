using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Domain;
using ChipSys.Admin.Domain.Dto;
using ChipSys.Admin.Services.Dto;

namespace ChipSys.Admin.Services;

/// <summary>
/// 文件接口
/// </summary>
public interface IFileService
{
    Task<PageOutput<FileGetPageOutput>> GetPageAsync(PageInput<FileGetPageInput> input);

    Task DeleteAsync(FileDeleteInput input);

    Task<FileEntity> UploadFileAsync(IFormFile file, string fileDirectory = "", bool fileReName = true);

    Task<List<FileEntity>> UploadFilesAsync([Required] IFormFileCollection files, string fileDirectory = "", bool fileReName = true);
}
