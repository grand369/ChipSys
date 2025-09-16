using ChipSys.Client.Contracts.Services.Client.Dto;

namespace ChipSys.Client.Contracts.Services.Client;

/// <summary>
/// 会员仪表板服务接口
/// </summary>
public interface IMemberDashboardService
{
    /// <summary>
    /// 获取会员仪表板统计数据
    /// </summary>
    /// <returns></returns>
    Task<MemberDashboardStatsOutput> GetDashboardStatsAsync();

    /// <summary>
    /// 获取会员收藏统计
    /// </summary>
    /// <returns></returns>
    Task<object> GetFavoriteStatsAsync();

    /// <summary>
    /// 获取会员供求信息统计
    /// </summary>
    /// <returns></returns>
    Task<object> GetSupplyDemandStatsAsync();
}
