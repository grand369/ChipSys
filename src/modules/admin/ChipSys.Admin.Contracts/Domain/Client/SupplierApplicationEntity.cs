using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Client.Contracts.Domain.Client;

/// <summary>
/// 供应商申请实体
/// </summary>
[Table(Name = DbConsts.ChipTableNamePrefix + "client_supplier_application", OldName = DbConsts.ChipTableOldNamePrefix + "client_supplier_application")]
public partial class SupplierApplicationEntity : EntityTenantWithData
{
    /// <summary>
    /// 会员用户Id
    /// </summary>
    public long MemberId { get; set; }

    /// <summary>
    /// 公司名称
    /// </summary>
    [Column(StringLength = 200)]
    public string CompanyName { get; set; }

    /// <summary>
    /// 公司地址
    /// </summary>
    [Column(StringLength = 500)]
    public string? CompanyAddress { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    [Column(StringLength = 100)]
    public string ContactName { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [Column(StringLength = 50)]
    public string ContactPhone { get; set; }

    /// <summary>
    /// 联系邮箱
    /// </summary>
    [Column(StringLength = 100)]
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 营业执照号
    /// </summary>
    [Column(StringLength = 100)]
    public string? BusinessLicense { get; set; }

    /// <summary>
    /// 主要产品类别
    /// </summary>
    [Column(StringLength = 500)]
    public string? MainProductCategories { get; set; }

    /// <summary>
    /// 公司简介
    /// </summary>
    [Column(StringLength = 2000)]
    public string? CompanyDescription { get; set; }

    /// <summary>
    /// 申请理由
    /// </summary>
    [Column(StringLength = 1000)]
    public string? ApplicationReason { get; set; }

    /// <summary>
    /// 审核状态：Pending-待审核，Approved-已通过，Rejected-已拒绝
    /// </summary>
    [Column(StringLength = 20)]
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// 审核意见
    /// </summary>
    [Column(StringLength = 1000)]
    public string? ReviewComment { get; set; }

    /// <summary>
    /// 审核人Id
    /// </summary>
    public long? ReviewerId { get; set; }

    /// <summary>
    /// 审核时间
    /// </summary>
    public DateTime? ReviewTime { get; set; }

    /// <summary>
    /// 提交时间
    /// </summary>
    public DateTime? SubmittedTime { get; set; }

    /// <summary>
    /// 取消时间
    /// </summary>
    public DateTime? CancelledTime { get; set; }
}
