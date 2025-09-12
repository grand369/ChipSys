using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Repositories;
using ChipSys.Admin.Resources;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Admin.Contracts.Services.ChipMgr.Supplier;
using ChipSys.Admin.Contracts.Services.ChipMgr.Supplier.Dto;

namespace ChipSys.Admin.Services.ChipMgr.Supplier;

/// <summary>
/// 上传清单服务
/// </summary>
[Order(74)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class UploadListService : BaseService, IUploadListService, IDynamicApi
{
    private readonly AdminRepositoryBase<UploadListEntity> _uploadListRep;
    private readonly AdminRepositoryBase<UploadListItemEntity> _uploadListItemRep;
    private readonly AdminLocalizer _adminLocalizer;

    public UploadListService(AdminRepositoryBase<UploadListEntity> uploadListRep, 
        AdminRepositoryBase<UploadListItemEntity> uploadListItemRep,
        AdminLocalizer adminLocalizer)
    {
        _uploadListRep = uploadListRep;
        _uploadListItemRep = uploadListItemRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UploadListGetOutput> GetAsync(long id)
    {
        var result = await _uploadListRep.GetAsync<UploadListGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<UploadListGetPageOutput>> GetPageAsync(PageInput<UploadListGetPageInput> input)
    {
        var userId = input.Filter?.UserId;
        var fileName = input.Filter?.FileName;
        var status = input.Filter?.Status;

        var list = await _uploadListRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(userId.HasValue, a => a.UserId == userId)
        .WhereIf(fileName.NotNull(), a => a.FileName.Contains(fileName))
        .WhereIf(status.NotNull(), a => a.Status == status)
        .Count(out var total)
        .OrderByDescending(a => a.CreatedAt)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<UploadListGetPageOutput>();

        var data = new PageOutput<UploadListGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<UploadListGetListOutput>> GetListAsync(UploadListGetListInput input)
    {
        var userId = input?.UserId;
        var fileName = input?.FileName;
        var status = input?.Status;

        var select = _uploadListRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(userId.HasValue, a => a.UserId == userId)
        .WhereIf(fileName.NotNull(), a => a.FileName.Contains(fileName))
        .WhereIf(status.NotNull(), a => a.Status == status)
        .OrderByDescending(a => a.CreatedAt);

        if (input.SortList != null && input.SortList.Count > 0)
        {
            input.SortList.ForEach(sort =>
            {
                select = select.OrderByPropertyNameIf(sort.Order.HasValue, sort.PropName, sort.IsAscending.Value);
            });
        }
        else
        {
            select = select.OrderByDescending(a => a.CreatedAt);
        }

        return await select.ToListAsync<UploadListGetListOutput>();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(UploadListAddInput input)
    {
        var entity = Mapper.Map<UploadListEntity>(input);
        await _uploadListRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(UploadListUpdateInput input)
    {
        var entity = await _uploadListRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["上传清单不存在"]);
        }

        Mapper.Map(input, entity);
        await _uploadListRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        // 删除清单条目
        await _uploadListItemRep.DeleteAsync(a => a.ListId == id);
        // 删除清单
        await _uploadListRep.DeleteAsync(a => a.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        // 删除清单条目
        await _uploadListItemRep.DeleteAsync(a => ids.Contains(a.ListId));
        // 删除清单
        await _uploadListRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        await _uploadListItemRep.SoftDeleteAsync(a => a.ListId == id);
        await _uploadListRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _uploadListItemRep.SoftDeleteAsync(a => ids.Contains(a.ListId));
        await _uploadListRep.SoftDeleteAsync(ids);
    }
}
