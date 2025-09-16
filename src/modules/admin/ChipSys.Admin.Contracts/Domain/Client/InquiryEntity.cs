using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Client.Contracts.Domain.Client;

/// <summary>
/// 询价实体
/// </summary>
[Table(Name = DbConsts.ChipTableNamePrefix + "client_inquiry", OldName = DbConsts.ChipTableOldNamePrefix + "client_inquiry")]
public partial class InquiryEntity : EntityTenantWithData
{
    /// <summary>
    /// 询价标题
    /// </summary>
    [Column(StringLength = 100)]
    public string Title { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [Column(StringLength = 100)]
    public string ProductName { get; set; }

    /// <summary>
    /// 产品型号
    /// </summary>
    [Column(StringLength = 50)]
    public string? ProductModel { get; set; }

    /// <summary>
    /// 产品品牌
    /// </summary>
    [Column(StringLength = 50)]
    public string? ProductBrand { get; set; }

    /// <summary>
    /// 产品制造商
    /// </summary>
    [Column(StringLength = 100)]
    public string? ProductManufacturer { get; set; }

    /// <summary>
    /// 需求数量
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// 预算范围
    /// </summary>
    [Column(StringLength = 100)]
    public string? BudgetRange { get; set; }

    /// <summary>
    /// 期望交期天数
    /// </summary>
    public int ExpectedDeliveryDays { get; set; }

    /// <summary>
    /// 询价有效期天数
    /// </summary>
    public int ValidDays { get; set; }

    /// <summary>
    /// 技术参数要求
    /// </summary>
    [Column(StringLength = 1000)]
    public string? TechnicalRequirements { get; set; }

    /// <summary>
    /// 质量要求
    /// </summary>
    [Column(StringLength = 500)]
    public string? QualityRequirements { get; set; }

    /// <summary>
    /// 其他要求
    /// </summary>
    [Column(StringLength = 500)]
    public string? OtherRequirements { get; set; }

    /// <summary>
    /// 联系信息
    /// </summary>
    [Column(StringLength = 500)]
    public string ContactInfo { get; set; }

    /// <summary>
    /// 报价数量
    /// </summary>
    public int QuoteCount { get; set; } = 0;

    /// <summary>
    /// 状态：Draft-草稿，Published-已发布，Closed-已关闭，Completed-已完成，Cancelled-已取消
    /// </summary>
    [Column(StringLength = 20)]
    public string Status { get; set; } = "Draft";

    /// <summary>
    /// 会员ID
    /// </summary>
    public long MemberId { get; set; }

    /// <summary>
    /// 报价列表
    /// </summary>
    [NotGen]
    public List<QuoteEntity>? Quotes { get; set; }
}
