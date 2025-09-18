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
using FreeSql;
using System.Linq;
using System.Collections.Generic;

namespace ChipSys.Admin.Services.Admin;

/// <summary>
/// 管理员端会员等级管理服务
/// </summary>
[Order(106)]
[DynamicApi(Area = "admin")]
public class MemberLevelManageService : BaseService, IDynamicApi
{
    private readonly AdminRepositoryBase<MemberLevelEntity> _memberLevelRep;
    private readonly AdminLocalizer _adminLocalizer;

    public MemberLevelManageService(
        AdminRepositoryBase<MemberLevelEntity> memberLevelRep,
        AdminLocalizer adminLocalizer)
    {
        _memberLevelRep = memberLevelRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 获取会员等级信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<MemberLevelGetOutput> GetAsync(long id)
    {
        var result = await _memberLevelRep.GetAsync<MemberLevelGetOutput>(id);
        if (result == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["会员等级记录不存在"]);
        }

        return result;
    }

    /// <summary>
    /// 获取会员等级分页列表（管理员端，可查看所有等级）
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<MemberLevelGetPageOutput>> GetPageAsync(PageInput<MemberLevelGetPageInput> input)
    {
        var level = input.Filter?.Level;
        var enabled = input.Filter?.Enabled;

        var list = await _memberLevelRep.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .WhereIf(level.NotNull(), a => a.Level == level)
            .WhereIf(enabled.HasValue, a => a.Enabled == enabled)
            .Count(out var total)
            .OrderByDescending(a => a.CreatedTime)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<MemberLevelGetPageOutput>();

        return new PageOutput<MemberLevelGetPageOutput>
        {
            List = list,
            Total = total
        };
    }

    /// <summary>
    /// 添加会员等级模板（管理员端，不关联具体会员）
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(MemberLevelAddInput input)
    {
        // 管理员端创建等级模板，不关联具体会员（MemberId设为0或null）
        var entity = Mapper.Map<MemberLevelEntity>(input);
        entity.MemberId = 0; // 0表示这是等级模板，不关联具体会员
        entity.Enabled = input.Enabled;
        await _memberLevelRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改会员等级模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(MemberLevelUpdateInput input)
    {
        var entity = await _memberLevelRep.GetAsync(input.Id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["会员等级记录不存在"]);
        }

        Mapper.Map(input, entity);
        await _memberLevelRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除会员等级模板
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var entity = await _memberLevelRep.GetAsync(id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["会员等级记录不存在"]);
        }

        await _memberLevelRep.DeleteAsync(id);
    }

    /// <summary>
    /// 获取所有等级模板（用于下拉选择）
    /// </summary>
    /// <returns></returns>
    public async Task<List<MemberLevelGetOutput>> GetLevelTemplatesAsync()
    {
        var list = await _memberLevelRep.Select
            .Where(a => a.MemberId == 0 && a.Enabled) // 只获取等级模板
            .OrderBy(a => a.Level)
            .ToListAsync<MemberLevelGetOutput>();

        return list;
    }
}
