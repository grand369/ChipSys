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
/// 会员收藏服务
/// </summary>
[Order(100)]
[DynamicApi(Area = "client")]
public class MemberFavoriteService : BaseService, IMemberFavoriteService, IDynamicApi
{
    private readonly AdminRepositoryBase<MemberFavoriteEntity> _memberFavoriteRep;
    private readonly AdminRepositoryBase<MemberLevelEntity> _memberLevelRep;
    private readonly AdminLocalizer _adminLocalizer;

    public MemberFavoriteService(
        AdminRepositoryBase<MemberFavoriteEntity> memberFavoriteRep,
        AdminRepositoryBase<MemberLevelEntity> memberLevelRep,
        AdminLocalizer adminLocalizer)
    {
        _memberFavoriteRep = memberFavoriteRep;
        _memberLevelRep = memberLevelRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<MemberFavoriteGetOutput> GetAsync(long id)
    {
        var result = await _memberFavoriteRep.GetAsync<MemberFavoriteGetOutput>(id);
        if (result == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["收藏记录不存在"]);
        }

        // 检查权限：只能查看自己的收藏
        if (result.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限查看此收藏记录"]);
        }

        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<MemberFavoriteGetPageOutput>> GetPageAsync(MemberFavoriteGetPageInput input)
    {
        var favoriteType = input.Filter?.FavoriteType;
        var favoriteName = input.Filter?.FavoriteName;

        var list = await _memberFavoriteRep.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .Where(a => a.MemberId == User.Id)
            .WhereIf(favoriteType.NotNull(), a => a.FavoriteType == favoriteType)
            .WhereIf(favoriteName.NotNull(), a => a.Remark.Contains(favoriteName))
            .Count(out var total)
            .OrderByDescending(a => a.CreatedTime)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<MemberFavoriteGetPageOutput>();

        // 填充收藏对象名称 - 使用简单的占位符，避免循环查询
        foreach (var item in list)
        {
            item.FavoriteName = GetSimpleFavoriteName(item.FavoriteType, item.FavoriteId);
        }

        return new PageOutput<MemberFavoriteGetPageOutput>
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
    public async Task<long> AddAsync(MemberFavoriteAddInput input)
    {
        // 检查是否已收藏
        if (await IsFavoritedAsync(input.FavoriteType, input.FavoriteId))
        {
            throw ResultOutput.Exception(_adminLocalizer["该对象已被收藏"]);
        }

        // 验证收藏对象是否存在
        await ValidateFavoriteObjectAsync(input.FavoriteType, input.FavoriteId);

        var entity = Mapper.Map<MemberFavoriteEntity>(input);
        entity.MemberId = User.Id;
        await _memberFavoriteRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(MemberFavoriteUpdateInput input)
    {
        var entity = await _memberFavoriteRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["收藏记录不存在"]);
        }

        if (entity.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限修改此收藏记录"]);
        }

        Mapper.Map(input, entity);
        await _memberFavoriteRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var entity = await _memberFavoriteRep.GetAsync(id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["收藏记录不存在"]);
        }

        if (entity.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限删除此收藏记录"]);
        }

        await _memberFavoriteRep.DeleteAsync(a => a.Id == id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        var entities = await _memberFavoriteRep.Select
            .Where(a => ids.Contains(a.Id) && a.MemberId == User.Id)
            .ToListAsync();

        if (entities.Count != ids.Length)
        {
            throw ResultOutput.Exception(_adminLocalizer["部分收藏记录无权限删除"]);
        }

        await _memberFavoriteRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 检查是否已收藏
    /// </summary>
    /// <param name="favoriteType"></param>
    /// <param name="favoriteId"></param>
    /// <returns></returns>
    public async Task<bool> IsFavoritedAsync(string favoriteType, long favoriteId)
    {
        return await _memberFavoriteRep.Select
            .Where(a => a.MemberId == User.Id && a.FavoriteType == favoriteType && a.FavoriteId == favoriteId)
            .AnyAsync();
    }

    /// <summary>
    /// 一键收藏/取消收藏
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<bool> ToggleFavoriteAsync(MemberFavoriteAddInput input)
    {
        var existing = await _memberFavoriteRep.Select
            .Where(a => a.MemberId == User.Id && a.FavoriteType == input.FavoriteType && a.FavoriteId == input.FavoriteId)
            .FirstAsync();

        if (existing != null)
        {
            // 已收藏，取消收藏
            await _memberFavoriteRep.DeleteAsync(existing.Id);
            return false;
        }
        else
        {
            // 未收藏，添加收藏
            await AddAsync(input);
            return true;
        }
    }

    /// <summary>
    /// 获取收藏对象名称（简单版本，不进行数据库查询）
    /// </summary>
    /// <param name="favoriteType"></param>
    /// <param name="favoriteId"></param>
    /// <returns></returns>
    private string GetSimpleFavoriteName(string favoriteType, long favoriteId)
    {
        return favoriteType switch
        {
            "Supplier" => "供应商",
            "Product" => "产品",
            "Contact" => "联系人",
            _ => "未知对象"
        };
    }


    /// <summary>
    /// 验证收藏对象是否存在
    /// </summary>
    /// <param name="favoriteType"></param>
    /// <param name="favoriteId"></param>
    /// <returns></returns>
    private async Task ValidateFavoriteObjectAsync(string favoriteType, long favoriteId)
    {
        // 这里需要根据实际的供应商、产品、联系人实体来验证存在性
        // 由于这些实体可能在其他模块中，这里使用占位符实现
        var exists = favoriteType switch
        {
            "Supplier" => true, // 假设存在
            "Product" => true,  // 假设存在
            "Contact" => true,  // 假设存在
            _ => false
        };

        if (!exists)
        {
            throw ResultOutput.Exception(_adminLocalizer["收藏对象不存在"]);
        }
    }
}
