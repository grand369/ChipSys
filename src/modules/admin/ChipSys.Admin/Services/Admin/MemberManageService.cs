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
        // 统计每个等级下已启用的最新等级记录的会员数量
        var q = await _memberLevelRep.Select
            .Where(a => a.Enabled)
            .OrderByDescending(a => a.CreatedTime)
            .ToListAsync();

        var latestByMember = q
            .GroupBy(x => x.MemberId)
            .Select(g => g.OrderByDescending(x => x.CreatedTime).First())
            .ToList();

        var result = latestByMember
            .GroupBy(x => x.Level)
            .Select(g => new LevelItem { Level = g.Key, Count = g.Count() })
            .OrderBy(x => x.Level)
            .ToList();

        return result;
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

        // 过滤等级：取最新启用等级记录
        if (!string.IsNullOrWhiteSpace(input.Level))
        {
            var latestLevels = _memberLevelRep.Orm.Select<MemberLevelEntity>()
                .Where(a => a.Enabled)
                .Where(a => a.Level == input.Level)
                .Where(a => !_memberLevelRep.Orm.Select<MemberLevelEntity>()
                    .Where(b => b.MemberId == a.MemberId && b.Enabled && b.CreatedTime > a.CreatedTime)
                    .Any())
                .ToList(a => a.MemberId);

            memberSel = memberSel.Where(a => latestLevels.Contains(a.Id));
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

        // 填充等级（可选：为了减少查询，这里二次查询最新等级映射）
        var memberIds = list.Select(x => x.Id).ToList();
        if (memberIds.Count > 0)
        {
            var latest = await _memberLevelRep.Select
                .Where(a => memberIds.Contains(a.MemberId) && a.Enabled)
                .OrderByDescending(a => a.CreatedTime)
                .ToListAsync();

            var latestMap = latest
                .GroupBy(x => x.MemberId)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.CreatedTime).First().Level);

            foreach (var row in list)
            {
                if (latestMap.TryGetValue(row.Id, out var lvl)) row.Level = lvl;
            }
        }

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
    /// 逻辑：关闭原有启用记录，插入新等级为启用记录
    /// </summary>
    [AdminTransaction]
    public virtual async Task AdjustLevelAsync(AdjustLevelInput input)
    {
        var member = await _memberRep.GetAsync(input.MemberId);
        if (member == null) throw ResultOutput.Exception(_adminLocalizer["会员不存在"]);

        // 禁用历史启用记录
        var olds = await _memberLevelRep.Select
            .Where(a => a.MemberId == input.MemberId && a.Enabled)
            .ToListAsync();
        foreach (var o in olds)
        {
            o.Enabled = false;
        }
        if (olds.Count > 0) await _memberLevelRep.UpdateAsync(olds);

        // 新增启用记录
        var entity = new MemberLevelEntity
        {
            MemberId = input.MemberId,
            Level = input.Level,
            Enabled = true,
            CreatedTime = System.DateTime.Now
        };
        await _memberLevelRep.InsertAsync(entity);
    }
}



