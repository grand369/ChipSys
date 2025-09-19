using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Client.Contracts.Domain.Client;

/// <summary>
/// 会员等级价格方案实体
/// </summary>
[Table(Name = DbConsts.ChipTableNamePrefix + "client_member_level_price_plan", OldName = DbConsts.ChipTableOldNamePrefix + "client_member_level_price_plan")]
public partial class MemberLevelPricePlanEntity : EntityTenantWithData
{
    /// <summary>
    /// 会员等级ID
    /// </summary>
    public long MemberLevelId { get; set; }

    /// <summary>
    /// 价格方案类型：Monthly-包月，Quarterly-包季，Yearly-包年
    /// </summary>
    [Column(StringLength = 20)]
    public string PlanType { get; set; } = "Monthly";

    /// <summary>
    /// 方案名称
    /// </summary>
    [Column(StringLength = 50)]
    public string PlanName { get; set; } = "包月";

    /// <summary>
    /// 原价（元）
    /// </summary>
    public decimal OriginalPrice { get; set; } = 0;

    /// <summary>
    /// 折扣后价格（元）
    /// </summary>
    public decimal DiscountedPrice { get; set; } = 0;

    /// <summary>
    /// 折扣百分比（0-100）
    /// </summary>
    public decimal DiscountPercent { get; set; } = 0;

    /// <summary>
    /// 方案描述
    /// </summary>
    [Column(StringLength = 200)]
    public string? Description { get; set; }

    /// <summary>
    /// 是否推荐方案
    /// </summary>
    public bool IsRecommended { get; set; } = false;

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; } = 0;
}
