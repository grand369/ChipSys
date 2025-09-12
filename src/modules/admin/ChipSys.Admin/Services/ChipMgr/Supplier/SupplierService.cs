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
/// 供应商服务
/// </summary>
[Order(71)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class SupplierService : BaseService, ISupplierService, IDynamicApi
{
    private readonly AdminRepositoryBase<SupplierEntity> _supplierRep;
    private readonly AdminLocalizer _adminLocalizer;

    public SupplierService(AdminRepositoryBase<SupplierEntity> supplierRep, 
        AdminLocalizer adminLocalizer)
    {
        _supplierRep = supplierRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SupplierGetOutput> GetAsync(long id)
    {
        var result = await _supplierRep.GetAsync<SupplierGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<SupplierGetPageOutput>> GetPageAsync(PageInput<SupplierGetPageInput> input)
    {
        var key = input.Filter?.Name;
        var contactPerson = input.Filter?.ContactPerson;
        var phone = input.Filter?.Phone;
        var email = input.Filter?.Email;
        var rating = input.Filter?.Rating;

        var list = await _supplierRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key))
        .WhereIf(contactPerson.NotNull(), a => a.ContactPerson.Contains(contactPerson))
        .WhereIf(phone.NotNull(), a => a.Phone.Contains(phone))
        .WhereIf(email.NotNull(), a => a.Email.Contains(email))
        .WhereIf(rating.HasValue, a => a.Rating == rating)
        .Count(out var total)
        .OrderByDescending(a => a.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<SupplierGetPageOutput>();

        var data = new PageOutput<SupplierGetPageOutput>()
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
    public async Task<List<SupplierGetListOutput>> GetListAsync(SupplierGetListInput input)
    {
        var key = input?.Name;
        var contactPerson = input?.ContactPerson;
        var phone = input?.Phone;
        var email = input?.Email;
        var rating = input?.Rating;

        var select = _supplierRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key))
        .WhereIf(contactPerson.NotNull(), a => a.ContactPerson.Contains(contactPerson))
        .WhereIf(phone.NotNull(), a => a.Phone.Contains(phone))
        .WhereIf(email.NotNull(), a => a.Email.Contains(email))
        .WhereIf(rating.HasValue, a => a.Rating == rating)
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
            select = select.OrderBy(a => a.Name);
        }

        return await select.ToListAsync<SupplierGetListOutput>();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(SupplierAddInput input)
    {
        if (await _supplierRep.Select.AnyAsync(a => a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["供应商名称已存在"]);
        }

        var entity = Mapper.Map<SupplierEntity>(input);
        await _supplierRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(SupplierUpdateInput input)
    {
        var entity = await _supplierRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["供应商不存在"]);
        }

        if (await _supplierRep.Select.AnyAsync(a => a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["供应商名称已存在"]);
        }

        Mapper.Map(input, entity);
        await _supplierRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        await _supplierRep.DeleteAsync(a => a.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        await _supplierRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        await _supplierRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _supplierRep.SoftDeleteAsync(ids);
    }
}
