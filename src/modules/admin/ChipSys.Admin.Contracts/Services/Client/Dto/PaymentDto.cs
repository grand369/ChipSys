using System;
using System.ComponentModel.DataAnnotations;

namespace ChipSys.Client.Contracts.Services.Client.Dto;

/// <summary>
/// 支付创建输入
/// </summary>
public class PaymentCreateInput
{
    /// <summary>
    /// 会员等级
    /// </summary>
    [Required(ErrorMessage = "会员等级不能为空")]
    public string Level { get; set; } = string.Empty;
    
    /// <summary>
    /// 支付金额（分）
    /// </summary>
    [Required(ErrorMessage = "支付金额不能为空")]
    [Range(1, int.MaxValue, ErrorMessage = "支付金额必须大于0")]
    public int Amount { get; set; }
    
    /// <summary>
    /// 支付方式：alipay, wechat
    /// </summary>
    [Required(ErrorMessage = "支付方式不能为空")]
    public string PaymentMethod { get; set; } = string.Empty;
    
    /// <summary>
    /// 订单描述
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// 支付创建输出
/// </summary>
public class PaymentCreateOutput
{
    /// <summary>
    /// 订单ID
    /// </summary>
    public string OrderId { get; set; } = string.Empty;
    
    /// <summary>
    /// 支付二维码
    /// </summary>
    public string? QrCode { get; set; }
    
    /// <summary>
    /// 支付链接
    /// </summary>
    public string? PayUrl { get; set; }
    
    /// <summary>
    /// 订单状态
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime ExpireTime { get; set; }
}

/// <summary>
/// 支付状态输出
/// </summary>
public class PaymentStatusOutput
{
    /// <summary>
    /// 订单ID
    /// </summary>
    public string OrderId { get; set; } = string.Empty;
    
    /// <summary>
    /// 支付状态：pending, paid, failed, expired
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// 支付时间
    /// </summary>
    public DateTime? PaidTime { get; set; }
    
    /// <summary>
    /// 支付金额
    /// </summary>
    public int Amount { get; set; }
    
    /// <summary>
    /// 第三方交易号
    /// </summary>
    public string? TransactionId { get; set; }
}

/// <summary>
/// 支付回调输入
/// </summary>
public class PaymentCallbackInput
{
    /// <summary>
    /// 订单ID
    /// </summary>
    public string OrderId { get; set; } = string.Empty;
    
    /// <summary>
    /// 支付状态
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// 第三方交易号
    /// </summary>
    public string? TransactionId { get; set; }
    
    /// <summary>
    /// 支付时间
    /// </summary>
    public DateTime? PaidTime { get; set; }
    
    /// <summary>
    /// 签名
    /// </summary>
    public string? Signature { get; set; }
}
