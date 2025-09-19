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
using ChipSys.Client.Contracts.Services.Client.Dto;
using ChipSys.Client.Contracts.Services.Client;
using FreeSql;
using System.Linq;
using System.Collections.Generic;

namespace ChipSys.Admin.Services.Admin;

/// <summary>
/// 管理员端会员等级价格方案管理服务
/// </summary>
[Order(108)]
[DynamicApi(Area = "admin")]
public class MemberLevelPricePlanManageService : BaseService, IDynamicApi
{
    private readonly AdminRepositoryBase<MemberLevelPricePlanEntity> _pricePlanRep;
    private readonly AdminRepositoryBase<MemberLevelEntity> _memberLevelRep;
    private readonly AdminLocalizer _adminLocalizer;

    public MemberLevelPricePlanManageService(
        AdminRepositoryBase<MemberLevelPricePlanEntity> pricePlanRep,
        AdminRepositoryBase<MemberLevelEntity> memberLevelRep,
        AdminLocalizer adminLocalizer)
    {
        _pricePlanRep = pricePlanRep;
        _memberLevelRep = memberLevelRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 获取会员等级价格方案信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<MemberLevelPricePlanGetOutput> GetAsync(long id)
    {
        var result = await _pricePlanRep.GetAsync<MemberLevelPricePlanGetOutput>(id);
        if (result == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["会员等级价格方案记录不存在"]);
        }

        return result;
    }

    /// <summary>
    /// 获取会员等级价格方案分页列表（管理员端）
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<MemberLevelPricePlanGetPageOutput>> GetPageAsync(PageInput<MemberLevelPricePlanGetPageInput> input)
    {
        var memberLevelId = input.Filter?.MemberLevelId;
        var planType = input.Filter?.PlanType;
        var enabled = input.Filter?.Enabled;

        var list = await _pricePlanRep.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .WhereIf(memberLevelId.HasValue, a => a.MemberLevelId == memberLevelId.Value)
            .WhereIf(planType.NotNull(), a => a.PlanType == planType)
            .WhereIf(enabled.HasValue, a => a.Enabled == enabled.Value)
            .Count(out var total)
            .OrderBy(a => a.Sort)
            .OrderByDescending(a => a.CreatedTime)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<MemberLevelPricePlanGetPageOutput>();

        return new PageOutput<MemberLevelPricePlanGetPageOutput>
        {
            List = list,
            Total = total
        };
    }

    /// <summary>
    /// 添加会员等级价格方案
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AdminTransaction]
    public virtual async Task<long> AddAsync(MemberLevelPricePlanAddInput input)
    {
        // 验证会员等级是否存在
        var memberLevel = await _memberLevelRep.GetAsync(input.MemberLevelId);
        if (memberLevel == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["会员等级不存在"]);
        }

        // 检查是否已存在相同类型的价格方案
        var existing = await _pricePlanRep.Select
            .Where(a => a.MemberLevelId == input.MemberLevelId && a.PlanType == input.PlanType && a.Enabled)
            .FirstAsync();

        if (existing != null)
        {
            throw ResultOutput.Exception(_adminLocalizer["该等级已存在相同类型的价格方案"]);
        }

        var entity = Mapper.Map<MemberLevelPricePlanEntity>(input);
        entity.Enabled = true;
        await _pricePlanRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 更新会员等级价格方案
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    [AdminTransaction]
    public virtual async Task UpdateAsync(MemberLevelPricePlanUpdateInput input)
    {
        var entity = await _pricePlanRep.GetAsync(input.Id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["会员等级价格方案记录不存在"]);
        }

        // 检查是否已存在相同类型的价格方案（排除自己）
        var existing = await _pricePlanRep.Select
            .Where(a => a.MemberLevelId == input.MemberLevelId && a.PlanType == input.PlanType && a.Enabled && a.Id != input.Id)
            .FirstAsync();

        if (existing != null)
        {
            throw ResultOutput.Exception(_adminLocalizer["该等级已存在相同类型的价格方案"]);
        }

        Mapper.Map(input, entity);
        await _pricePlanRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除会员等级价格方案
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var entity = await _pricePlanRep.GetAsync(id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["会员等级价格方案记录不存在"]);
        }

        await _pricePlanRep.DeleteAsync(id);
    }

    /// <summary>
    /// 获取指定等级的价格方案列表
    /// </summary>
    /// <param name="memberLevelId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<MemberLevelPricePlanGetOutput>> GetByMemberLevelIdAsync(long memberLevelId)
    {
        var list = await _pricePlanRep.Select
            .Where(a => a.MemberLevelId == memberLevelId && a.Enabled)
            .OrderBy(a => a.Sort)
            .OrderByDescending(a => a.CreatedTime)
            .ToListAsync<MemberLevelPricePlanGetOutput>();

        return list;
    }

    /// <summary>
    /// 批量设置价格方案
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AdminTransaction]
    public virtual async Task BatchSetPricePlansAsync(BatchSetPricePlansInput input)
    {
        // 验证会员等级是否存在
        var memberLevel = await _memberLevelRep.GetAsync(input.MemberLevelId);
        if (memberLevel == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["会员等级不存在"]);
        }

        // 删除该等级的所有现有价格方案
        await _pricePlanRep.DeleteAsync(a => a.MemberLevelId == input.MemberLevelId);

        // 添加新的价格方案
        if (input.PricePlans?.Count > 0)
        {
            var entities = input.PricePlans.Select(plan => new MemberLevelPricePlanEntity
            {
                MemberLevelId = input.MemberLevelId,
                PlanType = plan.PlanType,
                PlanName = plan.PlanName,
                OriginalPrice = plan.OriginalPrice,
                DiscountedPrice = plan.DiscountedPrice,
                DiscountPercent = plan.DiscountPercent,
                Description = plan.Description,
                IsRecommended = plan.IsRecommended,
                Enabled = plan.Enabled,
                Sort = plan.Sort
            }).ToList();

            await _pricePlanRep.InsertAsync(entities);
        }
    }

    /// <summary>
    /// 切换价格方案启用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enabled"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task ToggleEnabledAsync(long id, bool enabled)
    {
        var entity = await _pricePlanRep.GetAsync(id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["会员等级价格方案记录不存在"]);
        }

        entity.Enabled = enabled;
        await _pricePlanRep.UpdateAsync(entity);
    }
}
