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
/// 会员等级服务
/// </summary>
[Order(105)]
[DynamicApi(Area = "client")]
public class MemberLevelService : BaseService, IMemberLevelService, IDynamicApi
{
    private readonly AdminRepositoryBase<MemberLevelEntity> _memberLevelRep;
    private readonly AdminLocalizer _adminLocalizer;

    public MemberLevelService(
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

        // 检查权限：只能查看自己的等级信息
        if (result.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限查看此等级信息"]);
        }

        return result;
    }

    /// <summary>
    /// 获取会员等级分页列表
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
            .Where(a => a.MemberId == User.Id)
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
    /// 添加会员等级
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(MemberLevelAddInput input)
    {
        // 确定会员ID：如果输入中没有指定，则使用当前用户ID
        var memberId = input.MemberId ?? User.Id;
        
        // 检查是否已有相同等级的记录
        var existing = await _memberLevelRep.Select
            .Where(a => a.MemberId == memberId && a.Level == input.Level && a.Enabled)
            .FirstAsync();

        if (existing != null)
        {
            throw ResultOutput.Exception(_adminLocalizer["该会员已拥有该等级的会员权限"]);
        }

        var entity = Mapper.Map<MemberLevelEntity>(input);
        entity.MemberId = memberId;
        entity.Enabled = true;
        await _memberLevelRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改会员等级
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

        // 检查权限：只能修改自己的等级信息
        if (entity.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限修改此等级信息"]);
        }

        Mapper.Map(input, entity);
        await _memberLevelRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除会员等级
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

        // 检查权限：只能删除自己的等级信息
        if (entity.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限删除此等级信息"]);
        }

        await _memberLevelRep.DeleteAsync(id);
    }

    /// <summary>
    /// 获取当前会员等级信息
    /// </summary>
    /// <param name="memberId"></param>
    /// <returns></returns>
    public async Task<MemberLevelGetOutput> GetCurrentLevelAsync(long memberId)
    {
        var memberLevel = await _memberLevelRep.Select
            .Where(a => a.MemberId == memberId && a.Enabled)
            .OrderByDescending(a => a.CreatedTime)
            .FirstAsync();

        if (memberLevel == null)
        {
            // 返回默认免费会员等级
            return new MemberLevelGetOutput
            {
                Id = 0,
                MemberId = memberId,
                Level = "Free",
                CategoryLimit = 0,
                ProductDataLimit = 50,
                SupplierDataLimit = 50,
                ShowFullContactInfo = false,
                Enabled = true,
                CreatedTime = DateTime.Now
            };
        }

        return Mapper.Map<MemberLevelGetOutput>(memberLevel);
    }

    /// <summary>
    /// 升级会员等级
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> UpgradeLevelAsync(MemberLevelAddInput input)
    {
        // 确定会员ID：如果输入中没有指定，则使用当前用户ID
        var memberId = input.MemberId ?? User.Id;
        
        // 验证等级配置
        var levelConfig = GetLevelConfig(input.Level);
        if (levelConfig == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["无效的会员等级"]);
        }

        // 检查是否已有相同或更高级别的记录
        var existing = await _memberLevelRep.Select
            .Where(a => a.MemberId == memberId && a.Enabled)
            .OrderByDescending(a => a.CreatedTime)
            .FirstAsync();

        if (existing != null && IsHigherOrEqualLevel(input.Level, existing.Level))
        {
            throw ResultOutput.Exception(_adminLocalizer["该会员已拥有相同或更高级别的会员权限"]);
        }

        // 创建新的等级记录
        var entity = new MemberLevelEntity
        {
            MemberId = memberId,
            Level = input.Level,
            CategoryLimit = levelConfig?.CategoryLimit ?? 0,
            ProductDataLimit = levelConfig?.ProductDataLimit ?? 50,
            SupplierDataLimit = levelConfig?.SupplierDataLimit ?? 50,
            ShowFullContactInfo = levelConfig?.ShowFullContactInfo ?? false,
            Enabled = true,
            CreatedTime = DateTime.Now
        };

        await _memberLevelRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 获取等级配置
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    private (int CategoryLimit, int ProductDataLimit, int SupplierDataLimit, bool ShowFullContactInfo)? GetLevelConfig(string level)
    {
        return level switch
        {
            "Basic" => (2, 100, 100, true),
            "Standard" => (5, 500, 500, true),
            "Premium" => (10, 1000, 1000, true),
            "Enterprise" => (int.MaxValue, int.MaxValue, int.MaxValue, true),
            _ => null
        };
    }

    /// <summary>
    /// 比较等级高低
    /// </summary>
    /// <param name="level1"></param>
    /// <param name="level2"></param>
    /// <returns></returns>
    private bool IsHigherOrEqualLevel(string level1, string level2)
    {
        var levelOrder = new Dictionary<string, int>
        {
            { "Free", 0 },
            { "Basic", 1 },
            { "Standard", 2 },
            { "Premium", 3 },
            { "Enterprise", 4 }
        };

        return levelOrder.GetValueOrDefault(level1, 0) >= levelOrder.GetValueOrDefault(level2, 0);
    }

    /// <summary>
    /// 更新会员等级
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateLevelAsync(MemberLevelUpdateInput input)
    {
        await UpdateAsync(input);
    }

    /// <summary>
    /// 检查会员权限
    /// </summary>
    /// <param name="memberId"></param>
    /// <param name="permissionType"></param>
    /// <returns></returns>
    public async Task<bool> CheckPermissionAsync(long memberId, string permissionType)
    {
        var memberLevel = await _memberLevelRep.Select
            .Where(a => a.MemberId == memberId && a.Enabled)
            .OrderByDescending(a => a.CreatedTime)
            .FirstAsync();

        if (memberLevel == null)
        {
            // 默认免费会员权限
            return permissionType switch
            {
                "Inquiry" => false,
                "FullContactInfo" => false,
                _ => true
            };
        }

        return permissionType switch
        {
            "Inquiry" => memberLevel.Level != "Free",
            "FullContactInfo" => memberLevel.ShowFullContactInfo,
            _ => true
        };
    }
}
