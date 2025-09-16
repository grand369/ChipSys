using ChipSys.Admin.Core.Dto;
using System.ComponentModel.DataAnnotations;

namespace ChipSys.Client.Contracts.Services.Client.Dto;

/// <summary>
/// 供应商申请查询输入
/// </summary>
public class SupplierApplicationGetPageInput : PageInput<SupplierApplicationGetPageFilter>
{
}

/// <summary>
/// 供应商申请查询过滤
/// </summary>
public class SupplierApplicationGetPageFilter
{
    /// <summary>
    /// 公司名称
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    public string? ContactName { get; set; }

    /// <summary>
    /// 审核状态
    /// </summary>
    public string? Status { get; set; }
}

/// <summary>
/// 供应商申请分页输出
/// </summary>
public class SupplierApplicationGetPageOutput
{
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 公司名称
    /// </summary>
    public string CompanyName { get; set; }

    /// <summary>
    /// 公司地址
    /// </summary>
    public string? CompanyAddress { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    public string ContactName { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string ContactPhone { get; set; }

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 主要产品类别
    /// </summary>
    public string? MainProductCategories { get; set; }

    /// <summary>
    /// 审核状态
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 审核意见
    /// </summary>
    public string? ReviewComment { get; set; }

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
/// 供应商申请详情输出
/// </summary>
public class SupplierApplicationGetOutput
{
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 会员用户Id
    /// </summary>
    public long MemberId { get; set; }

    /// <summary>
    /// 公司名称
    /// </summary>
    public string CompanyName { get; set; }

    /// <summary>
    /// 公司地址
    /// </summary>
    public string? CompanyAddress { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    public string ContactName { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string ContactPhone { get; set; }

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 营业执照号
    /// </summary>
    public string? BusinessLicense { get; set; }

    /// <summary>
    /// 主要产品类别
    /// </summary>
    public string? MainProductCategories { get; set; }

    /// <summary>
    /// 公司简介
    /// </summary>
    public string? CompanyDescription { get; set; }

    /// <summary>
    /// 申请理由
    /// </summary>
    public string? ApplicationReason { get; set; }

    /// <summary>
    /// 审核状态
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 审核意见
    /// </summary>
    public string? ReviewComment { get; set; }

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
/// 供应商申请添加输入
/// </summary>
public class SupplierApplicationAddInput
{
    /// <summary>
    /// 公司名称
    /// </summary>
    [Required(ErrorMessage = "公司名称不能为空")]
    [StringLength(200, ErrorMessage = "公司名称长度不能超过200个字符")]
    public string CompanyName { get; set; }

    /// <summary>
    /// 公司地址
    /// </summary>
    [StringLength(500, ErrorMessage = "公司地址长度不能超过500个字符")]
    public string? CompanyAddress { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    [Required(ErrorMessage = "联系人姓名不能为空")]
    [StringLength(100, ErrorMessage = "联系人姓名长度不能超过100个字符")]
    public string ContactName { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [Required(ErrorMessage = "联系电话不能为空")]
    [StringLength(50, ErrorMessage = "联系电话长度不能超过50个字符")]
    public string ContactPhone { get; set; }

    /// <summary>
    /// 联系邮箱
    /// </summary>
    [EmailAddress(ErrorMessage = "联系邮箱格式不正确")]
    [StringLength(100, ErrorMessage = "联系邮箱长度不能超过100个字符")]
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 营业执照号
    /// </summary>
    [StringLength(100, ErrorMessage = "营业执照号长度不能超过100个字符")]
    public string? BusinessLicense { get; set; }

    /// <summary>
    /// 主要产品类别
    /// </summary>
    [StringLength(500, ErrorMessage = "主要产品类别长度不能超过500个字符")]
    public string? MainProductCategories { get; set; }

    /// <summary>
    /// 经营范围
    /// </summary>
    [StringLength(1000, ErrorMessage = "经营范围长度不能超过1000个字符")]
    public string? BusinessScope { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    [StringLength(500, ErrorMessage = "地址长度不能超过500个字符")]
    public string? Address { get; set; }

    /// <summary>
    /// 公司简介
    /// </summary>
    [StringLength(2000, ErrorMessage = "公司简介长度不能超过2000个字符")]
    public string? CompanyDescription { get; set; }

    /// <summary>
    /// 申请理由
    /// </summary>
    [StringLength(1000, ErrorMessage = "申请理由长度不能超过1000个字符")]
    public string? ApplicationReason { get; set; }
}

/// <summary>
/// 供应商申请修改输入
/// </summary>
public class SupplierApplicationUpdateInput
{
    /// <summary>
    /// 主键
    /// </summary>
    [Required(ErrorMessage = "请选择要修改的供应商申请")]
    public long Id { get; set; }

    /// <summary>
    /// 公司名称
    /// </summary>
    [Required(ErrorMessage = "公司名称不能为空")]
    [StringLength(200, ErrorMessage = "公司名称长度不能超过200个字符")]
    public string CompanyName { get; set; }

    /// <summary>
    /// 公司地址
    /// </summary>
    [StringLength(500, ErrorMessage = "公司地址长度不能超过500个字符")]
    public string? CompanyAddress { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    [Required(ErrorMessage = "联系人姓名不能为空")]
    [StringLength(100, ErrorMessage = "联系人姓名长度不能超过100个字符")]
    public string ContactName { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [Required(ErrorMessage = "联系电话不能为空")]
    [StringLength(50, ErrorMessage = "联系电话长度不能超过50个字符")]
    public string ContactPhone { get; set; }

    /// <summary>
    /// 联系邮箱
    /// </summary>
    [EmailAddress(ErrorMessage = "联系邮箱格式不正确")]
    [StringLength(100, ErrorMessage = "联系邮箱长度不能超过100个字符")]
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 营业执照号
    /// </summary>
    [StringLength(100, ErrorMessage = "营业执照号长度不能超过100个字符")]
    public string? BusinessLicense { get; set; }

    /// <summary>
    /// 主要产品类别
    /// </summary>
    [StringLength(500, ErrorMessage = "主要产品类别长度不能超过500个字符")]
    public string? MainProductCategories { get; set; }

    /// <summary>
    /// 公司简介
    /// </summary>
    [StringLength(2000, ErrorMessage = "公司简介长度不能超过2000个字符")]
    public string? CompanyDescription { get; set; }

    /// <summary>
    /// 申请理由
    /// </summary>
    [StringLength(1000, ErrorMessage = "申请理由长度不能超过1000个字符")]
    public string? ApplicationReason { get; set; }
}
