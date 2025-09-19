using ChipSys.Admin.Core.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChipSys.Client.Contracts.Services.Client.Dto;

/// <summary>
/// 会员等级价格方案输出
/// </summary>
public class MemberLevelPricePlanGetOutput : MemberLevelPricePlanAddInput
{
    public long Id { get; set; }
    public DateTime CreatedTime { get; set; }
}

/// <summary>
/// 会员等级价格方案分页输出
/// </summary>
public class MemberLevelPricePlanGetPageOutput : MemberLevelPricePlanGetOutput
{
    // 可以添加更多用于列表展示的字段
}

/// <summary>
/// 会员等级价格方案添加输入
/// </summary>
public class MemberLevelPricePlanAddInput
{
    /// <summary>
    /// 会员等级ID
    /// </summary>
    [Required(ErrorMessage = "会员等级ID不能为空")]
    public long MemberLevelId { get; set; }
    
    /// <summary>
    /// 价格方案类型：Monthly-包月，Quarterly-包季，Yearly-包年
    /// </summary>
    [Required(ErrorMessage = "价格方案类型不能为空")]
    [StringLength(20, ErrorMessage = "价格方案类型长度不能超过20个字符")]
    public string PlanType { get; set; } = "Monthly";
    
    /// <summary>
    /// 方案名称
    /// </summary>
    [Required(ErrorMessage = "方案名称不能为空")]
    [StringLength(50, ErrorMessage = "方案名称长度不能超过50个字符")]
    public string PlanName { get; set; } = "包月";
    
    /// <summary>
    /// 原价（元）
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "原价必须大于等于0")]
    public decimal OriginalPrice { get; set; } = 0;
    
    /// <summary>
    /// 折扣后价格（元）
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "折扣后价格必须大于等于0")]
    public decimal DiscountedPrice { get; set; } = 0;
    
    /// <summary>
    /// 折扣百分比（0-100）
    /// </summary>
    [Range(0, 100, ErrorMessage = "折扣百分比必须在0-100之间")]
    public decimal DiscountPercent { get; set; } = 0;
    
    /// <summary>
    /// 方案描述
    /// </summary>
    [StringLength(200, ErrorMessage = "方案描述长度不能超过200个字符")]
    public string? Description { get; set; }
    
    /// <summary>
    /// 是否推荐方案
    /// </summary>
    public bool IsRecommended { get; set; } = false;
    
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; } = true;
    
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; } = 0;
}

/// <summary>
/// 会员等级价格方案更新输入
/// </summary>
public class MemberLevelPricePlanUpdateInput : MemberLevelPricePlanAddInput
{
    public long Id { get; set; }
}

/// <summary>
/// 会员等级价格方案分页输入
/// </summary>
public class MemberLevelPricePlanGetPageInput
{
    public long? MemberLevelId { get; set; }
    public string? PlanType { get; set; }
    public bool? Enabled { get; set; }
}
