using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Product;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Repositories;
using ChipSys.Admin.Resources;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Admin.Contracts.Services.ChipMgr.Product;
using ChipSys.Admin.Contracts.Services.ChipMgr.Product.Dto;

namespace ChipSys.Admin.Services.ChipMgr.Product;

/// <summary>
/// 产品供应商关系服务
/// </summary>
[Order(73)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class ProductSupplierService : BaseService, IProductSupplierService, IDynamicApi
{
    private readonly AdminRepositoryBase<ProductSupplierEntity> _productSupplierRep;
    private readonly AdminRepositoryBase<ProductEntity> _productRep;
    private readonly AdminRepositoryBase<SupplierEntity> _supplierRep;
    private readonly AdminLocalizer _adminLocalizer;

    public ProductSupplierService(AdminRepositoryBase<ProductSupplierEntity> productSupplierRep, 
        AdminRepositoryBase<ProductEntity> productRep,
        AdminRepositoryBase<SupplierEntity> supplierRep,
        AdminLocalizer adminLocalizer)
    {
        _productSupplierRep = productSupplierRep;
        _productRep = productRep;
        _supplierRep = supplierRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ProductSupplierGetOutput> GetAsync(long id)
    {
        var result = await _productSupplierRep.GetAsync<ProductSupplierGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<ProductSupplierGetPageOutput>> GetPageAsync(PageInput<ProductSupplierGetPageInput> input)
    {
        var productId = input.Filter?.ProductId;
        var supplierId = input.Filter?.SupplierId;
        var currency = input.Filter?.Currency;
        var supplierModel = input.Filter?.SupplierModel;

        var list = await _productSupplierRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(productId.HasValue, a => a.ProductId == productId)
        .WhereIf(supplierId.HasValue, a => a.SupplierId == supplierId)
        .WhereIf(currency.NotNull(), a => a.Currency == currency)
        .WhereIf(supplierModel.NotNull(), a => a.SupplierModel.Contains(supplierModel))
        .Count(out var total)
        .OrderByDescending(a => a.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<ProductSupplierGetPageOutput>();

        var data = new PageOutput<ProductSupplierGetPageOutput>()
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
    public async Task<List<ProductSupplierGetListOutput>> GetListAsync(ProductSupplierGetListInput input)
    {
        var productId = input?.ProductId;
        var supplierId = input?.SupplierId;
        var currency = input?.Currency;
        var supplierModel = input?.SupplierModel;

        var select = _productSupplierRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(productId.HasValue, a => a.ProductId == productId)
        .WhereIf(supplierId.HasValue, a => a.SupplierId == supplierId)
        .WhereIf(currency.NotNull(), a => a.Currency == currency)
        .WhereIf(supplierModel.NotNull(), a => a.SupplierModel.Contains(supplierModel))
        .OrderBy(a => a.CurrentPrice);

        if (input.SortList != null && input.SortList.Count > 0)
        {
            input.SortList.ForEach(sort =>
            {
                select = select.OrderByPropertyNameIf(sort.Order.HasValue, sort.PropName, sort.IsAscending.Value);
            });
        }
        else
        {
            select = select.OrderBy(a => a.CurrentPrice);
        }

        return await select.ToListAsync<ProductSupplierGetListOutput>();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(ProductSupplierAddInput input)
    {
        if (!await _productRep.Select.AnyAsync(a => a.Id == input.ProductId))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品不存在"]);
        }

        if (!await _supplierRep.Select.AnyAsync(a => a.Id == input.SupplierId))
        {
            throw ResultOutput.Exception(_adminLocalizer["供应商不存在"]);
        }

        if (await _productSupplierRep.Select.AnyAsync(a => a.ProductId == input.ProductId && a.SupplierId == input.SupplierId))
        {
            throw ResultOutput.Exception(_adminLocalizer["该产品与供应商的关系已存在"]);
        }

        var entity = Mapper.Map<ProductSupplierEntity>(input);
        await _productSupplierRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(ProductSupplierUpdateInput input)
    {
        var entity = await _productSupplierRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品供应商关系不存在"]);
        }

        if (!await _productRep.Select.AnyAsync(a => a.Id == input.ProductId))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品不存在"]);
        }

        if (!await _supplierRep.Select.AnyAsync(a => a.Id == input.SupplierId))
        {
            throw ResultOutput.Exception(_adminLocalizer["供应商不存在"]);
        }

        if (await _productSupplierRep.Select.AnyAsync(a => a.Id != input.Id && a.ProductId == input.ProductId && a.SupplierId == input.SupplierId))
        {
            throw ResultOutput.Exception(_adminLocalizer["该产品与供应商的关系已存在"]);
        }

        Mapper.Map(input, entity);
        await _productSupplierRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        await _productSupplierRep.DeleteAsync(a => a.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        await _productSupplierRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        await _productSupplierRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _productSupplierRep.SoftDeleteAsync(ids);
    }
}
