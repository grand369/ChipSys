using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Services;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Repositories;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Client.Contracts.Domain.Client;
using ChipSys.Client.Contracts.Services.Client;
using ChipSys.Client.Contracts.Services.Client.Dto;
using FreeSql;
using System.Linq;
using System.Collections.Generic;

namespace ChipSys.Admin.Services.Client;

/// <summary>
/// 供应商申请服务
/// </summary>
[Order(102)]
[DynamicApi(Area = "client")]
public class SupplierApplicationService : BaseService, ISupplierApplicationService, IDynamicApi
{
    private readonly AdminRepositoryBase<SupplierApplicationEntity> _supplierApplicationRep;
    private readonly AdminLocalizer _adminLocalizer;

    public SupplierApplicationService(
        AdminRepositoryBase<SupplierApplicationEntity> supplierApplicationRep,
        AdminLocalizer adminLocalizer)
    {
        _supplierApplicationRep = supplierApplicationRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SupplierApplicationGetOutput> GetAsync(long id)
    {
        var result = await _supplierApplicationRep.GetAsync<SupplierApplicationGetOutput>(id);
        if (result == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["申请记录不存在"]);
        }

        // 检查权限：只能查看自己的申请
        if (result.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限查看此申请"]);
        }

        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<SupplierApplicationGetPageOutput>> GetPageAsync(SupplierApplicationGetPageInput input)
    {
        var companyName = input.Filter?.CompanyName;
        var contactName = input.Filter?.ContactName;
        var status = input.Filter?.Status;

        var list = await _supplierApplicationRep.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .Where(a => a.MemberId == User.Id)
            .WhereIf(companyName.NotNull(), a => a.CompanyName.Contains(companyName))
            .WhereIf(contactName.NotNull(), a => a.ContactName.Contains(contactName))
            .WhereIf(status.NotNull(), a => a.Status == status)
            .Count(out var total)
            .OrderByDescending(a => a.CreatedTime)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<SupplierApplicationGetPageOutput>();

        return new PageOutput<SupplierApplicationGetPageOutput>
        {
            List = list,
            Total = total
        };
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(SupplierApplicationAddInput input)
    {
        // 检查是否已有待审核的申请
        var hasPending = await HasPendingApplicationAsync();
        if (hasPending)
        {
            throw ResultOutput.Exception(_adminLocalizer["您已有待审核的申请，请等待审核结果"]);
        }

        var entity = Mapper.Map<SupplierApplicationEntity>(input);
        entity.MemberId = User.Id;
        entity.Status = "Draft"; // 默认为草稿状态
        await _supplierApplicationRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(SupplierApplicationUpdateInput input)
    {
        var entity = await _supplierApplicationRep.GetAsync(input.Id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["申请记录不存在"]);
        }

        // 检查权限：只能修改自己的申请
        if (entity.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限修改此申请"]);
        }

        // 只有草稿状态才能修改
        if (entity.Status != "Draft")
        {
            throw ResultOutput.Exception(_adminLocalizer["只有草稿状态的申请才能修改"]);
        }

        Mapper.Map(input, entity);
        await _supplierApplicationRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var entity = await _supplierApplicationRep.GetAsync(id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["申请记录不存在"]);
        }

        // 检查权限：只能删除自己的申请
        if (entity.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限删除此申请"]);
        }

        // 只有草稿状态才能删除
        if (entity.Status != "Draft")
        {
            throw ResultOutput.Exception(_adminLocalizer["只有草稿状态的申请才能删除"]);
        }

        await _supplierApplicationRep.DeleteAsync(id);
    }

    /// <summary>
    /// 提交申请
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task SubmitAsync(long id)
    {
        var entity = await _supplierApplicationRep.GetAsync(id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["申请记录不存在"]);
        }

        // 检查权限：只能提交自己的申请
        if (entity.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限提交此申请"]);
        }

        // 只有草稿状态才能提交
        if (entity.Status != "Draft")
        {
            throw ResultOutput.Exception(_adminLocalizer["只有草稿状态的申请才能提交"]);
        }

        // 检查是否已有待审核的申请
        var hasPending = await HasPendingApplicationAsync();
        if (hasPending)
        {
            throw ResultOutput.Exception(_adminLocalizer["您已有待审核的申请，请等待审核结果"]);
        }

        entity.Status = "Pending";
        entity.SubmittedTime = DateTime.Now;
        await _supplierApplicationRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 撤销申请
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task CancelAsync(long id)
    {
        var entity = await _supplierApplicationRep.GetAsync(id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["申请记录不存在"]);
        }

        // 检查权限：只能撤销自己的申请
        if (entity.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限撤销此申请"]);
        }

        // 只有待审核状态才能撤销
        if (entity.Status != "Pending")
        {
            throw ResultOutput.Exception(_adminLocalizer["只有待审核状态的申请才能撤销"]);
        }

        entity.Status = "Cancelled";
        entity.CancelledTime = DateTime.Now;
        await _supplierApplicationRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 检查是否已有待审核申请
    /// </summary>
    /// <returns></returns>
    public async Task<bool> HasPendingApplicationAsync()
    {
        var count = await _supplierApplicationRep.Select
            .Where(a => a.MemberId == User.Id && a.Status == "Pending")
            .CountAsync();
        return count > 0;
    }
}
