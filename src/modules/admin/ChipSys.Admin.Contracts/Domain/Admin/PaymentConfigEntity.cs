using System;
using System.ComponentModel.DataAnnotations;
using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Admin.Domain.Admin;

/// <summary>
/// 支付配置实体
/// </summary>
[Table(Name = "payment_configs")]
public class PaymentConfigEntity : EntityTenantWithData
{
    /// <summary>
    /// 支付方式：alipay, wechat
    /// </summary>
    [Column(StringLength = 20)]
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// 配置名称
    /// </summary>
    [Column(StringLength = 50)]
    public string ConfigName { get; set; } = string.Empty;

    /// <summary>
    /// 应用ID
    /// </summary>
    [Column(StringLength = 100)]
    public string AppId { get; set; } = string.Empty;

    /// <summary>
    /// 商户号
    /// </summary>
    [Column(StringLength = 100)]
    public string MerchantId { get; set; } = string.Empty;

    /// <summary>
    /// 私钥
    /// </summary>
    [Column(StringLength = 2000)]
    public string PrivateKey { get; set; } = string.Empty;

    /// <summary>
    /// 公钥
    /// </summary>
    [Column(StringLength = 2000)]
    public string PublicKey { get; set; } = string.Empty;

    /// <summary>
    /// 支付宝公钥（仅支付宝使用）
    /// </summary>
    [Column(StringLength = 2000)]
    public string AlipayPublicKey { get; set; } = string.Empty;

    /// <summary>
    /// 微信支付密钥（仅微信支付使用）
    /// </summary>
    [Column(StringLength = 100)]
    public string WechatSecret { get; set; } = string.Empty;

    /// <summary>
    /// 回调地址
    /// </summary>
    [Column(StringLength = 200)]
    public string NotifyUrl { get; set; } = string.Empty;

    /// <summary>
    /// 返回地址
    /// </summary>
    [Column(StringLength = 200)]
    public string ReturnUrl { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 环境：sandbox, production
    /// </summary>
    [Column(StringLength = 20)]
    public string Environment { get; set; } = "sandbox";

    /// <summary>
    /// 备注
    /// </summary>
    [Column(StringLength = 500)]
    public string? Remark { get; set; }
}
