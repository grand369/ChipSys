using System.ComponentModel.DataAnnotations;

namespace ChipSys.Client.Contracts.Services.Client.Dto;

/// <summary>
/// 会员手机号注册输入
/// </summary>
public class MemberRegByMobileInput
{
    /// <summary>
    /// 手机号
    /// </summary>
    [Required(ErrorMessage = "请输入手机号")]
    [RegularExpression(@"^1[3-9]\d{9}$", ErrorMessage = "手机号格式不正确")]
    public string Mobile { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    [Required(ErrorMessage = "请输入验证码")]
    public string Code { get; set; }

    /// <summary>
    /// 验证码Id
    /// </summary>
    [Required(ErrorMessage = "请先获取验证码")]
    public string CodeId { get; set; }
}

/// <summary>
/// 会员邮箱注册输入
/// </summary>
public class MemberRegByEmailInput
{
    /// <summary>
    /// 邮箱地址
    /// </summary>
    [Required(ErrorMessage = "请输入邮箱地址")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string Email { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    [Required(ErrorMessage = "请输入验证码")]
    public string Code { get; set; }

    /// <summary>
    /// 验证码Id
    /// </summary>
    [Required(ErrorMessage = "请先获取验证码")]
    public string CodeId { get; set; }
}

/// <summary>
/// 会员微信注册输入
/// </summary>
public class MemberRegByWechatInput
{
    /// <summary>
    /// 微信授权码
    /// </summary>
    [Required(ErrorMessage = "微信授权码不能为空")]
    public string Code { get; set; }

    /// <summary>
    /// 微信OpenId
    /// </summary>
    public string? OpenId { get; set; }

    /// <summary>
    /// 微信UnionId
    /// </summary>
    public string? UnionId { get; set; }

    /// <summary>
    /// 微信昵称
    /// </summary>
    public string? NickName { get; set; }

    /// <summary>
    /// 微信头像
    /// </summary>
    public string? Avatar { get; set; }
}

/// <summary>
/// 会员手机号登录输入
/// </summary>
public class MemberLoginByMobileInput
{
    /// <summary>
    /// 手机号
    /// </summary>
    [Required(ErrorMessage = "请输入手机号")]
    [RegularExpression(@"^1[3-9]\d{9}$", ErrorMessage = "手机号格式不正确")]
    public string Mobile { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    [Required(ErrorMessage = "请输入验证码")]
    public string Code { get; set; }

    /// <summary>
    /// 验证码Id
    /// </summary>
    [Required(ErrorMessage = "请先获取验证码")]
    public string CodeId { get; set; }
}

/// <summary>
/// 会员微信登录输入
/// </summary>
public class MemberLoginByWechatInput
{
    /// <summary>
    /// 微信授权码
    /// </summary>
    [Required(ErrorMessage = "微信授权码不能为空")]
    public string Code { get; set; }
}

/// <summary>
/// 会员登录输出
/// </summary>
public class MemberLoginOutput
{
    /// <summary>
    /// 访问令牌
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// 刷新令牌
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime Expires { get; set; }

    /// <summary>
    /// 会员信息
    /// </summary>
    public MemberInfoOutput MemberInfo { get; set; }
}

/// <summary>
/// 会员信息输出
/// </summary>
public class MemberInfoOutput
{
    /// <summary>
    /// 会员ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 会员昵称
    /// </summary>
    public string? NickName { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string? RealName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// 所属区域
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// 是否已完善信息
    /// </summary>
    public bool IsProfileComplete { get; set; }

    /// <summary>
    /// 微信信息
    /// </summary>
    public WechatInfoOutput? WechatInfo { get; set; }
}

/// <summary>
/// 微信信息输出
/// </summary>
public class WechatInfoOutput
{
    /// <summary>
    /// 微信昵称
    /// </summary>
    public string? NickName { get; set; }

    /// <summary>
    /// 微信头像
    /// </summary>
    public string? Avatar { get; set; }
}

/// <summary>
/// 会员信息补充输入
/// </summary>
public class MemberProfileUpdateInput
{
    /// <summary>
    /// 会员昵称
    /// </summary>
    [Required(ErrorMessage = "请输入昵称")]
    [StringLength(50, ErrorMessage = "昵称长度不能超过50个字符")]
    public string NickName { get; set; }

    /// <summary>
    /// 密码（可选）
    /// </summary>
    [StringLength(50, MinimumLength = 6, ErrorMessage = "密码长度必须在6-50个字符之间")]
    public string? Password { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [StringLength(50, ErrorMessage = "真实姓名长度不能超过50个字符")]
    public string? RealName { get; set; }

    /// <summary>
    /// 身份证号
    /// </summary>
    [RegularExpression(@"^[1-9]\d{5}(18|19|20)\d{2}((0[1-9])|(1[0-2]))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$", ErrorMessage = "身份证号格式不正确")]
    public string? IdCard { get; set; }

    /// <summary>
    /// 性别：Male-男，Female-女，Other-其他
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// 所属省份
    /// </summary>
    [StringLength(50, ErrorMessage = "省份长度不能超过50个字符")]
    public string? Province { get; set; }

    /// <summary>
    /// 所属城市
    /// </summary>
    [StringLength(50, ErrorMessage = "城市长度不能超过50个字符")]
    public string? City { get; set; }

    /// <summary>
    /// 所属区县
    /// </summary>
    [StringLength(50, ErrorMessage = "区县长度不能超过50个字符")]
    public string? District { get; set; }

    /// <summary>
    /// 详细地址
    /// </summary>
    [StringLength(200, ErrorMessage = "详细地址长度不能超过200个字符")]
    public string? Address { get; set; }
}
