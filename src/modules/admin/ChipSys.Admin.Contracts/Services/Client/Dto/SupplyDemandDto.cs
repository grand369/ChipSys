using ChipSys.Admin.Core.Dto;
using System.ComponentModel.DataAnnotations;

namespace ChipSys.Client.Contracts.Services.Client.Dto;

/// <summary>
/// 供求信息查询输入
/// </summary>
public class SupplyDemandGetPageInput : PageInput<SupplyDemandGetPageFilter>
{
}

/// <summary>
/// 供求信息查询过滤
/// </summary>
public class SupplyDemandGetPageFilter
{
    /// <summary>
    /// 信息类型
    /// </summary>
    public string? InfoType { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public string? Status { get; set; }
}

/// <summary>
/// 供求信息分页输出
/// </summary>
public class SupplyDemandGetPageOutput
{
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 信息类型
    /// </summary>
    public string InfoType { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// 产品型号
    /// </summary>
    public string? ProductModel { get; set; }

    /// <summary>
    /// 产品品牌
    /// </summary>
    public string? ProductBrand { get; set; }

    /// <summary>
    /// 数量要求
    /// </summary>
    public int? Quantity { get; set; }

    /// <summary>
    /// 价格范围
    /// </summary>
    public string? PriceRange { get; set; }

    /// <summary>
    /// 供货周期（天）
    /// </summary>
    public int? DeliveryDays { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }
}

/// <summary>
/// 供求信息详情输出
/// </summary>
public class SupplyDemandGetOutput
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
    /// 信息类型
    /// </summary>
    public string InfoType { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// 产品型号
    /// </summary>
    public string? ProductModel { get; set; }

    /// <summary>
    /// 产品品牌
    /// </summary>
    public string? ProductBrand { get; set; }

    /// <summary>
    /// 产品制造商
    /// </summary>
    public string? ProductManufacturer { get; set; }

    /// <summary>
    /// 产品描述
    /// </summary>
    public string? ProductDescription { get; set; }

    /// <summary>
    /// 产品参数
    /// </summary>
    public string? ProductSpecification { get; set; }

    /// <summary>
    /// 技术参数要求
    /// </summary>
    public string? TechnicalRequirements { get; set; }

    /// <summary>
    /// 数量要求
    /// </summary>
    public int? Quantity { get; set; }

    /// <summary>
    /// 价格范围
    /// </summary>
    public string? PriceRange { get; set; }

    /// <summary>
    /// 供货周期（天）
    /// </summary>
    public int? DeliveryDays { get; set; }

    /// <summary>
    /// 联系信息
    /// </summary>
    public string? ContactInfo { get; set; }

    /// <summary>
    /// 详细描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }
}

/// <summary>
/// 供求信息添加输入
/// </summary>
public class SupplyDemandAddInput
{
    /// <summary>
    /// 信息类型
    /// </summary>
    [Required(ErrorMessage = "信息类型不能为空")]
    [StringLength(20, ErrorMessage = "信息类型长度不能超过20个字符")]
    public string InfoType { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [Required(ErrorMessage = "产品名称不能为空")]
    [StringLength(200, ErrorMessage = "产品名称长度不能超过200个字符")]
    public string ProductName { get; set; }

    /// <summary>
    /// 产品型号
    /// </summary>
    [StringLength(200, ErrorMessage = "产品型号长度不能超过200个字符")]
    public string? ProductModel { get; set; }

    /// <summary>
    /// 产品品牌
    /// </summary>
    [StringLength(100, ErrorMessage = "产品品牌长度不能超过100个字符")]
    public string? ProductBrand { get; set; }

    /// <summary>
    /// 产品制造商
    /// </summary>
    [StringLength(100, ErrorMessage = "产品制造商长度不能超过100个字符")]
    public string? ProductManufacturer { get; set; }

    /// <summary>
    /// 产品描述
    /// </summary>
    [StringLength(500, ErrorMessage = "产品描述长度不能超过500个字符")]
    public string? ProductDescription { get; set; }

    /// <summary>
    /// 产品参数
    /// </summary>
    [StringLength(1000, ErrorMessage = "产品参数长度不能超过1000个字符")]
    public string? ProductSpecification { get; set; }

    /// <summary>
    /// 技术参数要求
    /// </summary>
    [StringLength(2000, ErrorMessage = "技术参数要求长度不能超过2000个字符")]
    public string? TechnicalRequirements { get; set; }

    /// <summary>
    /// 数量要求
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "数量要求必须大于0")]
    public int? Quantity { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    [StringLength(20, ErrorMessage = "单位长度不能超过20个字符")]
    public string? Unit { get; set; }

    /// <summary>
    /// 期望价格
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "期望价格必须大于0")]
    public decimal? ExpectedPrice { get; set; }

