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
/// 命中清单服务
/// </summary>
[Order(75)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class HitListService : BaseService, IHitListService, IDynamicApi
{
    private readonly AdminRepositoryBase<HitListEntity> _hitListRep;
    private readonly AdminRepositoryBase<UploadListItemEntity> _uploadListItemRep;
    private readonly AdminRepositoryBase<ProductSupplierEntity> _productSupplierRep;
    private readonly AdminLocalizer _adminLocalizer;

    public HitListService(AdminRepositoryBase<HitListEntity> hitListRep, 
        AdminRepositoryBase<UploadListItemEntity> uploadListItemRep,
        AdminRepositoryBase<ProductSupplierEntity> productSupplierRep,
        AdminLocalizer adminLocalizer)
    {
        _hitListRep = hitListRep;
        _uploadListItemRep = uploadListItemRep;
        _productSupplierRep = productSupplierRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<HitListGetOutput> GetAsync(long id)
    {
        var result = await _hitListRep.GetAsync<HitListGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<HitListGetPageOutput>> GetPageAsync(PageInput<HitListGetPageInput> input)
    {
        var userId = input.Filter?.UserId;
        var itemId = input.Filter?.ItemId;
        var productSupplierId = input.Filter?.ProductSupplierId;

        var list = await _hitListRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(userId.HasValue, a => a.UserId == userId)
        .WhereIf(itemId.HasValue, a => a.ItemId == itemId)
        .WhereIf(productSupplierId.HasValue, a => a.ProductSupplierId == productSupplierId)
        .Count(out var total)
        .OrderByDescending(a => a.CreatedAt)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<HitListGetPageOutput>();

        var data = new PageOutput<HitListGetPageOutput>()
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
    public async Task<List<HitListGetListOutput>> GetListAsync(HitListGetListInput input)
    {
        var userId = input?.UserId;
        var itemId = input?.ItemId;
        var productSupplierId = input?.ProductSupplierId;

        var select = _hitListRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(userId.HasValue, a => a.UserId == userId)
        .WhereIf(itemId.HasValue, a => a.ItemId == itemId)
        .WhereIf(productSupplierId.HasValue, a => a.ProductSupplierId == productSupplierId)
        .OrderByDescending(a => a.Confidence);

        if (input.SortList != null && input.SortList.Count > 0)
        {
            input.SortList.ForEach(sort =>
            {
                select = select.OrderByPropertyNameIf(sort.Order.HasValue, sort.PropName, sort.IsAscending.Value);
            });
        }
        else
        {
            select = select.OrderByDescending(a => a.Confidence);
        }

        return await select.ToListAsync<HitListGetListOutput>();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(HitListAddInput input)
    {
        if (!await _uploadListItemRep.Select.AnyAsync(a => a.Id == input.ItemId))
        {
            throw ResultOutput.Exception(_adminLocalizer["上传清单条目不存在"]);
        }

        if (!await _productSupplierRep.Select.AnyAsync(a => a.Id == input.ProductSupplierId))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品供应商关系不存在"]);
        }

        if (await _hitListRep.Select.AnyAsync(a => a.ItemId == input.ItemId && a.ProductSupplierId == input.ProductSupplierId))
        {
            throw ResultOutput.Exception(_adminLocalizer["该命中记录已存在"]);
        }

        var entity = Mapper.Map<HitListEntity>(input);
        await _hitListRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(HitListUpdateInput input)
    {
        var entity = await _hitListRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["命中清单不存在"]);
        }

        if (!await _uploadListItemRep.Select.AnyAsync(a => a.Id == input.ItemId))
        {
            throw ResultOutput.Exception(_adminLocalizer["上传清单条目不存在"]);
        }

        if (!await _productSupplierRep.Select.AnyAsync(a => a.Id == input.ProductSupplierId))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品供应商关系不存在"]);
        }

        if (await _hitListRep.Select.AnyAsync(a => a.Id != input.Id && a.ItemId == input.ItemId && a.ProductSupplierId == input.ProductSupplierId))
        {
            throw ResultOutput.Exception(_adminLocalizer["该命中记录已存在"]);
        }

        Mapper.Map(input, entity);
        await _hitListRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        await _hitListRep.DeleteAsync(a => a.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        await _hitListRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        await _hitListRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _hitListRep.SoftDeleteAsync(ids);
    }
}
