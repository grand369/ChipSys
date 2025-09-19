using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Services;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Repositories;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Client.Contracts.Domain.Client;
using ChipSys.Admin.Domain.User;
using FreeSql;
using System.Linq;
using System.Collections.Generic;

namespace ChipSys.Admin.Services.Admin;

/// <summary>
/// 平台管理/权限管理下：会员管理服务
/// - 管理会员基本信息（新增/编辑/禁用）
/// - 调整会员等级（独立操作）
/// - 左右联动：左侧等级列表，右侧对应会员列表
/// </summary>
[Order(210)]
[DynamicApi(Area = "admin")] 
public class MemberManageService : BaseService, IDynamicApi
{
    private readonly AdminRepositoryBase<MemberEntity> _memberRep;
    private readonly AdminRepositoryBase<MemberLevelEntity> _memberLevelRep;
    private readonly AdminRepositoryBase<UserEntity> _userRep;
    private readonly AdminLocalizer _adminLocalizer;

    public MemberManageService(
        AdminRepositoryBase<MemberEntity> memberRep,
        AdminRepositoryBase<MemberLevelEntity> memberLevelRep,
        AdminRepositoryBase<UserEntity> userRep,
        AdminLocalizer adminLocalizer)
    {
        _memberRep = memberRep;
        _memberLevelRep = memberLevelRep;
        _userRep = userRep;
        _adminLocalizer = adminLocalizer;
    }

