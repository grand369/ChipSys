using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Product;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Repositories;
using ChipSys.Admin.Resources;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Admin.Contracts.Services.ChipMgr.Supplier;
using ChipSys.Admin.Contracts.Services.ChipMgr.Supplier.Dto;

namespace ChipSys.Admin.Services.ChipMgr.Supplier;

/// <summary>
/// 上传清单条目服务
/// </summary>
[Order(77)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class UploadListItemService : BaseService, IUploadListItemService, IDynamicApi
{
    private readonly AdminRepositoryBase<UploadListItemEntity> _uploadListItemRep;
    private readonly AdminRepositoryBase<UploadListEntity> _uploadListRep;
    private readonly AdminRepositoryBase<ProductEntity> _productRep;
    private readonly AdminLocalizer _adminLocalizer;

    public UploadListItemService(AdminRepositoryBase<UploadListItemEntity> uploadListItemRep, 
        AdminRepositoryBase<UploadListEntity> uploadListRep,
        AdminRepositoryBase<ProductEntity> productRep,
        AdminLocalizer adminLocalizer)
    {
        _uploadListItemRep = uploadListItemRep;
        _uploadListRep = uploadListRep;
        _productRep = productRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UploadListItemGetOutput> GetAsync(long id)
    {
        var result = await _uploadListItemRep.GetAsync<UploadListItemGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<UploadListItemGetPageOutput>> GetPageAsync(PageInput<UploadListItemGetPageInput> input)
    {
        var listId = input.Filter?.ListId;
        var rawCode = input.Filter?.RawCode;
        var rawBrand = input.Filter?.RawBrand;
        var matchStatus = input.Filter?.MatchStatus;
        var matchedProductId = input.Filter?.MatchedProductId;

        var list = await _uploadListItemRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(listId.HasValue, a => a.ListId == listId)
        .WhereIf(rawCode.NotNull(), a => a.RawCode.Contains(rawCode))
        .WhereIf(rawBrand.NotNull(), a => a.RawBrand.Contains(rawBrand))
        .WhereIf(matchStatus.NotNull(), a => a.MatchStatus == matchStatus)
        .WhereIf(matchedProductId.HasValue, a => a.MatchedProductId == matchedProductId)
        .Count(out var total)
        .OrderByDescending(a => a.CreatedAt)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<UploadListItemGetPageOutput>();

        var data = new PageOutput<UploadListItemGetPageOutput>()
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
    public async Task<List<UploadListItemGetListOutput>> GetListAsync(UploadListItemGetListInput input)
    {
        var listId = input?.ListId;
        var rawCode = input?.RawCode;
        var rawBrand = input?.RawBrand;
        var matchStatus = input?.MatchStatus;
        var matchedProductId = input?.MatchedProductId;

        var select = _uploadListItemRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(listId.HasValue, a => a.ListId == listId)
        .WhereIf(rawCode.NotNull(), a => a.RawCode.Contains(rawCode))
        .WhereIf(rawBrand.NotNull(), a => a.RawBrand.Contains(rawBrand))
        .WhereIf(matchStatus.NotNull(), a => a.MatchStatus == matchStatus)
        .WhereIf(matchedProductId.HasValue, a => a.MatchedProductId == matchedProductId)
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

        return await select.ToListAsync<UploadListItemGetListOutput>();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(UploadListItemAddInput input)
    {
        if (!await _uploadListRep.Select.AnyAsync(a => a.Id == input.ListId))
        {
            throw ResultOutput.Exception(_adminLocalizer["上传清单不存在"]);
        }

        if (input.MatchedProductId.HasValue && !await _productRep.Select.AnyAsync(a => a.Id == input.MatchedProductId))
        {
            throw ResultOutput.Exception(_adminLocalizer["匹配的产品不存在"]);
        }

        var entity = Mapper.Map<UploadListItemEntity>(input);
        await _uploadListItemRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(UploadListItemUpdateInput input)
    {
        var entity = await _uploadListItemRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["上传清单条目不存在"]);
        }

        if (!await _uploadListRep.Select.AnyAsync(a => a.Id == input.ListId))
        {
            throw ResultOutput.Exception(_adminLocalizer["上传清单不存在"]);
        }

        if (input.MatchedProductId.HasValue && !await _productRep.Select.AnyAsync(a => a.Id == input.MatchedProductId))
        {
            throw ResultOutput.Exception(_adminLocalizer["匹配的产品不存在"]);
        }

        Mapper.Map(input, entity);
        await _uploadListItemRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        await _uploadListItemRep.DeleteAsync(a => a.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        await _uploadListItemRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        await _uploadListItemRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _uploadListItemRep.SoftDeleteAsync(ids);
    }
}
