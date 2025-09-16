using ChipSys.Admin.Core.Dto;
using ChipSys.Client.Contracts.Services.Client.Dto;

namespace ChipSys.Client.Contracts.Services.Client;

/// <summary>
/// 会员认证服务接口
/// </summary>
public interface IMemberAuthService
{
    /// <summary>
    /// 发送手机验证码
    /// </summary>
    /// <param name="mobile">手机号</param>
    /// <returns>验证码ID</returns>
    Task<string> SendMobileCodeAsync(string mobile);

    /// <summary>
    /// 发送邮箱验证码
    /// </summary>
    /// <param name="email">邮箱地址</param>
    /// <returns>验证码ID</returns>
    Task<string> SendEmailCodeAsync(string email);

    /// <summary>
    /// 检查手机号是否已注册
    /// </summary>
    /// <param name="mobile">手机号</param>
    /// <returns>是否已注册</returns>
    Task<bool> CheckMobileExistsAsync(string mobile);

    /// <summary>
    /// 检查邮箱是否已注册
    /// </summary>
    /// <param name="email">邮箱地址</param>
    /// <returns>是否已注册</returns>
    Task<bool> CheckEmailExistsAsync(string email);

    /// <summary>
    /// 手机号注册
    /// </summary>
    /// <param name="input">注册信息</param>
    /// <returns>登录结果</returns>
    Task<MemberLoginOutput> RegisterByMobileAsync(MemberRegByMobileInput input);

    /// <summary>
    /// 邮箱注册
    /// </summary>
    /// <param name="input">注册信息</param>
    /// <returns>登录结果</returns>
    Task<MemberLoginOutput> RegisterByEmailAsync(MemberRegByEmailInput input);

    /// <summary>
    /// 微信注册
    /// </summary>
    /// <param name="input">注册信息</param>
    /// <returns>登录结果</returns>
    Task<MemberLoginOutput> RegisterByWechatAsync(MemberRegByWechatInput input);

    /// <summary>
    /// 手机号登录
    /// </summary>
    /// <param name="input">登录信息</param>
    /// <returns>登录结果</returns>
    Task<MemberLoginOutput> LoginByMobileAsync(MemberLoginByMobileInput input);

    /// <summary>
    /// 微信登录
    /// </summary>
    /// <param name="input">登录信息</param>
    /// <returns>登录结果</returns>
    Task<MemberLoginOutput> LoginByWechatAsync(MemberLoginByWechatInput input);

    /// <summary>
    /// 获取会员信息
    /// </summary>
    /// <returns>会员信息</returns>
    Task<MemberInfoOutput> GetMemberInfoAsync();

    /// <summary>
    /// 更新会员信息
    /// </summary>
    /// <param name="input">会员信息</param>
    /// <returns>操作结果</returns>
    Task UpdateProfileAsync(MemberProfileUpdateInput input);

    /// <summary>
    /// 刷新令牌
    /// </summary>
    /// <param name="refreshToken">刷新令牌</param>
    /// <returns>新的令牌信息</returns>
    Task<MemberLoginOutput> RefreshTokenAsync(string refreshToken);
}