    #region DTOs
    public class MemberGetPageInput
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Keyword { get; set; }
        public string? Level { get; set; }
        public bool? Enabled { get; set; }
    }

    public class MemberSimpleOutput
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? NickName { get; set; }
        public string? RealName { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public bool Enabled { get; set; }
        public string? Level { get; set; }
        public System.DateTime CreatedTime { get; set; }
    }

    public class MemberAddInput
    {
        public string? NickName { get; set; }
        public string? RealName { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string Level { get; set; } = "Free";
        public DateTime? LevelEffectiveTime { get; set; }
        public DateTime? LevelExpireTime { get; set; }
    }

    public class MemberUpdateInput : MemberAddInput
    {
        public long Id { get; set; }
        public bool? Enabled { get; set; }
    }

    public class ToggleEnabledInput
    {
        public long Id { get; set; }
        public bool Enabled { get; set; }
    }

    public class AdjustLevelInput
    {
        public long MemberId { get; set; }
        public string Level { get; set; }
    }

    public class LevelItem
    {
        public string Level { get; set; }
        public int Count { get; set; }
    }
    #endregion

    /// <summary>
    /// 左侧：获取会员等级列表（包含当前启用等级的会员数量）
    /// </summary>
    [HttpGet]
    public async Task<List<LevelItem>> GetLevelsAsync()
    {
        // 获取所有已定义的等级模板
        var allLevelTemplates = await _memberLevelRep.Select
            .Where(a => a.Enabled)
            .ToListAsync();

        // 统计每个等级下的会员数量（直接从会员表统计）
        var memberCountByLevel = await _memberRep.Select
            .Where(a => a.Enabled)
            .GroupBy(a => a.Level)
            .ToListAsync(a => new { Level = a.Key??"", Count = a.Count() });

        var memberCountDict = memberCountByLevel.ToDictionary(x => x.Level, x => x.Count);

        var allLevels =  allLevelTemplates.Select(p=>new LevelItem
        {
            Level = p.Level,
            Count = memberCountDict.ContainsKey(p.Level) ? memberCountDict[p.Level] : 0
        }).ToList();

        // 合并模板等级和实际会员等级，确保所有等级都显示
        //var allLevels = allLevelTemplates
        //    .Select(t => t.Level)
        //    .Union(memberCountDict.Keys)
        //    .Distinct()
        //    .OrderBy(x => x)
        //    .Select(level => new LevelItem 
        //    { 
        //        Level = level, 
        //        Count = memberCountDict.ContainsKey(level) ? memberCountDict[level] : 0 
        //    })
        //    .ToList();

        return allLevels;
    }

    /// <summary>
    /// 右侧：按等级、关键字分页查询会员列表
    /// </summary>
    [HttpPost]
    public async Task<PageOutput<MemberSimpleOutput>> GetMembersAsync(MemberGetPageInput input)
    {
        var memberSel = _memberRep.Select;

        if (input.Enabled.HasValue)
            memberSel = memberSel.Where(a => a.Enabled == input.Enabled.Value);

        if (!string.IsNullOrWhiteSpace(input.Keyword))
        {
            var kw = input.Keyword.Trim();
            memberSel = memberSel.Where(a => a.NickName.Contains(kw) || a.RealName.Contains(kw));
        }

        // 过滤等级：直接从会员表的等级字段过滤
        if (!string.IsNullOrWhiteSpace(input.Level))
        {
            memberSel = memberSel.Where(a => a.Level == input.Level);
        }

        // 先获取会员列表
        var members = await memberSel
            .Count(out var total)
            .OrderByDescending(a => a.CreatedTime)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync();

        // 获取用户信息
        var userIds = members.Select(m => m.UserId).ToList();
        var users = await _userRep.Select
            .Where(a => userIds.Contains(a.Id))
            .ToListAsync();

        var userDict = users.ToDictionary(u => u.Id, u => u);

        // 组装结果
        var list = members.Select(m => new MemberSimpleOutput
        {
            Id = m.Id,
            UserId = m.UserId,
            NickName = m.NickName,
            RealName = m.RealName,
            Mobile = userDict.TryGetValue(m.UserId, out var user) ? user.Mobile : null,
            Email = userDict.TryGetValue(m.UserId, out var user2) ? user2.Email : null,
            Enabled = m.Enabled,
            CreatedTime = m.CreatedTime ?? DateTime.Now
        }).ToList();

        // 等级信息已经直接从会员表获取，无需额外查询

        return new PageOutput<MemberSimpleOutput>
        {
            List = list,
            Total = total
        };
    }

    /// <summary>
    /// 新增会员基本信息（不含等级）
    /// 自动创建 User 与 Member 关系（若需要）
    /// </summary>
    [AdminTransaction]
    public virtual async Task<long> AddAsync(MemberAddInput input)
    {
        // 创建用户
        var user = new UserEntity
        {
            UserName = input.Mobile ?? input.Email ?? input.NickName ?? input.RealName,
            Name = input.RealName ?? input.NickName,
            Mobile = input.Mobile,
            Email = input.Email,
            Enabled = true,
            CreatedTime = System.DateTime.Now
        };
        await _userRep.InsertAsync(user);

        // 创建会员
        var member = new MemberEntity
        {
            UserId = user.Id,
            NickName = input.NickName ?? input.RealName ?? user.UserName,
            RealName = input.RealName,
            Level = input.Level,
            LevelEffectiveTime = input.LevelEffectiveTime,
            LevelExpireTime = input.LevelExpireTime,
            Enabled = true,
            CreatedTime = System.DateTime.Now
        };
        await _memberRep.InsertAsync(member);
        return member.Id;
    }

    /// <summary>
    /// 更新会员基本信息（不含等级）
    /// </summary>
    public async Task UpdateAsync(MemberUpdateInput input)
    {
        var member = await _memberRep.GetAsync(input.Id);
        if (member == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["会员不存在"]);
        }

        // 更新 Member
        member.NickName = input.NickName ?? member.NickName;
        member.RealName = input.RealName ?? member.RealName;
        if (input.Enabled.HasValue) member.Enabled = input.Enabled.Value;
        await _memberRep.UpdateAsync(member);

        // 更新 User 基本信息
        var user = await _userRep.GetAsync(member.UserId);
        if (user != null)
        {
            user.Name = input.RealName ?? user.Name;
            user.Mobile = input.Mobile ?? user.Mobile;
            user.Email = input.Email ?? user.Email;
            await _userRep.UpdateAsync(user);
        }
    }

    /// <summary>
    /// 启用/禁用会员（Enabled=1/0）
    /// </summary>
    [HttpPost]
    public async Task ToggleEnabledAsync(ToggleEnabledInput input)
    {
        var member = await _memberRep.GetAsync(input.Id);
        if (member == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["会员不存在"]);
        }
        member.Enabled = input.Enabled;
        await _memberRep.UpdateAsync(member);
    }

    /// <summary>
    /// 调整会员等级（独立按钮）
    /// 逻辑：直接更新会员表的等级字段
    /// </summary>
    [AdminTransaction]
    public virtual async Task AdjustLevelAsync(AdjustLevelInput input)
    {
        var member = await _memberRep.GetAsync(input.MemberId);
        if (member == null) throw ResultOutput.Exception(_adminLocalizer["会员不存在"]);

        // 直接更新会员表的等级字段
        member.Level = input.Level;
        member.LevelEffectiveTime = DateTime.Now;
        // 可以根据等级模板设置过期时间
        var levelTemplate = await _memberLevelRep.Select
            .Where(a => a.Level == input.Level && a.Enabled)
            .FirstAsync();
        
        if (levelTemplate != null)
        {
            // 根据等级模板设置过期时间（例如：付费等级1年有效期）
            if (input.Level != "Free")
            {
                member.LevelExpireTime = DateTime.Now.AddYears(1);
            }
        }

        await _memberRep.UpdateAsync(member);
    }
}



