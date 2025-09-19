using ChipSys.Admin.Core.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChipSys.Client.Contracts.Services.Client.Dto;

/// <summary>
/// 会员等级输出
/// </summary>
public class MemberLevelGetOutput : MemberLevelAddInput
{
    public long Id { get; set; }
    public DateTime CreatedTime { get; set; }
}

/// <summary>
/// 会员等级分页输出
/// </summary>
public class MemberLevelGetPageOutput : MemberLevelGetOutput
{
    // 可以添加更多用于列表展示的字段
}

/// <summary>
/// 会员等级添加输入
/// </summary>
public class MemberLevelAddInput
{
    /// <summary>
    /// 会员ID（可选，用于直接为特定会员创建等级，0表示等级模板）
    /// </summary>
    public long? MemberId { get; set; }
    
    [Required(ErrorMessage = "会员等级不能为空")]
    [StringLength(20, ErrorMessage = "会员等级长度不能超过20个字符")]
    public string Level { get; set; } = "Free";
    
    /// <summary>
    /// 等级显示名称
    /// </summary>
    [StringLength(50, ErrorMessage = "等级显示名称长度不能超过50个字符")]
    public string LevelName { get; set; } = "免费会员";
    
    [Range(0, int.MaxValue, ErrorMessage = "分类限制必须大于等于0")]
    public int CategoryLimit { get; set; } = 0;
    
    [Required(ErrorMessage = "产品数据限制必须大于0")]
    [Range(1, int.MaxValue, ErrorMessage = "产品数据限制必须大于0")]
    public int ProductDataLimit { get; set; } = 50;
    
    [Required(ErrorMessage = "供应商数据限制必须大于0")]
    [Range(1, int.MaxValue, ErrorMessage = "供应商数据限制必须大于0")]
    public int SupplierDataLimit { get; set; } = 50;
    
    public bool ShowFullContactInfo { get; set; } = false;
    
    [Range(0, double.MaxValue, ErrorMessage = "原价必须大于等于0")]
    public decimal OriginalPrice { get; set; } = 0;
    
    [Range(0, double.MaxValue, ErrorMessage = "折扣后价格必须大于等于0")]
    public decimal DiscountedPrice { get; set; } = 0;
    
    [Range(0, 100, ErrorMessage = "折扣百分比必须在0-100之间")]
    public decimal DiscountPercent { get; set; } = 0;
    
    public bool Enabled { get; set; } = true;
    
    [Required(ErrorMessage = "生效时间不能为空")]
    public DateTime? EffectiveTime { get; set; }
    
    [Required(ErrorMessage = "过期时间不能为空")]
    public DateTime? ExpireTime { get; set; }
}

/// <summary>
/// 会员等级更新输入
/// </summary>
public class MemberLevelUpdateInput : MemberLevelAddInput
{
    [Required(ErrorMessage = "请选择要修改的会员等级")]
    public long Id { get; set; }
}

/// <summary>
/// 会员等级分页输入
/// </summary>
public class MemberLevelGetPageInput : PageInput
{
    public long? MemberId { get; set; }
    public string? Level { get; set; }
    public bool? Enabled { get; set; }
}
