using System;
using System.ComponentModel.DataAnnotations;
using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Client.Contracts.Domain.Client;

/// <summary>
/// 供应商申请实体
/// </summary>
[Table(Name = "supplier_applications")]
public class SupplierApplicationEntity : EntityTenantWithData
{
    /// <summary>
    /// 会员ID
    /// </summary>
    public long MemberId { get; set; }

    /// <summary>
    /// 公司名称
    /// </summary>
    [Column(StringLength = 100)]
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 公司简称
    /// </summary>
    [Column(StringLength = 50)]
    public string? ShortName { get; set; }

    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    [Column(StringLength = 50)]
    public string? CreditCode { get; set; }

    /// <summary>
    /// 营业执照号
    /// </summary>
    [Column(StringLength = 50)]
    public string? BusinessLicense { get; set; }

    /// <summary>
    /// 法定代表人
    /// </summary>
    [Column(StringLength = 50)]
    public string? LegalPerson { get; set; }

    /// <summary>
    /// 注册资本
    /// </summary>
    [Column(StringLength = 50)]
    public string? RegisteredCapital { get; set; }

    /// <summary>
    /// 成立日期
    /// </summary>
    public DateTime? EstablishedDate { get; set; }

    /// <summary>
    /// 经营范围
    /// </summary>
    [Column(StringLength = 500)]
    public string? BusinessScope { get; set; }

    /// <summary>
    /// 公司地址
    /// </summary>
    [Column(StringLength = 200)]
    public string? Address { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    [Column(StringLength = 50)]
    public string ContactName { get; set; } = string.Empty;

    /// <summary>
    /// 联系人电话
    /// </summary>
    [Column(StringLength = 20)]
    public string ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    [Column(StringLength = 100)]
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 申请状态：pending, approved, rejected
    /// </summary>
    [Column(StringLength = 20)]
    public string Status { get; set; } = "pending";

    /// <summary>
    /// 审核意见
    /// </summary>
    [Column(StringLength = 500)]
    public string? ReviewComment { get; set; }

    /// <summary>
    /// 审核人ID
    /// </summary>
    public long? ReviewerId { get; set; }

    /// <summary>
    /// 审核时间
    /// </summary>
    public DateTime? ReviewTime { get; set; }

    /// <summary>
    /// 申请材料（JSON格式存储文件路径）
    /// </summary>
    [Column(StringLength = 1000)]
    public string? Materials { get; set; }
}