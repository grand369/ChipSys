using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Contracts.Domain.ChipMgr.ProductCategory;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Product;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Repositories;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Services.ProductCategory.Dto;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;

namespace ChipSys.Admin.Services.ProductCategory;

/// <summary>
/// 产品分类服务
/// </summary>
[Order(69)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class CategoryService : BaseService, ICategoryService, IDynamicApi
{
    private readonly AdminRepositoryBase<CategoryEntity> _categoryRep;
    private readonly AdminRepositoryBase<ProductEntity> _productRep;
    private readonly AdminLocalizer _adminLocalizer;

    public CategoryService(AdminRepositoryBase<CategoryEntity> categoryRep, 
        AdminRepositoryBase<ProductEntity> productRep,
        AdminLocalizer adminLocalizer)
    {
        _categoryRep = categoryRep;
        _productRep = productRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<CategoryGetOutput> GetAsync(long id)
    {
        var result = await _categoryRep.GetAsync<CategoryGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<CategoryGetPageOutput>> GetPageAsync(PageInput<CategoryGetPageInput> input)
    {
        var key = input.Filter?.Name;
        var code = input.Filter?.Code;
        var parentId = input.Filter?.ParentId;
        var enabled = input.Filter?.Enabled;

        var list = await _categoryRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
        .WhereIf(code.NotNull(), a => a.Code.Contains(code))
        .WhereIf(parentId.HasValue, a => a.ParentId == parentId)
        .WhereIf(enabled.HasValue, a => a.Enabled == enabled)
        .Count(out var total)
        .OrderByDescending(a => a.Sort)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<CategoryGetPageOutput>();

        var data = new PageOutput<CategoryGetPageOutput>()
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
    public async Task<List<CategoryGetListOutput>> GetListAsync(CategoryGetListInput input)
    {
        var key = input?.Name;
        var code = input?.Code;
        var parentId = input?.ParentId;
        var enabled = input?.Enabled;

        var select = _categoryRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
        .WhereIf(code.NotNull(), a => a.Code.Contains(code))
        .WhereIf(parentId.HasValue, a => a.ParentId == parentId)
        .WhereIf(enabled.HasValue, a => a.Enabled == enabled)
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

        return await select.ToListAsync<CategoryGetListOutput>();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(CategoryAddInput input)
    {
        if (await _categoryRep.Select.AnyAsync(a => a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品分类名称已存在"]);
        }

        if (input.Code.NotNull() && await _categoryRep.Select.AnyAsync(a => a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品分类编码已存在"]);
        }

        if (input.ParentId.HasValue && input.ParentId > 0)
        {
            if (!await _categoryRep.Select.AnyAsync(a => a.Id == input.ParentId))
            {
                throw ResultOutput.Exception(_adminLocalizer["上级产品分类不存在"]);
            }
        }

        var entity = Mapper.Map<CategoryEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _categoryRep.Select.MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _categoryRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(CategoryUpdateInput input)
    {
        var entity = await _categoryRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品分类不存在"]);
        }

        if (input.Id == input.ParentId)
        {
            throw ResultOutput.Exception(_adminLocalizer["上级产品分类不能是当前产品分类"]);
        }

        if (await _categoryRep.Select.AnyAsync(a => a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品分类名称已存在"]);
        }

        if (input.Code.NotNull() && await _categoryRep.Select.AnyAsync(a => a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品分类编码已存在"]);
        }

        if (input.ParentId.HasValue && input.ParentId > 0)
        {
            if (!await _categoryRep.Select.AnyAsync(a => a.Id == input.ParentId))
            {
                throw ResultOutput.Exception(_adminLocalizer["上级产品分类不存在"]);
            }
        }

        if (input.ParentId.HasValue)
        {
            var childIdList = await _categoryRep.Select
            .Where(a => a.Id == input.Id)
            .AsTreeCte()
            .ToListAsync(a => a.Id);
            if (childIdList.Contains(input.ParentId.Value))
            {
                throw ResultOutput.Exception(_adminLocalizer["上级产品分类不能是下级产品分类"]);
            }
        }

        Mapper.Map(input, entity);
        await _categoryRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        // 删除产品
        await _productRep.DeleteAsync(a => a.CategoryId == id);

        // 删除产品分类
        await _categoryRep.DeleteAsync(a => a.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        // 删除产品
        await _productRep.DeleteAsync(a => ids.Contains(a.CategoryId));

        // 删除产品分类
        await _categoryRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        await _productRep.SoftDeleteAsync(a => a.CategoryId == id);
        await _categoryRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _productRep.SoftDeleteAsync(a => ids.Contains(a.CategoryId));
        await _categoryRep.SoftDeleteAsync(ids);
    }
}
