using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.Product.Dto;

/// <summary>
/// 产品供应商关系修改
/// </summary>
public class ProductSupplierUpdateInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "请选择要修改的产品供应商关系")]
    public long Id { get; set; }

    /// <summary>
    /// 产品Id
    /// </summary>
    [Required(ErrorMessage = "请选择产品")]
    public long ProductId { get; set; }

    /// <summary>
    /// 供应商Id
    /// </summary>
    [Required(ErrorMessage = "请选择供应商")]
    public long SupplierId { get; set; }

    /// <summary>
    /// 之前价格
    /// </summary>
    public decimal? PreviousPrice { get; set; }

    /// <summary>
    /// 当前价格
    /// </summary>
    [Required(ErrorMessage = "请输入当前价格")]
    public decimal CurrentPrice { get; set; }

    /// <summary>
    /// 货币
    /// </summary>
    [StringLength(10, ErrorMessage = "货币长度不能超过10个字符")]
    public string Currency { get; set; } = "CNY";

    /// <summary>
    /// 成色
    /// </summary>
    [StringLength(20, ErrorMessage = "成色长度不能超过20个字符")]
    public string Condition { get; set; }

    /// <summary>
    /// 使用描述
    /// </summary>
    [StringLength(200, ErrorMessage = "使用描述长度不能超过200个字符")]
    public string UsageDescription { get; set; }

    /// <summary>
    /// 供应商型号
    /// </summary>
    [StringLength(200, ErrorMessage = "供应商型号长度不能超过200个字符")]
    public string SupplierModel { get; set; }

    /// <summary>
    /// 最小起订量
    /// </summary>
    public int MOQ { get; set; }

    /// <summary>
    /// 交期天数
    /// </summary>
    public int? LeadTimeDays { get; set; }

    /// <summary>
    /// 库存数量
    /// </summary>
    public int StockQty { get; set; }

    /// <summary>
    /// 有效开始
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// 有效截止
    /// </summary>
    public DateTime? ValidTo { get; set; }
}