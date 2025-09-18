using System;
using System.ComponentModel.DataAnnotations;

namespace ChipSys.Client.Contracts.Services.Client.Dto;

/// <summary>
/// 供应商申请输入
/// </summary>
public class SupplierApplicationAddInput
{
    /// <summary>
    /// 公司名称
    /// </summary>
    [Required(ErrorMessage = "公司名称不能为空")]
    [StringLength(100, ErrorMessage = "公司名称长度不能超过100个字符")]
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 公司简称
    /// </summary>
    [StringLength(50, ErrorMessage = "公司简称长度不能超过50个字符")]
    public string? ShortName { get; set; }

    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    [StringLength(50, ErrorMessage = "统一社会信用代码长度不能超过50个字符")]
    public string? CreditCode { get; set; }

    /// <summary>
    /// 营业执照号
    /// </summary>
    [StringLength(50, ErrorMessage = "营业执照号长度不能超过50个字符")]
    public string? BusinessLicense { get; set; }

    /// <summary>
    /// 法定代表人
    /// </summary>
    [StringLength(50, ErrorMessage = "法定代表人长度不能超过50个字符")]
    public string? LegalPerson { get; set; }

    /// <summary>
    /// 注册资本
    /// </summary>
    [StringLength(50, ErrorMessage = "注册资本长度不能超过50个字符")]
    public string? RegisteredCapital { get; set; }

    /// <summary>
    /// 成立日期
    /// </summary>
    public DateTime? EstablishedDate { get; set; }

    /// <summary>
    /// 经营范围
    /// </summary>
    [StringLength(500, ErrorMessage = "经营范围长度不能超过500个字符")]
    public string? BusinessScope { get; set; }

    /// <summary>
    /// 公司地址
    /// </summary>
    [StringLength(200, ErrorMessage = "公司地址长度不能超过200个字符")]
    public string? Address { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    [Required(ErrorMessage = "联系人姓名不能为空")]
    [StringLength(50, ErrorMessage = "联系人姓名长度不能超过50个字符")]
    public string ContactName { get; set; } = string.Empty;

    /// <summary>
    /// 联系人电话
    /// </summary>
    [Required(ErrorMessage = "联系人电话不能为空")]
    [StringLength(20, ErrorMessage = "联系人电话长度不能超过20个字符")]
    public string ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    [StringLength(100, ErrorMessage = "联系人邮箱长度不能超过100个字符")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 申请材料（文件路径列表）
    /// </summary>
    public string[]? Materials { get; set; }
}

/// <summary>
/// 供应商申请输出
/// </summary>
public class SupplierApplicationGetOutput : SupplierApplicationAddInput
{
    /// <summary>
    /// 申请ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 会员ID
    /// </summary>
    public long MemberId { get; set; }

    /// <summary>
    /// 申请状态
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// 审核意见
    /// </summary>
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
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }
}

/// <summary>
/// 供应商申请分页输入
/// </summary>
public class SupplierApplicationGetPageInput
{
    /// <summary>
    /// 会员ID
    /// </summary>
    public long? MemberId { get; set; }

    /// <summary>
    /// 申请状态
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// 公司名称
    /// </summary>
    public string? CompanyName { get; set; }
}

/// <summary>
/// 供应商申请审核输入
/// </summary>
public class SupplierApplicationReviewInput
{
    /// <summary>
    /// 申请ID
    /// </summary>
    [Required(ErrorMessage = "申请ID不能为空")]
    public long Id { get; set; }

    /// <summary>
    /// 审核结果：approved, rejected
    /// </summary>
    [Required(ErrorMessage = "审核结果不能为空")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// 审核意见
    /// </summary>
    [StringLength(500, ErrorMessage = "审核意见长度不能超过500个字符")]
    public string? ReviewComment { get; set; }
}