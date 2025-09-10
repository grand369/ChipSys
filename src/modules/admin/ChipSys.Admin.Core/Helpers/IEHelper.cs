using Magicodes.ExporterAndImporter.Core.Models;
using Magicodes.ExporterAndImporter.Excel;
using Magicodes.ExporterAndImporter.Excel.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Auth;
using ChipSys.Admin.Tools.Cache;
using ChipSys.Admin.Core.Resources;

namespace ChipSys.Admin.Core.Helpers;

/// <summary>
/// ���뵼��������
/// </summary>
[InjectSingleton]
public class IEHelper
{
    private readonly AdminCoreLocalizer _adminCoreLocalizer;
    private readonly IUser _user;
    private readonly ICacheTool _cache;

    public IEHelper(AdminCoreLocalizer adminCoreLocalizer,
        IUser user,
        ICacheTool cache) 
    {
        _adminCoreLocalizer = adminCoreLocalizer;
        _user = user;
        _cache = cache;
    }

    /// <summary>
    /// ����ģ��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public async Task<ActionResult> DownloadTemplateAsync<T>(T type, string fileName) where T : class, new()
    {
        var result = await new ExcelImporter().GenerateTemplateBytes<T>();
        return new XlsxFileResult(result, fileName);
    }

    /// <summary>
    /// ���ش������ļ�
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public async Task<ActionResult> DownloadErrorMarkAsync(string fileId, string fileName)
    {
        var excelErrorMarkKey = CacheKeys.GetExcelErrorMarkKey(_user.Id, fileId);
        var fileStream = await _cache.GetAsync<byte[]>(excelErrorMarkKey);
        await _cache.DelAsync(excelErrorMarkKey);
        if (fileStream == null)
        {
            throw ResultOutput.Exception(_adminCoreLocalizer["�����µ������ݣ������ش������ļ�"], statusCode: 500);
        }

        if (fileName.IsNull())
        {
            fileName = _adminCoreLocalizer["�������ļ�{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")];
        }

        return new XlsxFileResult(fileStream, fileName);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="dataItems"></param>
    /// <param name="fileName"></param>
    /// <param name="sheetName"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> ExportDataAsync<T>(ICollection<T> dataItems, string fileName = null, string sheetName = null) where T : class, new()
    {
        var result = await new ExcelExporter().Append(dataItems, sheetName).ExportAppendDataAsByteArray();

        if (fileName.IsNull())
        {
            fileName = _adminCoreLocalizer["�����б�{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")];
        }

        return new XlsxFileResult(result, fileName);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="file"></param>
    /// <param name="fileId"></param>
    /// <param name="importResultCallback"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ImportResult<T>> ImportDataAsync<T>(IFormFile file, string fileId, Func<ImportResult<T>, Task<ImportResult<T>>> importResultCallback = null) where T : class, new()
    {
        var importResult = await new ExcelImporter().Import<T>(file.OpenReadStream());

        if (importResultCallback != null)
        {
            importResult = await importResultCallback(importResult);
        }

        var errorMsg = new StringBuilder();
        if (importResult != null && importResult.HasError)
        {
            if (importResult.Exception != null)
            {
                errorMsg.AppendLine(_adminCoreLocalizer["������Ϣ��"]);
                errorMsg.AppendLine(importResult.Exception.Message);
            }

            if (importResult.TemplateErrors != null && importResult.TemplateErrors.Count > 0)
            {
                errorMsg.AppendLine(_adminCoreLocalizer["ȱ�������У�"] + string.Join("��", importResult.TemplateErrors.Select(m => m.RequireColumnName).ToList()));
            }
        }

        var rowErros = importResult.RowErrors;
        if (rowErros?.Count > 0)
        {
            errorMsg.AppendLine(_adminCoreLocalizer["������д����"]);
            rowErros = rowErros.OrderBy(a => a.RowIndex).ToList();
            foreach (DataRowErrorInfo drErrorInfo in rowErros)
            {
                foreach (var item in drErrorInfo.FieldErrors)
                {
                    errorMsg.AppendLine(_adminCoreLocalizer["��{0}�� - {1}��{2}", drErrorInfo.RowIndex, item.Key, item.Value]);
                }
            }

            //����������ļ�
            new ExcelImporter().OutputBussinessErrorData<T>(file.OpenReadStream(), rowErros.ToList(), out byte[] fileByte);
            var userId = _user.Id;
            await _cache.DelAsync(CacheKeys.GetExcelErrorMarkKey(userId, fileId));
            await _cache.SetAsync(CacheKeys.GetExcelErrorMarkKey(userId, fileId), fileByte, TimeSpan.FromMinutes(20));
        }

        if (errorMsg.Length > 0)
        {
            throw ResultOutput.Exception(errorMsg.ToString());
        }

        return importResult;
    }
}
