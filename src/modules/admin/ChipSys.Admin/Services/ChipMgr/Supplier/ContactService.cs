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
/// 联系人服务
/// </summary>
[Order(72)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class ContactService : BaseService, IContactService, IDynamicApi
{
    private readonly AdminRepositoryBase<ContactEntity> _contactRep;
    private readonly AdminRepositoryBase<SupplierEntity> _supplierRep;
    private readonly AdminLocalizer _adminLocalizer;

    public ContactService(AdminRepositoryBase<ContactEntity> contactRep, 
        AdminRepositoryBase<SupplierEntity> supplierRep,
        AdminLocalizer adminLocalizer)
    {
        _contactRep = contactRep;
        _supplierRep = supplierRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ContactGetOutput> GetAsync(long id)
    {
        var result = await _contactRep.GetAsync<ContactGetOutput>(id);
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<ContactGetPageOutput>> GetPageAsync(PageInput<ContactGetPageInput> input)
    {
        var key = input.Filter?.Name;
        var phone = input.Filter?.Phone;
        var email = input.Filter?.Email;
        var supplierId = input.Filter?.SupplierId;

        var list = await _contactRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key))
        .WhereIf(phone.NotNull(), a => a.Phone.Contains(phone))
        .WhereIf(email.NotNull(), a => a.Email.Contains(email))
        .WhereIf(supplierId.HasValue, a => a.SupplierId == supplierId)
        .Count(out var total)
        .OrderByDescending(a => a.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<ContactGetPageOutput>();

        var data = new PageOutput<ContactGetPageOutput>()
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
    public async Task<List<ContactGetListOutput>> GetListAsync(ContactGetListInput input)
    {
        var key = input?.Name;
        var phone = input?.Phone;
        var email = input?.Email;
        var supplierId = input?.SupplierId;

        var select = _contactRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key))
        .WhereIf(phone.NotNull(), a => a.Phone.Contains(phone))
        .WhereIf(email.NotNull(), a => a.Email.Contains(email))
        .WhereIf(supplierId.HasValue, a => a.SupplierId == supplierId)
        .OrderBy(a => a.Name);

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

        return await select.ToListAsync<ContactGetListOutput>();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(ContactAddInput input)
    {
        if (!await _supplierRep.Select.AnyAsync(a => a.Id == input.SupplierId))
        {
            throw ResultOutput.Exception(_adminLocalizer["供应商不存在"]);
        }

        var entity = Mapper.Map<ContactEntity>(input);
        await _contactRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(ContactUpdateInput input)
    {
        var entity = await _contactRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["联系人不存在"]);
        }

        if (!await _supplierRep.Select.AnyAsync(a => a.Id == input.SupplierId))
        {
            throw ResultOutput.Exception(_adminLocalizer["供应商不存在"]);
        }

        Mapper.Map(input, entity);
        await _contactRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        await _contactRep.DeleteAsync(a => a.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        await _contactRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        await _contactRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _contactRep.SoftDeleteAsync(ids);
    }
}
