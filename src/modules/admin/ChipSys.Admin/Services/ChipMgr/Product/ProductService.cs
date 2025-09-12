using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Product;
using ChipSys.Admin.Contracts.Domain.ChipMgr.ProductCategory;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Repositories;
using ChipSys.Admin.Resources;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Admin.Contracts.Services.ChipMgr.Product;
using ChipSys.Admin.Contracts.Services.ChipMgr.Product.Dto;

namespace ChipSys.Admin.Services.ChipMgr.Product;

/// <summary>
/// 产品服务
/// </summary>
[Order(70)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class ProductService : BaseService, IProductService, IDynamicApi
{
    private readonly AdminRepositoryBase<ProductEntity> _productRep;
    private readonly AdminRepositoryBase<CategoryEntity> _categoryRep;
    private readonly AdminLocalizer _adminLocalizer;

    public ProductService(AdminRepositoryBase<ProductEntity> productRep, 
        AdminRepositoryBase<CategoryEntity> categoryRep,
        AdminLocalizer adminLocalizer)
    {
        _productRep = productRep;
        _categoryRep = categoryRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ProductGetOutput> GetAsync(long id)
    {
        var result = await _productRep.GetAsync<ProductGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<ProductGetPageOutput>> GetPageAsync(PageInput<ProductGetPageInput> input)
    {
        var key = input.Filter?.Code;
        var brand = input.Filter?.Brand;
        var manufacturer = input.Filter?.Manufacturer;
        var model = input.Filter?.Model;
        var categoryId = input.Filter?.CategoryId;
        var enabled = input.Filter?.Enabled;

        var list = await _productRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Code.Contains(key) || a.Description.Contains(key))
        .WhereIf(brand.NotNull(), a => a.Brand.Contains(brand))
        .WhereIf(manufacturer.NotNull(), a => a.Manufacturer.Contains(manufacturer))
        .WhereIf(model.NotNull(), a => a.Model.Contains(model))
        .WhereIf(categoryId.HasValue, a => a.CategoryId == categoryId)
        .WhereIf(enabled.HasValue, a => a.Enabled == enabled)
        .Count(out var total)
        .OrderByDescending(a => a.Sort)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<ProductGetPageOutput>();

        var data = new PageOutput<ProductGetPageOutput>()
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
    public async Task<List<ProductGetListOutput>> GetListAsync(ProductGetListInput input)
    {
        var key = input?.Code;
        var brand = input?.Brand;
        var manufacturer = input?.Manufacturer;
        var model = input?.Model;
        var categoryId = input?.CategoryId;
        var enabled = input?.Enabled;

        var select = _productRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Code.Contains(key) || a.Description.Contains(key))
        .WhereIf(brand.NotNull(), a => a.Brand.Contains(brand))
        .WhereIf(manufacturer.NotNull(), a => a.Manufacturer.Contains(manufacturer))
        .WhereIf(model.NotNull(), a => a.Model.Contains(model))
        .WhereIf(categoryId.HasValue, a => a.CategoryId == categoryId)
        .WhereIf(enabled.HasValue, a => a.Enabled == enabled)
        ;

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

        return await select.ToListAsync<ProductGetListOutput>();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(ProductAddInput input)
    {
        if (await _productRep.Select.AnyAsync(a => a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品编码已存在"]);
        }

        if (input.CategoryId > 0 && !await _categoryRep.Select.AnyAsync(a => a.Id == input.CategoryId))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品分类不存在"]);
        }

        var entity = Mapper.Map<ProductEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _productRep.Select.MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _productRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(ProductUpdateInput input)
    {
        var entity = await _productRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品不存在"]);
        }

        if (await _productRep.Select.AnyAsync(a => a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品编码已存在"]);
        }

        if (input.CategoryId > 0 && !await _categoryRep.Select.AnyAsync(a => a.Id == input.CategoryId))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品分类不存在"]);
        }

        Mapper.Map(input, entity);
        await _productRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        await _productRep.DeleteAsync(a => a.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        await _productRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        await _productRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _productRep.SoftDeleteAsync(ids);
    }
}
