using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Client.Contracts.Domain.Client;

/// <summary>
/// 供求信息实体
/// </summary>
[Table(Name = DbConsts.ChipTableNamePrefix + "client_supply_demand", OldName = DbConsts.ChipTableOldNamePrefix + "client_supply_demand")]
public partial class SupplyDemandEntity : EntityTenantWithData
{
    /// <summary>
    /// 会员用户Id
    /// </summary>
    public long MemberId { get; set; }

    /// <summary>
    /// 信息类型：Supply-供应，Demand-需求，Inquiry-询价
    /// </summary>
    [Column(StringLength = 20)]
    public string InfoType { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    [Column(StringLength = 200)]
    public string? Title { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [Column(StringLength = 200)]
    public string ProductName { get; set; }

    /// <summary>
    /// 产品型号
    /// </summary>
    [Column(StringLength = 200)]
    public string? ProductModel { get; set; }

    /// <summary>
    /// 产品品牌
    /// </summary>
    [Column(StringLength = 100)]
    public string? ProductBrand { get; set; }

    /// <summary>
    /// 产品制造商
    /// </summary>
    [Column(StringLength = 100)]
    public string? ProductManufacturer { get; set; }

    /// <summary>
    /// 技术参数要求
    /// </summary>
    [Column(StringLength = 2000)]
    public string? TechnicalRequirements { get; set; }

    /// <summary>
    /// 数量要求
    /// </summary>
    public int? Quantity { get; set; }

    /// <summary>
    /// 价格范围
    /// </summary>
    [Column(StringLength = 100)]
    public string? PriceRange { get; set; }

    /// <summary>
    /// 供货周期（天）
    /// </summary>
    public int? DeliveryDays { get; set; }

    /// <summary>
    /// 联系信息
    /// </summary>
    [Column(StringLength = 500)]
    public string? ContactInfo { get; set; }

    /// <summary>
    /// 详细描述
    /// </summary>
    [Column(StringLength = 2000)]
    public string? Description { get; set; }

    /// <summary>
    /// 状态：Draft-草稿，Published-已发布，Closed-已关闭
    /// </summary>
    [Column(StringLength = 20)]
    public string Status { get; set; } = "Draft";

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; } = true;
}
