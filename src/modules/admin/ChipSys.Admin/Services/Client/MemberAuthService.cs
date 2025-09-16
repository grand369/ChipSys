using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Services;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Repositories;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Client.Contracts.Domain.Client;
using ChipSys.Client.Contracts.Services.Client;
using ChipSys.Client.Contracts.Services.Client.Dto;
using FreeSql;
using System.Linq;
using System.Collections.Generic;
using ChipSys.Admin.Core.Helpers;
using ChipSys.Admin.Core.Auth;
using ChipSys.Admin.Domain.User;
using ChipSys.Admin.Domain.UserRole;
using ChipSys.Admin.Domain.Role;
using ChipSys.Admin.Domain.Tenant;
using ChipSys.Admin.Core.Configs;
using Microsoft.Extensions.Options;
using ChipSys.Admin.Services.User;
using ChipSys.Admin.Services.Auth;
using ChipSys.Common.Helpers;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ChipSys.Admin.Core.Extensions;
using ChipSys.Admin.Core;
using ChipSys.Admin.Domain.Org;
using ChipSys.Admin.Services.Auth.Dto;

namespace ChipSys.Admin.Services.Client;

/// <summary>
/// 会员认证服务
/// </summary>
[Order(100)]
[DynamicApi(Area = "client")]
public class MemberAuthService : BaseService, IMemberAuthService, IDynamicApi
{
    private readonly AdminRepositoryBase<MemberEntity> _memberRep;
    private readonly AdminRepositoryBase<UserEntity> _userRep;
    private readonly AdminRepositoryBase<RoleEntity> _roleRep;
    private readonly AdminRepositoryBase<TenantEntity> _tenantRep;
    private readonly AdminLocalizer _adminLocalizer;
    private readonly IUserService _userService;
    private readonly IOptions<JwtConfig> _jwtConfig;
    private readonly AuthService _authService;
    private readonly IOptions<AppConfig> _appConfig;

    public MemberAuthService(
        AdminRepositoryBase<MemberEntity> memberRep,
        AdminRepositoryBase<UserEntity> userRep,
        AdminRepositoryBase<RoleEntity> roleRep,
        AdminRepositoryBase<TenantEntity> tenantRep,
        AdminLocalizer adminLocalizer,
        IOptions<AppConfig> appConfig,
        IUserService userService,
        IOptions<JwtConfig> jwtConfig,
        AuthService authService)
    {
        _memberRep = memberRep;
        _userRep = userRep;
        _roleRep = roleRep;
        _tenantRep = tenantRep;
        _adminLocalizer = adminLocalizer;
        _userService = userService;
        _jwtConfig = jwtConfig;
        _authService = authService;
        _appConfig = appConfig;
    }

    /// <summary>
    /// 发送手机验证码
    /// </summary>
    /// <param name="mobile">手机号</param>
    /// <returns>验证码ID</returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<string> SendMobileCodeAsync(string mobile)
    {
        if (string.IsNullOrEmpty(mobile))
        {
            throw ResultOutput.Exception(_adminLocalizer["请输入手机号"]);
        }

        if (!IsValidMobile(mobile))
        {
            throw ResultOutput.Exception(_adminLocalizer["手机号格式不正确"]);
        }

        // 生成验证码
        var code = StringHelper.GenerateRandomNumber(6);
        var codeId = Guid.NewGuid().ToString();

        // 存储验证码到缓存
        var codeKey = CacheKeys.GetSmsCodeKey(mobile, codeId);
        await Cache.SetAsync(codeKey, code, TimeSpan.FromMinutes(5));

        // TODO: 发送短信验证码
        // 这里应该调用短信服务发送验证码
        // 暂时在日志中输出验证码，实际使用时需要替换为真实的短信服务
        AppInfo.Log.Info($"手机验证码: {mobile} -> {code}");

        return codeId;
    }

