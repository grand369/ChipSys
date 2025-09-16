namespace ChipSys.Client.Contracts.Services.Client.Dto;

/// <summary>
/// 会员仪表板统计输出
/// </summary>
public class MemberDashboardStatsOutput
{
    /// <summary>
    /// 收藏的供应商数量
    /// </summary>
    public int FavoriteSupplierCount { get; set; }

    /// <summary>
    /// 收藏的产品数量
    /// </summary>
    public int FavoriteProductCount { get; set; }

    /// <summary>
    /// 收藏的联系人数量
    /// </summary>
    public int FavoriteContactCount { get; set; }

    /// <summary>
    /// 发布的供应信息数量
    /// </summary>
    public int SupplyInfoCount { get; set; }

    /// <summary>
    /// 发布的需求信息数量
    /// </summary>
    public int DemandInfoCount { get; set; }

    /// <summary>
    /// 供应商申请状态
    /// </summary>
    public string? SupplierApplicationStatus { get; set; }

    /// <summary>
    /// 最近收藏的供应商
    /// </summary>
    public List<RecentFavoriteOutput> RecentFavoriteSuppliers { get; set; } = new();

    /// <summary>
    /// 最近收藏的产品
    /// </summary>
    public List<RecentFavoriteOutput> RecentFavoriteProducts { get; set; } = new();

    /// <summary>
    /// 最近的供求信息
    /// </summary>
    public List<RecentSupplyDemandOutput> RecentSupplyDemands { get; set; } = new();
}

/// <summary>
/// 最近收藏输出
/// </summary>
public class RecentFavoriteOutput
{
    /// <summary>
    /// 收藏Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 对象Id
    /// </summary>
    public long FavoriteId { get; set; }

    /// <summary>
    /// 对象名称
    /// </summary>
    public string FavoriteName { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 收藏时间
    /// </summary>
    public DateTime CreatedTime { get; set; }
}

/// <summary>
/// 最近供求信息输出
/// </summary>
public class RecentSupplyDemandOutput
{
    /// <summary>
    /// 信息Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 信息类型
    /// </summary>
    public string InfoType { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }
}
