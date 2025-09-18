using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Services;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Repositories;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Client.Contracts.Services.Client;
using ChipSys.Client.Contracts.Services.Client.Dto;
using ChipSys.Client.Contracts.Domain.Client;
using FreeSql;
using System.Text.Json;

namespace ChipSys.Admin.Services.Client;

/// <summary>
/// 供应商申请服务
/// </summary>
[Order(108)]
[DynamicApi(Area = "client")]
public class SupplierApplicationService : BaseService, ISupplierApplicationService, IDynamicApi
{
    private readonly AdminRepositoryBase<SupplierApplicationEntity> _supplierApplicationRep;
    private readonly AdminRepositoryBase<MemberLevelEntity> _memberLevelRep;
    private readonly AdminLocalizer _adminLocalizer;

    public SupplierApplicationService(
        AdminRepositoryBase<SupplierApplicationEntity> supplierApplicationRep,
        AdminRepositoryBase<MemberLevelEntity> memberLevelRep,
        AdminLocalizer adminLocalizer)
    {
        _supplierApplicationRep = supplierApplicationRep;
        _memberLevelRep = memberLevelRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 提交供应商申请
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(SupplierApplicationAddInput input)
    {
        // 检查是否已有待审核的申请
        var existing = await _supplierApplicationRep.Select
            .Where(a => a.MemberId == User.Id && a.Status == "pending")
            .FirstAsync();

        if (existing != null)
        {
            throw ResultOutput.Exception(_adminLocalizer["您已有待审核的供应商申请"]);
        }

        // 检查会员等级是否支持申请供应商
        var memberLevel = await _memberLevelRep.Select
            .Where(a => a.MemberId == User.Id && a.Enabled)
            .OrderByDescending(a => a.CreatedTime)
            .FirstAsync();

        if (memberLevel == null || memberLevel.Level == "Free")
        {
            throw ResultOutput.Exception(_adminLocalizer["免费会员不能申请成为供应商，请先升级会员等级"]);
        }

        // 创建申请记录
        var entity = Mapper.Map<SupplierApplicationEntity>(input);
        entity.MemberId = User.Id;
        entity.Status = "pending";
        entity.CreatedTime = DateTime.Now;

        // 处理申请材料
        if (input.Materials != null && input.Materials.Length > 0)
        {
            entity.Materials = JsonSerializer.Serialize(input.Materials);
        }

        await _supplierApplicationRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 获取申请详情
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
    /// 获取申请分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<SupplierApplicationGetOutput>> GetPageAsync(PageInput<SupplierApplicationGetPageInput> input)
    {
        var memberId = input.Filter?.MemberId;
        var status = input.Filter?.Status;
        var companyName = input.Filter?.CompanyName;

        var list = await _supplierApplicationRep.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .WhereIf(memberId.HasValue, a => a.MemberId == memberId)
            .WhereIf(status.NotNull(), a => a.Status == status)
            .WhereIf(companyName.NotNull(), a => a.CompanyName.Contains(companyName))
            .Count(out var total)
            .OrderByDescending(a => a.CreatedTime)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<SupplierApplicationGetOutput>();

        return new PageOutput<SupplierApplicationGetOutput>
        {
            List = list,
            Total = total
        };
    }

    /// <summary>
    /// 获取我的申请
    /// </summary>
    /// <returns></returns>
    public async Task<SupplierApplicationGetOutput?> GetMyApplicationAsync()
    {
        var result = await _supplierApplicationRep.Select
            .Where(a => a.MemberId == User.Id)
            .OrderByDescending(a => a.CreatedTime)
            .FirstAsync<SupplierApplicationGetOutput>();

        return result;
    }
}