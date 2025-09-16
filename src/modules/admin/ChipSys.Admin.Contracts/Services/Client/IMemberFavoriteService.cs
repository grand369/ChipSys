using ChipSys.Admin.Core.Dto;
using ChipSys.Client.Contracts.Services.Client.Dto;

namespace ChipSys.Client.Contracts.Services.Client;

/// <summary>
/// 会员收藏服务接口
/// </summary>
public interface IMemberFavoriteService
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<MemberFavoriteGetOutput> GetAsync(long id);

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageOutput<MemberFavoriteGetPageOutput>> GetPageAsync(MemberFavoriteGetPageInput input);

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<long> AddAsync(MemberFavoriteAddInput input);

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateAsync(MemberFavoriteUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(long id);

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task BatchDeleteAsync(long[] ids);

    /// <summary>
    /// 检查是否已收藏
    /// </summary>
    /// <param name="favoriteType"></param>
    /// <param name="favoriteId"></param>
    /// <returns></returns>
    Task<bool> IsFavoritedAsync(string favoriteType, long favoriteId);

    /// <summary>
    /// 一键收藏
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> ToggleFavoriteAsync(MemberFavoriteAddInput input);
}
