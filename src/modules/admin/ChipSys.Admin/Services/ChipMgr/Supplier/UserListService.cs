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
/// 用户操作清单服务
/// </summary>
[Order(76)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class UserListService : BaseService, IUserListService, IDynamicApi
{
    private readonly AdminRepositoryBase<UserListEntity> _userListRep;
    private readonly AdminRepositoryBase<ProductSupplierEntity> _productSupplierRep;
    private readonly AdminLocalizer _adminLocalizer;

    public UserListService(AdminRepositoryBase<UserListEntity> userListRep, 
        AdminRepositoryBase<ProductSupplierEntity> productSupplierRep,
        AdminLocalizer adminLocalizer)
    {
        _userListRep = userListRep;
        _productSupplierRep = productSupplierRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UserListGetOutput> GetAsync(long id)
    {
        var result = await _userListRep.GetAsync<UserListGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<UserListGetPageOutput>> GetPageAsync(PageInput<UserListGetPageInput> input)
    {
        var userId = input.Filter?.UserId;
        var productSupplierId = input.Filter?.ProductSupplierId;
        var listType = input.Filter?.ListType;
        var status = input.Filter?.Status;

        var list = await _userListRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(userId.HasValue, a => a.UserId == userId)
        .WhereIf(productSupplierId.HasValue, a => a.ProductSupplierId == productSupplierId)
        .WhereIf(listType.NotNull(), a => a.ListType == listType)
        .WhereIf(status.NotNull(), a => a.Status == status)
        .Count(out var total)
        .OrderByDescending(a => a.CreatedAt)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<UserListGetPageOutput>();

        var data = new PageOutput<UserListGetPageOutput>()
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
    public async Task<List<UserListGetListOutput>> GetListAsync(UserListGetListInput input)
    {
        var userId = input?.UserId;
        var productSupplierId = input?.ProductSupplierId;
        var listType = input?.ListType;
        var status = input?.Status;

        var select = _userListRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(userId.HasValue, a => a.UserId == userId)
        .WhereIf(productSupplierId.HasValue, a => a.ProductSupplierId == productSupplierId)
        .WhereIf(listType.NotNull(), a => a.ListType == listType)
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

        return await select.ToListAsync<UserListGetListOutput>();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(UserListAddInput input)
    {
        if (!await _productSupplierRep.Select.AnyAsync(a => a.Id == input.ProductSupplierId))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品供应商关系不存在"]);
        }

        if (await _userListRep.Select.AnyAsync(a => a.UserId == input.UserId && a.ProductSupplierId == input.ProductSupplierId && a.ListType == input.ListType))
        {
            throw ResultOutput.Exception(_adminLocalizer["该用户操作记录已存在"]);
        }

        var entity = Mapper.Map<UserListEntity>(input);
        await _userListRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(UserListUpdateInput input)
    {
        var entity = await _userListRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["用户操作清单不存在"]);
        }

        if (!await _productSupplierRep.Select.AnyAsync(a => a.Id == input.ProductSupplierId))
        {
            throw ResultOutput.Exception(_adminLocalizer["产品供应商关系不存在"]);
        }

        if (await _userListRep.Select.AnyAsync(a => a.Id != input.Id && a.UserId == input.UserId && a.ProductSupplierId == input.ProductSupplierId && a.ListType == input.ListType))
        {
            throw ResultOutput.Exception(_adminLocalizer["该用户操作记录已存在"]);
        }

        Mapper.Map(input, entity);
        await _userListRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        await _userListRep.DeleteAsync(a => a.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        await _userListRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        await _userListRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _userListRep.SoftDeleteAsync(ids);
    }
}
