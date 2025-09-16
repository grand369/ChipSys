using ChipSys.Admin.Core.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChipSys.Client.Contracts.Services.Client.Dto;

/// <summary>
/// 询价输出
/// </summary>
public class InquiryGetOutput : InquiryAddInput
{
    public long Id { get; set; }
    public int QuoteCount { get; set; }
    public string Status { get; set; }
    public long MemberId { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime? ModifiedTime { get; set; }
}

/// <summary>
/// 询价分页输出
/// </summary>
public class InquiryGetPageOutput : InquiryGetOutput
{
    // 可以添加更多用于列表展示的字段
}

/// <summary>
/// 询价添加输入
/// </summary>
public class InquiryAddInput
{
    [Required(ErrorMessage = "询价标题不能为空")]
    [StringLength(100, ErrorMessage = "询价标题长度不能超过100个字符")]
    public string Title { get; set; }

    [Required(ErrorMessage = "产品名称不能为空")]
    [StringLength(100, ErrorMessage = "产品名称长度不能超过100个字符")]
    public string ProductName { get; set; }

    [StringLength(50, ErrorMessage = "产品型号长度不能超过50个字符")]
    public string? ProductModel { get; set; }

    [StringLength(50, ErrorMessage = "产品品牌长度不能超过50个字符")]
    public string? ProductBrand { get; set; }

    [StringLength(100, ErrorMessage = "产品制造商长度不能超过100个字符")]
    public string? ProductManufacturer { get; set; }

    [Required(ErrorMessage = "需求数量必须大于0")]
    [Range(1, int.MaxValue, ErrorMessage = "需求数量必须大于0")]
    public int Quantity { get; set; }

    [StringLength(100, ErrorMessage = "预算范围长度不能超过100个字符")]
    public string? BudgetRange { get; set; }

    [Required(ErrorMessage = "期望交期天数必须大于0")]
    [Range(1, 365, ErrorMessage = "期望交期天数必须在1-365天之间")]
    public int ExpectedDeliveryDays { get; set; }

    [Required(ErrorMessage = "询价有效期天数必须大于0")]
    [Range(1, 90, ErrorMessage = "询价有效期天数必须在1-90天之间")]
    public int ValidDays { get; set; }

    [StringLength(1000, ErrorMessage = "技术参数要求长度不能超过1000个字符")]
    public string? TechnicalRequirements { get; set; }

    [StringLength(500, ErrorMessage = "质量要求长度不能超过500个字符")]
    public string? QualityRequirements { get; set; }

    [StringLength(500, ErrorMessage = "其他要求长度不能超过500个字符")]
    public string? OtherRequirements { get; set; }

    [Required(ErrorMessage = "联系信息不能为空")]
    [StringLength(500, ErrorMessage = "联系信息长度不能超过500个字符")]
    public string ContactInfo { get; set; }
}

/// <summary>
/// 询价更新输入
/// </summary>
public class InquiryUpdateInput : InquiryAddInput
{
    [Required(ErrorMessage = "ID不能为空")]
    public long Id { get; set; }
}

/// <summary>
/// 询价分页输入
/// </summary>
public class InquiryGetPageInput
{
    public string? Title { get; set; }
    public string? ProductName { get; set; }
    public string? Status { get; set; }
}
