using System;
using System.ComponentModel.DataAnnotations;
using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Client.Contracts.Domain.Client;

/// <summary>
/// 支付订单实体
/// </summary>
[Table(Name = "payment_orders")]
public class PaymentOrderEntity : EntityTenantWithData
{
    /// <summary>
    /// 订单号
    /// </summary>
    [Column(StringLength = 50)]
    public string OrderId { get; set; } = string.Empty;

    /// <summary>
    /// 会员ID
    /// </summary>
    public long MemberId { get; set; }

    /// <summary>
    /// 会员等级
    /// </summary>
    [Column(StringLength = 20)]
    public string Level { get; set; } = string.Empty;

    /// <summary>
    /// 支付金额（分）
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// 支付方式
    /// </summary>
    [Column(StringLength = 20)]
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// 订单描述
    /// </summary>
    [Column(StringLength = 200)]
    public string? Description { get; set; }

    /// <summary>
    /// 支付状态：pending, paid, failed, expired
    /// </summary>
    [Column(StringLength = 20)]
    public string Status { get; set; } = "pending";

    /// <summary>
    /// 第三方交易号
    /// </summary>
    [Column(StringLength = 100)]
    public string? TransactionId { get; set; }

    /// <summary>
    /// 支付时间
    /// </summary>
    public DateTime? PaidTime { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime ExpireTime { get; set; }
}
