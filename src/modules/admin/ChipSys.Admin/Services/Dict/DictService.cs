using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Magicodes.ExporterAndImporter.Core.Models;
using Mapster;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Db;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Helpers;
using ChipSys.Admin.Domain.Dict;
using ChipSys.Admin.Domain.DictType;
using ChipSys.Admin.Domain.Dict.Dto;
using ChipSys.Admin.Repositories;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Services.Dict.Dto;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using Yitter.IdGenerator;

namespace ChipSys.Admin.Services.Dict;

/// <summary>
/// �����ֵ����
/// </summary>
[Order(60)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class DictService : BaseService, IDictService, IDynamicApi
{
    private readonly AdminRepositoryBase<DictEntity> _dictRep;
    private readonly AdminRepositoryBase<DictTypeEntity> _dictTypeRep;
    private readonly AdminLocalizer _adminLocalizer;
    private readonly IEHelper _iEHelper;

    public DictService(AdminRepositoryBase<DictEntity> dictRep,
        AdminRepositoryBase<DictTypeEntity> dictTypeRep,
        AdminLocalizer adminLocalizer,
        IEHelper iEHelper)
    {
        _dictRep = dictRep;
        _dictTypeRep = dictTypeRep;
        _adminLocalizer = adminLocalizer;
        _iEHelper = iEHelper;
    }

    /// <summary>
    /// ��ѯ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DictGetOutput> GetAsync(long id)
    {
        var result = await _dictRep.GetAsync<DictGetOutput>(id);
        return result;
    }

    /// <summary>
    /// ��ѯ��ҳ
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Obsolete($"��ʹ��{nameof(GetAllAsync)}�������")]
    [HttpPost]
    public async Task<PageOutput<DictGetPageOutput>> GetPageAsync(PageInput<DictGetPageInput> input)
    {
        var key = input.Filter?.Name;
        var dictTypeId = input.Filter?.DictTypeId;

        var select = _dictRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(dictTypeId.HasValue && dictTypeId.Value > 0, a => a.DictTypeId == dictTypeId)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
        .Count(out var total)
        .OrderBy(a => a.Parent);

        if (input.SortList != null && input.SortList.Count > 0)
        {
            input.SortList.ForEach(sort =>
            {
                select = select.OrderByPropertyNameIf(sort.Order.HasValue, sort.PropName, sort.IsAscending.Value);
            });
        }
        else
        {
            select = select.OrderBy(a => a.Sort);
        }

        var list = await select
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<DictGetPageOutput>();

        var data = new PageOutput<DictGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// ��ѯ�б�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<DictGetAllOutput>> GetAllAsync(DictGetAllInput input)
    {
        var key = input?.Name;

        var select = _dictRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .Where(a => a.DictTypeId == input.DictTypeId)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
        .OrderBy(a => a.ParentId);

        if (input.SortList != null && input.SortList.Count > 0)
        {
            input.SortList.ForEach(sort =>
            {
                select = select.OrderByPropertyNameIf(sort.Order.HasValue, sort.PropName, sort.IsAscending.Value);
            });
        }
        else
        {
            select = select.OrderBy(a => a.Sort);
        }

        return await select.ToListAsync<DictGetAllOutput>();
    }

    /// <summary>
    /// ͨ�����ͱ����ѯ�б�
    /// </summary>
    /// <param name="codes">�ֵ����ͱ����б�</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<Dictionary<string, List<DictGetListOutput>>> GetListAsync(string[] codes)
    {
        var list = await _dictRep.Select
        .Where(a => codes.Contains(a.DictType.Code) && a.DictType.Enabled == true && a.Enabled == true)
        .OrderBy(a => a.Sort)
        .ToListAsync(a => new DictGetListOutput { DictTypeCode = a.DictType.Code });

        var dicts = new Dictionary<string, List<DictGetListOutput>>();
        foreach (var code in codes)
        {
            if (code.NotNull())
                dicts[code] = list.Where(a => a.DictTypeCode == code).ToList();
        }

        return dicts;
    }

    /// <summary>
    /// ͨ���������Ʋ�ѯ�б�
    /// </summary>
    /// <param name="names">�ֵ����������б�</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<Dictionary<string, List<DictGetListOutput>>> GetListByNamesAsync(string[] names)
    {
        var list = await _dictRep.Select
        .Where(a => names.Contains(a.DictType.Name) && a.DictType.Enabled == true && a.Enabled == true)
        .OrderBy(a => a.Sort)
        .ToListAsync(a => new DictGetListOutput { DictTypeName = a.DictType.Name });

        var dicts = new Dictionary<string, List<DictGetListOutput>>();
        foreach (var name in names)
        {
            if (name.NotNull())
                dicts[name] = list.Where(a => a.DictTypeName == name).ToList();
        }

        return dicts;
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(DictAddInput input)
    {
        // ��ȡ�ֵ�������Ϣ
        var dictType = await _dictTypeRep.GetAsync(input.DictTypeId);
        if (dictType == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["�ֵ����Ͳ�����"]);
        }

        // �����νṹǿ������ParentId=0
        if (!dictType.IsTree)
        {
            input.ParentId = 0;
        }
        // ���νṹ��Ҫ��֤ParentId��Ч��
        else if (input.ParentId > 0)
        {
            var parentExists = await _dictRep.Select.Where(a => a.Id == input.ParentId).AnyAsync();

            if (!parentExists)
            {
                throw ResultOutput.Exception(_adminLocalizer["�����ֵ䲻����"]);
            }
        }

        if (await _dictRep.Select.AnyAsync(a => a.DictTypeId == input.DictTypeId && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["�ֵ������Ѵ���"]);
        }

        if (input.Code.NotNull() && await _dictRep.Select.AnyAsync(a => a.DictTypeId == input.DictTypeId && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["�ֵ�����Ѵ���"]);
        }

        if (input.Value.NotNull() && await _dictRep.Select.AnyAsync(a => a.DictTypeId == input.DictTypeId && a.Value == input.Value))
        {
            throw ResultOutput.Exception(_adminLocalizer["�ֵ�ֵ�Ѵ���"]);
        }

        var entity = Mapper.Map<DictEntity>(input);
        if (entity.Sort == 0)
        {
            var maxSortQuery = _dictRep.Select.Where(a => a.DictTypeId == input.DictTypeId);
            // ���νṹʱ��ͬһ���ڵ�������
            if (dictType.IsTree)
            {
                maxSortQuery = maxSortQuery.Where(a => a.ParentId == input.ParentId);
            }

            var sort = await maxSortQuery.MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _dictRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// �޸�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(DictUpdateInput input)
    {
        // ��ȡ�ֵ�������Ϣ
        var dictType = await _dictTypeRep.GetAsync(input.DictTypeId);
        if (dictType == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["�ֵ����Ͳ�����"]);
        }

        // �����νṹǿ������ParentId=0
        if (!dictType.IsTree)
        {
            input.ParentId = 0;
        }
        // ���νṹ��Ҫ��֤ParentId��Ч��
        else if (input.ParentId > 0)
        {
            var parentExists = await _dictRep.Select.Where(a => a.Id == input.ParentId).AnyAsync();

            if (!parentExists)
            {
                throw ResultOutput.Exception(_adminLocalizer["�����ֵ䲻����"]);
            }
        }

        var entity = await _dictRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["�ֵ䲻����"]);
        }

        if (await _dictRep.Select.AnyAsync(a => a.Id != input.Id && a.DictTypeId == input.DictTypeId && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["�ֵ������Ѵ���"]);
        }

        if (input.Code.NotNull() && await _dictRep.Select.AnyAsync(a => a.Id != input.Id && a.DictTypeId == input.DictTypeId && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["�ֵ�����Ѵ���"]);
        }

        if (input.Value.NotNull() && await _dictRep.Select.AnyAsync(a => a.Id != input.Id && a.DictTypeId == input.DictTypeId && a.Value == input.Value))
        {
            throw ResultOutput.Exception(_adminLocalizer["�ֵ�ֵ�Ѵ���"]);
        }

        Mapper.Map(input, entity);
        await _dictRep.UpdateAsync(entity);
    }

    /// <summary>
    /// ����ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        await _dictRep.DeleteAsync(m => m.Id == id);
    }

    /// <summary>
    /// ��������ɾ��
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchDeleteAsync(long[] ids)
    {
        await _dictRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task SoftDeleteAsync(long id)
    {
        await _dictRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// ����ɾ��
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _dictRep.SoftDeleteAsync(ids);
    }

    /// <summary>
    /// ���ص���ģ��
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [NonFormatResult]
    public async Task<ActionResult> DownloadTemplateAsync()
    {
        var fileName = _adminLocalizer["�����ֵ�ģ��{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")];
        return await _iEHelper.DownloadTemplateAsync(new DictImport(), fileName);
    }

    /// <summary>
    /// ���ش������ļ�
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    [HttpPost]
    [NonFormatResult]
    public async Task<ActionResult> DownloadErrorMarkAsync(string fileId, string fileName)
    {
        if (fileName.IsNull())
        {
            fileName = _adminLocalizer["�����ֵ������{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")];
        }
        return await _iEHelper.DownloadErrorMarkAsync(fileId, fileName);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [NonFormatResult]
    public async Task<ActionResult> ExportDataAsync(ExportInput input)
    {
        using var _ = _dictRep.DataFilter.DisableAll();

        var select = _dictRep.Select;
        if (input.SortList != null && input.SortList.Count > 0)
        {
            select = select.SortList(input.SortList);
        }
        else
        {
            select = select.OrderBy(a => a.DictType.Sort).OrderBy(a => a.Sort);
        }

        //��ѯ����
        var dataList = await select.WhereDynamicFilter(input.DynamicFilter)
            .ToListAsync(a => new DictExport 
            { 
                DictTypeName = a.DictType.Name,
                ParentName = a.Parent.Name,
            });

        var dictTypeName = dataList.Count > 0 ? dataList[0].DictTypeName : string.Empty;

        //��������
        var fileName = input.FileName.NotNull() ? input.FileName : _adminLocalizer["�����ֵ�-{0}�б�{1}.xlsx", dictTypeName, DateTime.Now.ToString("yyyyMMddHHmmss")];

        return await _iEHelper.ExportDataAsync(dataList, fileName, dictTypeName);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="file"></param>
    /// <param name="duplicateAction"></param>
    /// <param name="fileId"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ImportOutput> ImportDataAsync([Required] IFormFile file, int duplicateAction, string fileId)
    {
        var newIdSet = new HashSet<long>();

        var importResult = await _iEHelper.ImportDataAsync<DictImport>(file, fileId, async (importResult) =>
        {
            // �������
            var importDataList = importResult.Data;
            var importDictTypeNameList = importDataList.Select(a => a.DictTypeName).Distinct().ToList();
            var dictTypeList = await _dictTypeRep.Where(a => importDictTypeNameList.Contains(a.Name)).ToListAsync(a => new { a.Id, a.Name });
            var dictTypeIdList = dictTypeList.Select(a => a.Id).Distinct().ToList();
            var dictList = await _dictRep.Where(a => dictTypeIdList.Contains(a.DictTypeId)).ToListAsync(a => new { a.Id, a.DictTypeId, a.Name, a.Code, a.Value });

            if (importResult.RowErrors == null)
            {
                importResult.RowErrors = new List<DataRowErrorInfo>();
            }
            var errorList = importResult.RowErrors;

            // ================ ��������1��Ԥ����ID����������ӳ�� ================
            var nameIdMap = new Dictionary<(long, string), long>(); // (DictTypeId, Name) -> Id
            foreach (var importData in importDataList)
            {
                // �����ֵ�����ID
                importData.DictTypeId = dictTypeList.Where(a => a.Name == importData.DictTypeName).Select(a => a.Id).FirstOrDefault();

                // �������ݿ��е��ֵ���
                var dbItem = dictList.FirstOrDefault(a => a.DictTypeId == importData.DictTypeId && a.Name == importData.Name);

                if (dbItem != null)
                {
                    // �Ѵ��ڣ�ʹ�����ݿ�ID
                    importData.Id = dbItem.Id;
                }
                else
                {
                    // �����ݣ�������ID
                    var newId = YitIdHelper.NextId();
                    importData.Id = newId;
                    newIdSet.Add(newId);
                }

                // ��¼����ӳ�䣨�����ֵ�������Чʱ��
                if (importData.DictTypeId > 0)
                {
                    var key = (importData.DictTypeId, importData.Name);
                    nameIdMap[key] = importData.Id;
                }
            }

            // ================ ��������2������ParentId ================
            foreach (var importData in importDataList)
            {
                var rowIndex = importDataList.ToList().FindIndex(o => o.Equals(importData)) + 2;

                if (importData.DictTypeId <= 0)
                {
                    // �ֵ����Ͳ�����
                    var errorInfo = new DataRowErrorInfo()
                    {
                        RowIndex = rowIndex,
                    };
                    errorInfo.FieldErrors.Add(_adminLocalizer["�ֵ����"], _adminLocalizer["{0}������", importData.DictTypeName]);
                    errorList.Add(errorInfo);
                    continue;
                }

                // ����ParentId�����ȴӱ��ε��������в��ң�
                if (!string.IsNullOrEmpty(importData.ParentName))
                {
                    var parentKey = (importData.DictTypeId, importData.ParentName);

                    if (nameIdMap.TryGetValue(parentKey, out var parentId))
                    {
                        importData.ParentId = parentId;
                    }
                    else
                    {
                        // �����ݿ��в���
                        var dbParent = dictList.FirstOrDefault(a =>
                            a.DictTypeId == importData.DictTypeId &&
                            a.Name == importData.ParentName);

                        importData.ParentId = dbParent?.Id ?? 0;

                        // ���ݿ���Ҳ�Ҳ���
                        if (importData.ParentId == 0)
                        {
                            var errorInfo = new DataRowErrorInfo()
                            {
                                RowIndex = rowIndex,
                            };
                            errorInfo.FieldErrors.Add(_adminLocalizer["�ϼ��ֵ�����"], _adminLocalizer["{0}������", importData.ParentName]);
                            errorList.Add(errorInfo);
                        }
                    }
                }
                else
                {
                    importData.ParentId = 0; // ���ϼ�
                }

                // ================ �Ż�����ظ���� ================
                // ͬʱ������ݿ�ͱ��ε�������
                if (duplicateAction == 1) // ����ѡ�񸲸�ʱ�ż��
                {
                    // �������ظ�
                    if (!string.IsNullOrEmpty(importData.Code))
                    {
                        // ��鱾�ε�������
                        var importDup = importDataList.FirstOrDefault(a =>
                            a != importData &&
                            a.DictTypeId == importData.DictTypeId &&
                            a.Code == importData.Code);

                        // ������ݿ�
                        var dbDup = dictList.FirstOrDefault(a =>
                            a.DictTypeId == importData.DictTypeId &&
                            a.Code == importData.Code &&
                            a.Id != importData.Id); // �ų�����

                        if (importDup != null || dbDup != null)
                        {
                            var errorInfo = new DataRowErrorInfo()
                            {
                                RowIndex = rowIndex,
                            };
                            errorInfo.FieldErrors.Add(_adminLocalizer["�ֵ����"], _adminLocalizer["{0}�Ѵ���", importData.Code]);
                            errorList.Add(errorInfo);
                        }
                    }

                    // ���ֵ�ظ����߼�ͬ�ϣ�
                    if (!string.IsNullOrEmpty(importData.Value))
                    {
                        var importDup = importDataList.FirstOrDefault(a =>
                            a != importData &&
                            a.DictTypeId == importData.DictTypeId &&
                            a.Value == importData.Value);

                        var dbDup = dictList.FirstOrDefault(a =>
                            a.DictTypeId == importData.DictTypeId &&
                            a.Value == importData.Value &&
                            a.Id != importData.Id);

                        if (importDup != null || dbDup != null)
                        {
                            var errorInfo = new DataRowErrorInfo()
                            {
                                RowIndex = rowIndex,
                            };
                            errorInfo.FieldErrors.Add(_adminLocalizer["�ֵ�ֵ"], _adminLocalizer["{0}�Ѵ���", importData.Value]);
                            errorList.Add(errorInfo);
                        }
                    }
                }
            }

            return importResult;
        });

        var importDataList = importResult.Data;
        var output = new ImportOutput()
        {
            Total = importDataList.Count
        };

        if (output.Total > 0)
        {
            // ��������
            var insertList = importDataList
                .Where(a => a.Id > 0 && newIdSet.Contains(a.Id)) // ʹ�����ɵ�ID
                .Select(a => a.Adapt<DictEntity>())
                .ToList();

            output.InsertCount = insertList.Count;
            await _dictRep.InsertAsync(insertList);

            // ��������
            var updateList = importDataList
                .Where(a => a.Id > 0 && !newIdSet.Contains(a.Id)) // �ų������ɵ�ID
                .ToList();

            if (duplicateAction == 1 && updateList.Count > 0)
            {
                var updateIds = updateList.Select(a => a.Id).ToList();
                var dbDataList = await _dictRep.Where(a => updateIds.Contains(a.Id)).ToListAsync();

                foreach (var dbItem in dbDataList)
                {
                    var importItem = updateList.First(a => a.Id == dbItem.Id);
                    importItem.Adapt(dbItem); // ʹ��Mapster����ʵ��
                }

                output.UpdateCount = dbDataList.Count;
                await _dictRep.UpdateAsync(dbDataList);
            }
        }

        return output;
    }
}
