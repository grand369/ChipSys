using ChipSys.Admin.Core.Dto;
using System.ComponentModel.DataAnnotations;

namespace ChipSys.Client.Contracts.Services.Client.Dto;

/// <summary>
/// 会员收藏查询输入
/// </summary>
public class MemberFavoriteGetPageInput : PageInput<MemberFavoriteGetPageFilter>
{
}

/// <summary>
/// 会员收藏查询过滤
/// </summary>
public class MemberFavoriteGetPageFilter
{
    /// <summary>
    /// 收藏类型
    /// </summary>
    public string? FavoriteType { get; set; }

    /// <summary>
    /// 收藏对象名称
    /// </summary>
    public string? FavoriteName { get; set; }
}

/// <summary>
/// 会员收藏分页输出
/// </summary>
public class MemberFavoriteGetPageOutput
{
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 收藏类型
    /// </summary>
    public string FavoriteType { get; set; }

    /// <summary>
    /// 收藏对象Id
    /// </summary>
    public long FavoriteId { get; set; }

    /// <summary>
    /// 收藏对象名称
    /// </summary>
    public string FavoriteName { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }
}

/// <summary>
/// 会员收藏详情输出
/// </summary>
public class MemberFavoriteGetOutput
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
    /// 收藏类型
    /// </summary>
    public string FavoriteType { get; set; }

    /// <summary>
    /// 收藏对象Id
    /// </summary>
    public long FavoriteId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }
}

/// <summary>
/// 会员收藏添加输入
/// </summary>
public class MemberFavoriteAddInput
{
    /// <summary>
    /// 收藏类型
    /// </summary>
    [Required(ErrorMessage = "收藏类型不能为空")]
    [StringLength(50, ErrorMessage = "收藏类型长度不能超过50个字符")]
    public string FavoriteType { get; set; }

    /// <summary>
    /// 收藏对象Id
    /// </summary>
    [Required(ErrorMessage = "收藏对象ID必须大于0")]
    [Range(1, long.MaxValue, ErrorMessage = "收藏对象ID必须大于0")]
    public long FavoriteId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
    public string? Remark { get; set; }
}

/// <summary>
/// 会员收藏修改输入
/// </summary>
public class MemberFavoriteUpdateInput
{
    /// <summary>
    /// 主键
    /// </summary>
    [Required(ErrorMessage = "请选择要修改的会员收藏")]
    public long Id { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
    public string? Remark { get; set; }
}