    /// <summary>
    /// 货币类型
    /// </summary>
    [StringLength(10, ErrorMessage = "货币类型长度不能超过10个字符")]
    public string? Currency { get; set; }

    /// <summary>
    /// 价格范围
    /// </summary>
    [StringLength(100, ErrorMessage = "价格范围长度不能超过100个字符")]
    public string? PriceRange { get; set; }

    /// <summary>
    /// 供货周期（天）
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "供货周期必须大于0")]
    public int? DeliveryDays { get; set; }

    /// <summary>
    /// 联系信息
    /// </summary>
    [StringLength(500, ErrorMessage = "联系信息长度不能超过500个字符")]
    public string? ContactInfo { get; set; }

    /// <summary>
    /// 详细描述
    /// </summary>
    [StringLength(2000, ErrorMessage = "详细描述长度不能超过2000个字符")]
    public string? Description { get; set; }
}

/// <summary>
/// 供求信息修改输入
/// </summary>
public class SupplyDemandUpdateInput
{
    /// <summary>
    /// 主键
    /// </summary>
    [Required(ErrorMessage = "请选择要修改的供求信息")]
    public long Id { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [Required(ErrorMessage = "产品名称不能为空")]
    [StringLength(200, ErrorMessage = "产品名称长度不能超过200个字符")]
    public string ProductName { get; set; }

    /// <summary>
    /// 产品型号
    /// </summary>
    [StringLength(200, ErrorMessage = "产品型号长度不能超过200个字符")]
    public string? ProductModel { get; set; }

    /// <summary>
    /// 产品品牌
    /// </summary>
    [StringLength(100, ErrorMessage = "产品品牌长度不能超过100个字符")]
    public string? ProductBrand { get; set; }

    /// <summary>
    /// 产品制造商
    /// </summary>
    [StringLength(100, ErrorMessage = "产品制造商长度不能超过100个字符")]
    public string? ProductManufacturer { get; set; }

    /// <summary>
    /// 产品描述
    /// </summary>
    [StringLength(500, ErrorMessage = "产品描述长度不能超过500个字符")]
    public string? ProductDescription { get; set; }

    /// <summary>
    /// 产品参数
    /// </summary>
    [StringLength(1000, ErrorMessage = "产品参数长度不能超过1000个字符")]
    public string? ProductSpecification { get; set; }

    /// <summary>
    /// 技术参数要求
    /// </summary>
    [StringLength(2000, ErrorMessage = "技术参数要求长度不能超过2000个字符")]
    public string? TechnicalRequirements { get; set; }

    /// <summary>
    /// 数量要求
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "数量要求必须大于0")]
    public int? Quantity { get; set; }

    /// <summary>
    /// 价格范围
    /// </summary>
    [StringLength(100, ErrorMessage = "价格范围长度不能超过100个字符")]
    public string? PriceRange { get; set; }

    /// <summary>
    /// 供货周期（天）
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "供货周期必须大于0")]
    public int? DeliveryDays { get; set; }

    /// <summary>
    /// 联系信息
    /// </summary>
    [StringLength(500, ErrorMessage = "联系信息长度不能超过500个字符")]
    public string? ContactInfo { get; set; }

    /// <summary>
    /// 详细描述
    /// </summary>
    [StringLength(2000, ErrorMessage = "详细描述长度不能超过2000个字符")]
    public string? Description { get; set; }
}