    /// <summary>
    /// 发送邮箱验证码
    /// </summary>
    /// <param name="email">邮箱地址</param>
    /// <returns>验证码ID</returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<string> SendEmailCodeAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw ResultOutput.Exception(_adminLocalizer["请输入邮箱地址"]);
        }

        if (!IsValidEmail(email))
        {
            throw ResultOutput.Exception(_adminLocalizer["邮箱格式不正确"]);
        }

        // 生成验证码
        var code = StringHelper.GenerateRandomNumber(6);
        var codeId = Guid.NewGuid().ToString();

        // 存储验证码到缓存
        var codeKey = CacheKeys.GetEmailCodeKey(email, codeId);
        await Cache.SetAsync(codeKey, code, TimeSpan.FromMinutes(5));

        // TODO: 发送邮箱验证码
        // 这里应该调用邮件服务发送验证码
        // 暂时在日志中输出验证码，实际使用时需要替换为真实的邮件服务
        AppInfo.Log.Info($"邮箱验证码: {email} -> {code}");

        return codeId;
    }

    /// <summary>
    /// 检查手机号是否已注册
    /// </summary>
    /// <param name="mobile">手机号</param>
    /// <returns>是否已注册</returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<bool> CheckMobileExistsAsync(string mobile)
    {
        if (string.IsNullOrEmpty(mobile))
        {
            return false;
        }

        using var _ = _userRep.DataFilter.DisableAll();
        using var __ = _userRep.DataFilter.Enable(FilterNames.Delete);

        var exists = await _userRep.Select.Where(a => a.Mobile == mobile).AnyAsync();
        return exists;
    }

    /// <summary>
    /// 检查邮箱是否已注册
    /// </summary>
    /// <param name="email">邮箱地址</param>
    /// <returns>是否已注册</returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<bool> CheckEmailExistsAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }

        using var _ = _userRep.DataFilter.DisableAll();
        using var __ = _userRep.DataFilter.Enable(FilterNames.Delete);

        var exists = await _userRep.Select.Where(a => a.Email == email).AnyAsync();
        return exists;
    }

    /// <summary>
    /// 手机号注册
    /// </summary>
    /// <param name="input">注册信息</param>
    /// <returns>登录结果</returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<MemberLoginOutput> RegisterByMobileAsync(MemberRegByMobileInput input)
    {
        // 验证手机号格式
        if (!IsValidMobile(input.Mobile))
        {
            throw ResultOutput.Exception(_adminLocalizer["手机号格式不正确"]);
        }

        // 验证验证码
        await ValidateMobileCodeAsync(input.Mobile, input.CodeId, input.Code);

        // 检查手机号是否已注册
        var exists = await CheckMobileExistsAsync(input.Mobile);
        if (exists)
        {
            throw ResultOutput.Exception(_adminLocalizer["该手机号已注册"]);
        }

        // 创建用户和会员
        var result = await CreateUserAndMemberAsync(input.Mobile, null, input.Mobile, null);
        return result;
    }

    /// <summary>
    /// 邮箱注册
    /// </summary>
    /// <param name="input">注册信息</param>
    /// <returns>登录结果</returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<MemberLoginOutput> RegisterByEmailAsync(MemberRegByEmailInput input)
    {
        // 验证邮箱格式
        if (!IsValidEmail(input.Email))
        {
            throw ResultOutput.Exception(_adminLocalizer["邮箱格式不正确"]);
        }

        // 验证验证码
        await ValidateEmailCodeAsync(input.Email, input.CodeId, input.Code);

        // 检查邮箱是否已注册
        var exists = await CheckEmailExistsAsync(input.Email);
        if (exists)
        {
            throw ResultOutput.Exception(_adminLocalizer["该邮箱已注册"]);
        }

        // 创建用户和会员
        var result = await CreateUserAndMemberAsync(null, input.Email, null, input.Email);
        return result;
    }

    /// <summary>
    /// 微信注册
    /// </summary>
    /// <param name="input">注册信息</param>
    /// <returns>登录结果</returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<MemberLoginOutput> RegisterByWechatAsync(MemberRegByWechatInput input)
    {
        // TODO: 验证微信授权码，获取微信用户信息
        // 这里应该调用微信API验证授权码并获取用户信息
        // 暂时使用传入的信息

        // 检查微信OpenId是否已注册
        var exists = await _memberRep.Select.Where(a => a.WechatOpenId == input.OpenId).AnyAsync();
        if (exists)
        {
            throw ResultOutput.Exception(_adminLocalizer["该微信账号已注册"]);
        }

        // 创建用户和会员
        var result = await CreateUserAndMemberAsync(null, null, input.NickName, null, input);
        return result;
    }

    /// <summary>
    /// 手机号登录
    /// </summary>
    /// <param name="input">登录信息</param>
    /// <returns>登录结果</returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<MemberLoginOutput> LoginByMobileAsync(MemberLoginByMobileInput input)
    {
        // 验证手机号格式
        if (!IsValidMobile(input.Mobile))
        {
            throw ResultOutput.Exception(_adminLocalizer["手机号格式不正确"]);
        }

        // 验证验证码
        await ValidateMobileCodeAsync(input.Mobile, input.CodeId, input.Code);

        // 查找用户
        using var _ = _userRep.DataFilter.DisableAll();
        using var __ = _userRep.DataFilter.Enable(FilterNames.Delete);

        var user = await _userRep.Select.Where(a => a.Mobile == input.Mobile).ToOneAsync();
        if (user == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["用户不存在"]);
        }

        if (!user.Enabled)
        {
            throw ResultOutput.Exception(_adminLocalizer["账号已停用"]);
        }
        using var ___ = _memberRep.DataFilter.DisableAll();
        using var ____ = _memberRep.DataFilter.Enable(FilterNames.Delete);

        // 查找会员信息
        var member = await _memberRep.Select.Where(a => a.UserId == user.Id && a.TenantId == user.TenantId).ToOneAsync();
        if (member == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["会员信息不存在"]);
        }

        // 生成登录结果
        return await GenerateLoginResultAsync(user, member);
    }

    /// <summary>
    /// 微信登录
    /// </summary>
    /// <param name="input">登录信息</param>
    /// <returns>登录结果</returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<MemberLoginOutput> LoginByWechatAsync(MemberLoginByWechatInput input)
    {
        // TODO: 验证微信授权码，获取微信用户信息
        // 这里应该调用微信API验证授权码并获取用户信息
        // 暂时使用模拟数据

        // 查找会员信息
        var member = await _memberRep.Select.Where(a => a.WechatOpenId == "mock_openid").ToOneAsync();
        if (member == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["微信账号未注册"]);
        }

        // 查找用户信息
        var user = await _userRep.Select.Where(a => a.Id == member.UserId).ToOneAsync();
        if (user == null || !user.Enabled)
        {
            throw ResultOutput.Exception(_adminLocalizer["用户不存在或已停用"]);
        }

        // 生成登录结果
        return await GenerateLoginResultAsync(user, member);
    }

    /// <summary>
    /// 获取会员信息
    /// </summary>
    /// <returns>会员信息</returns>
    [Login]
    public async Task<MemberInfoOutput> GetMemberInfoAsync()
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["未登录"]);
        }

        var member = await _memberRep.Select.Where(a => a.UserId == User.Id).ToOneAsync();
        if (member == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["会员信息不存在"]);
        }

        var user = await _userRep.Select.Where(a => a.Id == User.Id).ToOneAsync();
        if (user == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["用户信息不存在"]);
        }

        return new MemberInfoOutput
        {
            Id = member.Id,
            UserId = member.UserId,
            NickName = member.NickName,
            RealName = member.RealName,
            Mobile = user.Mobile,
            Email = user.Email,
            Gender = member.Gender,
            Region = $"{member.Province}{member.City}{member.District}".Trim(),
            IsProfileComplete = member.IsProfileComplete,
            WechatInfo = member.WechatOpenId != null ? new WechatInfoOutput
            {
                NickName = member.WechatNickName,
                Avatar = member.WechatAvatar
            } : null
        };
    }

    /// <summary>
    /// 更新会员信息
    /// </summary>
    /// <param name="input">会员信息</param>
    /// <returns>操作结果</returns>
    [Login]
    public async Task UpdateProfileAsync(MemberProfileUpdateInput input)
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["未登录"]);
        }

        var member = await _memberRep.Select.Where(a => a.UserId == User.Id).ToOneAsync();
        if (member == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["会员信息不存在"]);
        }

        // 更新会员信息
        member.NickName = input.NickName;
        member.RealName = input.RealName;
        member.IdCard = input.IdCard;
        member.Gender = input.Gender;
        member.Province = input.Province;
        member.City = input.City;
        member.District = input.District;
        member.Address = input.Address;
        member.IsProfileComplete = true;

        await _memberRep.UpdateAsync(member);

        // 如果设置了密码，更新用户密码
        if (!string.IsNullOrEmpty(input.Password))
        {
            var user = await _userRep.Select.Where(a => a.Id == User.Id).ToOneAsync();
            if (user != null)
            {
                user.Password = MD5Encrypt.Encrypt32(input.Password);
                await _userRep.UpdateAsync(user);
            }
        }
    }

    /// <summary>
    /// 刷新令牌
    /// </summary>
    /// <param name="refreshToken">刷新令牌</param>
    /// <returns>新的令牌信息</returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<MemberLoginOutput> RefreshTokenAsync(string refreshToken)
    {
        // TODO: 实现刷新令牌逻辑
        throw new NotImplementedException("刷新令牌功能待实现");
    }

    #region 私有方法

    /// <summary>
    /// 验证手机验证码
    /// </summary>
    private async Task ValidateMobileCodeAsync(string mobile, string codeId, string code)
    {
        if (string.IsNullOrEmpty(codeId) || string.IsNullOrEmpty(code))
        {
            throw ResultOutput.Exception(_adminLocalizer["验证码错误"]);
        }

        // 开发环境测试验证码
        if (code == "123456" && (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" || 
                                 Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == "Development"))
        {
            AppInfo.Log.Info($"使用开发环境测试验证码: {mobile} -> {code}");
            return;
        }

        var codeKey = CacheKeys.GetSmsCodeKey(mobile, codeId);
        var cachedCode = await Cache.GetAsync(codeKey);
        if (cachedCode == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["验证码已过期"]);
        }

        await Cache.DelAsync(codeKey);
        if (cachedCode != code)
        {
            throw ResultOutput.Exception(_adminLocalizer["验证码错误"]);
        }
    }

    /// <summary>
    /// 验证邮箱验证码
    /// </summary>
    private async Task ValidateEmailCodeAsync(string email, string codeId, string code)
    {
        if (string.IsNullOrEmpty(codeId) || string.IsNullOrEmpty(code))
        {
            throw ResultOutput.Exception(_adminLocalizer["验证码错误"]);
        }

        // 开发环境测试验证码
        if (code == "123456" && (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" || 
                                 Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == "Development"))
        {
            AppInfo.Log.Info($"使用开发环境测试验证码: {email} -> {code}");
            return;
        }

        var codeKey = CacheKeys.GetEmailCodeKey(email, codeId);
        var cachedCode = await Cache.GetAsync(codeKey);
        if (cachedCode == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["验证码已过期"]);
        }

        await Cache.DelAsync(codeKey);
        if (cachedCode != code)
        {
            throw ResultOutput.Exception(_adminLocalizer["验证码错误"]);
        }
    }

    /// <summary>
    /// 创建用户和会员
    /// </summary>
    private async Task<MemberLoginOutput> CreateUserAndMemberAsync(
        string? mobile, 
        string? email, 
        string? userName, 
        string? displayName,
        MemberRegByWechatInput? wechatInput = null)
    {
        // 获取默认租户（中台租户）
        // 这里使用第一个可用的租户作为默认租户
        var defaultTenant = await _tenantRep.Select.Where(a => a.Enabled).FirstAsync();
        if (defaultTenant == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["系统配置错误：未找到默认租户"]);
        }

        // 获取会员角色，如果不存在则创建
        using var _ = _roleRep.DataFilter.DisableAll();
        using var __ = _roleRep.DataFilter.Enable(FilterNames.Delete);

        var memberRole = await _roleRep.Select.Where(a => a.Name == "会员" && a.TenantId == defaultTenant.Id).FirstAsync();
        if (memberRole == null)
        {
            // 创建会员角色
            memberRole = new RoleEntity
            {
                TenantId = defaultTenant.Id,
                Name = "会员",
                Code = "Member",
                Description = "普通会员角色",
                Type = RoleType.Role,
                CreatedTime = DateTime.Now
            };
            await _roleRep.InsertAsync(memberRole);
            AppInfo.Log.Info($"自动创建会员角色: {memberRole.Name} (ID: {memberRole.Id})");
        }

        // 创建用户
        var user = new UserEntity
        {
            TenantId = defaultTenant.Id,
            UserName = userName ?? mobile ?? email,
            Name = displayName ?? userName ?? mobile ?? email,
            Mobile = mobile,
            Email = email,
            Password = MD5Encrypt.Encrypt32("123456"), // 默认密码
            Enabled = true,
            CreatedTime = DateTime.Now
        };

        await _userRep.InsertAsync(user);

        // 分配会员角色
        var userRole = new UserRoleEntity
        {
            UserId = user.Id,
            RoleId = memberRole.Id,
            CreatedTime = DateTime.Now
        };

        await _userRep.Orm.Insert(userRole).ExecuteAffrowsAsync();

        // 创建会员信息
        var member = new MemberEntity
        {
            UserId = user.Id,
            NickName = displayName ?? userName ?? mobile ?? email,
            WechatOpenId = wechatInput?.OpenId,
            WechatUnionId = wechatInput?.UnionId,
            WechatNickName = wechatInput?.NickName,
            WechatAvatar = wechatInput?.Avatar,
            IsProfileComplete = wechatInput != null, // 微信注册默认已完善信息
            Enabled = true,
            CreatedTime = DateTime.Now,
            TenantId = defaultTenant.Id,
        };

        // 设置数据权限归属（OwnerOrgId、OwnerOrgName）
        try
        {
            var org = await _tenantRep.Orm.Select<OrgEntity>()
                .Where(x => x.Id == defaultTenant.OrgId && x.TenantId == defaultTenant.Id)
                .FirstAsync();
            if (org != null)
            {
                member.OwnerOrgId = org.Id;
                member.OwnerOrgName = org.Name;
            }
            else
            {
                // 回退：至少设置OwnerOrgId为租户绑定的OrgId
                member.OwnerOrgId = defaultTenant.OrgId;
            }
        }
        catch { }

        await _memberRep.InsertAsync(member);

        // 生成登录结果
        return await GenerateLoginResultAsync(user, member);
    }

    /// <summary>
    /// 生成登录结果
    /// </summary>
    private async Task<MemberLoginOutput> GenerateLoginResultAsync(UserEntity user, MemberEntity member)
    {
        // 使用与管理员登录相同的token生成方式
        var authLoginOutput = new AuthLoginOutput
        {
            Id = user.Id,
            UserName = user.UserName,
            Name = user.Name,
            Type = user.Type,
            TenantId = user.TenantId
        };
        if (_appConfig.Value.Tenant)
        {
            var tenant = await _tenantRep.Select.WhereDynamic(user.TenantId).ToOneAsync<AuthLoginTenantModel>();
            if (!(tenant != null && tenant.Enabled))
            {
                throw ResultOutput.Exception(_adminLocalizer["企业已停用，禁止登录"]);
            }
            authLoginOutput.Tenant = tenant;
        }
        // 使用AuthService的token生成方法
        var accessToken = _authService.GetToken(authLoginOutput);
        var refreshToken = Guid.NewGuid().ToString();

        // 存储刷新令牌到缓存
        var refreshKey = CacheKeys.GetRefreshTokenKey(refreshToken);
        await Cache.SetAsync(refreshKey, user.Id.ToString(), TimeSpan.FromDays(7));

        // 计算token过期时间
        var expires = DateTime.Now.AddMinutes(_jwtConfig.Value.Expires);

        return new MemberLoginOutput
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expires = expires,
            MemberInfo = new MemberInfoOutput
            {
                Id = member.Id,
                UserId = member.UserId,
                NickName = member.NickName,
                RealName = member.RealName,
                Mobile = user.Mobile,
                Email = user.Email,
                Gender = member.Gender,
                Region = $"{member.Province}{member.City}{member.District}".Trim(),
                IsProfileComplete = member.IsProfileComplete,
                WechatInfo = member.WechatOpenId != null ? new WechatInfoOutput
                {
                    NickName = member.WechatNickName,
                    Avatar = member.WechatAvatar
                } : null
            }
        };
    }

    /// <summary>
    /// 验证手机号格式
    /// </summary>
    /// <param name="mobile">手机号</param>
    /// <returns></returns>
    private static bool IsValidMobile(string mobile)
    {
        if (string.IsNullOrEmpty(mobile))
            return false;
        
        // 中国手机号正则表达式
        return Regex.IsMatch(mobile, @"^1[3-9]\d{9}$");
    }

    /// <summary>
    /// 验证邮箱格式
    /// </summary>
    /// <param name="email">邮箱地址</param>
    /// <returns></returns>
    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;
        
        // 邮箱正则表达式
        return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
    }

    #endregion
}
