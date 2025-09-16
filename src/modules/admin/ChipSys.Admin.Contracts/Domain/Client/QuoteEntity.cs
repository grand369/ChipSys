using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Client.Contracts.Domain.Client;

/// <summary>
/// 报价实体
/// </summary>
[Table(Name = DbConsts.ChipTableNamePrefix + "client_quote", OldName = DbConsts.ChipTableOldNamePrefix + "client_quote")]
public partial class QuoteEntity : EntityTenantWithData
{
    /// <summary>
    /// 询价ID
    /// </summary>
    public long InquiryId { get; set; }

    /// <summary>
    /// 供应商ID
    /// </summary>
    public long SupplierId { get; set; }

    /// <summary>
    /// 供应商名称
    /// </summary>
    [Column(StringLength = 100)]
    public string SupplierName { get; set; }

    /// <summary>
    /// 报价金额
    /// </summary>
    [Column(Precision = 18, Scale = 2)]
    public decimal QuotePrice { get; set; }

    /// <summary>
    /// 货币类型
    /// </summary>
    [Column(StringLength = 10)]
    public string Currency { get; set; } = "CNY";

    /// <summary>
    /// 交期天数
    /// </summary>
    public int DeliveryDays { get; set; }

    /// <summary>
    /// 最小起订量
    /// </summary>
    public int MinOrderQuantity { get; set; }

    /// <summary>
    /// 有效期天数
    /// </summary>
    public int ValidDays { get; set; }

    /// <summary>
    /// 有效期至
    /// </summary>
    public DateTime? ValidUntil { get; set; }

    /// <summary>
    /// 产品规格说明
    /// </summary>
    [Column(StringLength = 1000)]
    public string? ProductSpecification { get; set; }

    /// <summary>
    /// 质量保证
    /// </summary>
    [Column(StringLength = 500)]
    public string? QualityAssurance { get; set; }

    /// <summary>
    /// 报价备注
    /// </summary>
    [Column(StringLength = 1000)]
    public string? QuoteRemark { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    [Column(StringLength = 50)]
    public string ContactName { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [Column(StringLength = 20)]
    public string ContactPhone { get; set; }

    /// <summary>
    /// 联系邮箱
    /// </summary>
    [Column(StringLength = 100)]
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 状态：Pending-待审核，Approved-已通过，Accepted-已接受，Rejected-已拒绝，Cancelled-已取消
    /// </summary>
    [Column(StringLength = 20)]
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// 接受时间
    /// </summary>
    public DateTime? AcceptedTime { get; set; }

    /// <summary>
    /// 拒绝时间
    /// </summary>
    public DateTime? RejectedTime { get; set; }

    /// <summary>
    /// 询价信息
    /// </summary>
    [NotGen]
    public InquiryEntity? Inquiry { get; set; }
}
