using ChipSys.Admin.Core.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChipSys.Client.Contracts.Services.Client.Dto;

/// <summary>
/// 报价输出
/// </summary>
public class QuoteGetOutput : QuoteAddInput
{
    public long Id { get; set; }
    public DateTime? ValidUntil { get; set; }
    public string Status { get; set; }
    public DateTime? AcceptedTime { get; set; }
    public DateTime? RejectedTime { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime? ModifiedTime { get; set; }
}

/// <summary>
/// 报价分页输出
/// </summary>
public class QuoteGetPageOutput : QuoteGetOutput
{
    // 可以添加更多用于列表展示的字段
}

/// <summary>
/// 报价添加输入
/// </summary>
public class QuoteAddInput
{
    [Required(ErrorMessage = "询价ID必须大于0")]
    [Range(1, long.MaxValue, ErrorMessage = "询价ID必须大于0")]
    public long InquiryId { get; set; }

    [Required(ErrorMessage = "供应商ID必须大于0")]
    [Range(1, long.MaxValue, ErrorMessage = "供应商ID必须大于0")]
    public long SupplierId { get; set; }

    [Required(ErrorMessage = "供应商名称不能为空")]
    [StringLength(100, ErrorMessage = "供应商名称长度不能超过100个字符")]
    public string SupplierName { get; set; }

    [Required(ErrorMessage = "报价必须大于0")]
    [Range(0.01, double.MaxValue, ErrorMessage = "报价必须大于0")]
    public decimal QuotePrice { get; set; }

    [Required(ErrorMessage = "货币不能为空")]
    [StringLength(10, ErrorMessage = "货币长度不能超过10个字符")]
    public string Currency { get; set; } = "CNY";

    [Required(ErrorMessage = "交期天数必须大于0")]
    [Range(1, 365, ErrorMessage = "交期天数必须在1-365天之间")]
    public int DeliveryDays { get; set; }

    [Required(ErrorMessage = "最小起订量必须大于0")]
    [Range(1, int.MaxValue, ErrorMessage = "最小起订量必须大于0")]
    public int MinOrderQuantity { get; set; }

    [Required(ErrorMessage = "有效期天数必须大于0")]
    [Range(1, 90, ErrorMessage = "有效期天数必须在1-90天之间")]
    public int ValidDays { get; set; }

    [StringLength(1000, ErrorMessage = "产品规格说明长度不能超过1000个字符")]
    public string? ProductSpecification { get; set; }

    [StringLength(500, ErrorMessage = "质量保证说明长度不能超过500个字符")]
    public string? QualityAssurance { get; set; }

    [StringLength(1000, ErrorMessage = "报价备注长度不能超过1000个字符")]
    public string? QuoteRemark { get; set; }

    [Required(ErrorMessage = "联系人姓名不能为空")]
    [StringLength(50, ErrorMessage = "联系人姓名长度不能超过50个字符")]
    public string ContactName { get; set; }

    [Required(ErrorMessage = "联系电话不能为空")]
    [StringLength(20, ErrorMessage = "联系电话长度不能超过20个字符")]
    public string ContactPhone { get; set; }

    [EmailAddress(ErrorMessage = "请输入正确的邮箱地址")]
    [StringLength(100, ErrorMessage = "联系邮箱长度不能超过100个字符")]
    public string? ContactEmail { get; set; }
}

/// <summary>
/// 报价更新输入
/// </summary>
public class QuoteUpdateInput : QuoteAddInput
{
    [Required(ErrorMessage = "ID不能为空")]
    public long Id { get; set; }

    [Required(ErrorMessage = "状态不能为空")]
    public string Status { get; set; }
}

/// <summary>
/// 报价分页输入
/// </summary>
public class QuoteGetPageInput : PageInput
{
    public long? InquiryId { get; set; }
    public string? SupplierName { get; set; }
    public string? Status { get; set; }
}
