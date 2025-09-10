using DotNetCore.CAP;
using FreeSql;
using IP2Region.Net.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Utilities.Encoders;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using ChipSys.Admin.Core;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Auth;
using ChipSys.Admin.Core.Captcha;
using ChipSys.Admin.Core.Configs;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Db;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Helpers;
using ChipSys.Admin.Domain.Permission;
using ChipSys.Admin.Domain.PkgPermission;
using ChipSys.Admin.Domain.RolePermission;
using ChipSys.Admin.Domain.Tenant;
using ChipSys.Admin.Domain.TenantPkg;
using ChipSys.Admin.Domain.User;
using ChipSys.Admin.Domain.UserRole;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Services.Auth.Dto;
using ChipSys.Admin.Services.LoginLog;
using ChipSys.Admin.Services.LoginLog.Dto;
using ChipSys.Admin.Services.Tenant;
using ChipSys.Admin.Services.Tenant.Dto;
using ChipSys.Admin.Services.User;
using ChipSys.Common.Extensions;
using ChipSys.Common.Helpers;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Plugin.Lazy.SlideCaptcha.Core.Validator;
using static ChipSys.Plugin.Lazy.SlideCaptcha.Core.ValidateResult;
using LocationInfo = ChipSys.Admin.Core.Records.LocationInfo;

namespace ChipSys.Admin.Services.Auth;

/// <summary>
/// ��֤��Ȩ����
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class AuthService : BaseService, IAuthService, IDynamicApi
{
    private readonly UserHelper _userHelper;
    private readonly AdminLocalizer _adminLocalizer;
    private readonly ICapPublisher _capPublisher;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserToken _userToken;
    private readonly IUserService _userService;
    private readonly ILoginLogService _loginLogService;
    private readonly IOptions<AppConfig> _appConfig;
    private readonly Lazy<IOptions<JwtConfig>> _jwtConfig;
    private readonly Lazy<IUserRepository> _userRep;
    private readonly Lazy<ITenantRepository> _tenantRep;
    private readonly Lazy<IPermissionRepository> _permissionRep;
    private readonly Lazy<IPasswordHasher<UserEntity>> _passwordHasher;
    private readonly Lazy<ISlideCaptcha> _captcha;
    private readonly Lazy<ITenantService> _tenantService;

    public AuthService(
        UserHelper userHelper,
        AdminLocalizer adminLocalizer,
        ICapPublisher capPublisher,
        IHttpContextAccessor httpContextAccessor,
        IUserToken userToken,
        IUserService userService,
        ILoginLogService loginLogService,
        IOptions<AppConfig> appConfig,
        Lazy<IOptions<JwtConfig>> jwtConfig,
        Lazy<IUserRepository> userRep,
        Lazy<ITenantRepository> tenantRep,
        Lazy<IPermissionRepository> permissionRep,
        Lazy<IPasswordHasher<UserEntity>> passwordHasher,
        Lazy<ISlideCaptcha> captcha,
        Lazy<ITenantService> tenantService
    )
    {
        _appConfig = appConfig;
        _jwtConfig = jwtConfig;
        _userRep = userRep;
        _tenantRep = tenantRep;
        _permissionRep = permissionRep;
        _passwordHasher = passwordHasher;
        _captcha = captcha;
        _tenantService = tenantService;
        _userHelper = userHelper;
        _adminLocalizer = adminLocalizer;
        _capPublisher = capPublisher;
        _httpContextAccessor = httpContextAccessor;
        _userToken = userToken;
        _userService = userService;
        _loginLogService = loginLogService;
    }

    /// <summary>
    /// ���ӵ�¼��־
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private async Task AddLoginLogAsync(LoginLogAddInput input)
    {
        if (input.IP.IsNull())
        {
            input.IP = IPHelper.GetIP(_httpContextAccessor?.HttpContext?.Request);
            var locationInfo = GetIpLocationInfo(input.IP);
            input.Country = locationInfo?.Country;
            input.Province = locationInfo?.Province;
            input.City = locationInfo?.City;
            input.Isp = locationInfo?.Isp;
        }

        string ua = _httpContextAccessor?.HttpContext?.Request?.Headers?.UserAgent;
        if (ua.NotNull())
        {
            var client = UAParser.Parser.GetDefault().Parse(ua);
            var device = client.Device.Family;
            device = device.ToLower() == "other" ? "" : device;
            input.Browser = client.UA.Family;
            input.Os = client.OS.Family;
            input.Device = device;
            input.BrowserInfo = ua;
        }

        if (_appConfig.Value.Log.Method == LogMethod.Cap)
        {
            await _capPublisher.PublishAsync(
                SubscribeNames.LoginLogAdd,
                input
            );
        }
        else
        {
            await _loginLogService.AddAsync(input);
        }
    }

    /// <summary>
    /// ��õ�¼��־������Ϣ
    /// </summary>
    /// <param name="authLoginOutput"></param>
    /// <param name="locationInfo"></param>
    /// <param name="user"></param>
    /// <param name="stopwatch"></param>
    /// <returns></returns>
    private LoginLogAddInput GetLoginLogAddInput(AuthLoginOutput authLoginOutput, LocationInfo locationInfo, UserEntity user, Stopwatch stopwatch)
    {
        return new LoginLogAddInput
        {
            TenantId = authLoginOutput.TenantId,
            Name = authLoginOutput.Name,
            ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
            Status = true,
            CreatedUserId = authLoginOutput.Id,
            CreatedUserName = user.UserName,
            Country = locationInfo?.Country,
            Province = locationInfo?.Province,
            City = locationInfo?.City,
            Isp = locationInfo?.Isp,
        };
    }

    /// <summary>
    /// ���IP��ַ
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    private LocationInfo GetIpLocationInfo(string ip)
    {
        var locationInfo = new LocationInfo();
        if (_appConfig.Value.IP2Region.Enable)
        {
            if(IPHelper.IsIP(ip))
            {
                var region = AppInfo.GetRequiredService<ISearcher>().Search(ip);
                locationInfo = LocationInfo.Parse(region);
            }
        }

        return locationInfo;
    }

    /// <summary>
    /// ��������¼��Ϣ
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="ip"></param>
    /// <param name="locationInfo"></param>
    /// <returns></returns>
    [NonAction]
    public async Task UpdateLastLoginInfoAsync(long userId, string ip, LocationInfo locationInfo)
    {
        var update = _userRep.Value.UpdateDiy.Set(a => new UserEntity
        {
            LastLoginIP = ip,
            LastLoginTime = DbHelper.ServerTime,
        });

        if (_appConfig.Value.IP2Region.Enable)
        {
            update = update.Set(a => new UserEntity
            {
                LastLoginCountry = locationInfo.Country,
                LastLoginProvince = locationInfo.Province,
                LastLoginCity = locationInfo.City,
            });
        }

        await update.WhereDynamic(userId)
        .ExecuteAffrowsAsync();
    }

    /// <summary>
    /// ���token
    /// </summary>
    /// <param name="user">�û���Ϣ</param>
    /// <returns></returns>
    [NonAction]
    public string GetToken(AuthLoginOutput user)
    {
        if (user == null)
        {
            return null;
        }

        var claims = new List<Claim>()
        {
            new (ClaimAttributes.UserId, user.Id.ToString(), ClaimValueTypes.Integer64),
            new (ClaimAttributes.UserName, user.UserName),
            new (ClaimAttributes.Name, user.Name??""),
            new (ClaimAttributes.UserType, user.Type.ToInt().ToString(), ClaimValueTypes.Integer32),
            new (JwtRegisteredClaimNames.Iat, DateTime.Now.ToTimestamp().ToString(), ClaimValueTypes.Integer64),
        };

        if (_appConfig.Value.Tenant)
        {
            claims.AddRange(
            [
                new (ClaimAttributes.TenantId, user.TenantId.ToString(), ClaimValueTypes.Integer64),
                new (ClaimAttributes.TenantType, user.Tenant?.TenantType.ToInt().ToString(), ClaimValueTypes.Integer32),
                new (ClaimAttributes.DbKey, user.Tenant?.DbKey??"")
            ]);
        }

        var token = _userToken.Create([.. claims]);

       return token;
    }

    /// <summary>
    /// ���������Ϣ
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [NonAction]
    public TokenInfo GetTokenInfo(AuthLoginOutput user)
    {
        var token = GetToken(user);

        var now = DateTime.Now;
        var jwtConfig = _jwtConfig.Value.Value;

        return new TokenInfo
        {
            AccessToken = token,
            AccessTokenExpiresAt = now.AddMinutes(jwtConfig.Expires),
            AccessTokenLifeTime = jwtConfig.Expires * 60,
            RefreshToken = Guid.NewGuid().ToString("N"),
            RefreshTokenExpiresAt = now.AddMinutes(jwtConfig.Expires + jwtConfig.RefreshExpires),
            RefreshTokenLifeTime = jwtConfig.RefreshExpires * 60,
            Timestamp = now.ToTimestamp()
        };
    }

    /// <summary>
    /// ��ѯ��Կ
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<AuthGetPasswordEncryptKeyOutput> GetPasswordEncryptKeyAsync()
    {
        //д��Redis
        var guid = Guid.NewGuid().ToString("N");
        var key = CacheKeys.PassWordEncrypt + guid;
        //����key
        byte[] keyBytes = Encoding.Default.GetBytes(StringHelper.GenerateRandom(16));
        string keyHexString = BitConverter.ToString(keyBytes);
        var encryptKey = keyHexString.Replace("-", "").ToLower();
        //����iv
        byte[] ivBytes = Encoding.Default.GetBytes(StringHelper.GenerateRandom(16));
        string ivHexString = BitConverter.ToString(ivBytes);
        var iv = ivHexString.Replace("-", "").ToLower();
        //���
        var passwordKeyOutput= new AuthGetPasswordEncryptKeyOutput { Key = guid, EncryptKey = encryptKey, Iv = iv };
        //д����
        await Cache.SetAsync(key, passwordKeyOutput, TimeSpan.FromMinutes(5));
        return passwordKeyOutput;
    }

    /// <summary>
    /// ��ѯ�û�������Ϣ
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<AuthUserProfileOutput> GetUserProfileAsync()
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["δ��¼"]);
        }

        var userRep = _userRep.Value;
        using var _u = userRep.DataFilter.Disable(FilterNames.Self, FilterNames.Data);

        var profile = await userRep
        .Where(u => u.Id == User.Id)
        .FirstAsync(u => new AuthUserProfileOutput 
        {
            DeptName = u.Org.Name,
            CorpName = u.Tenant.Org.Name,
            Position = u.Staff.Position
        });

        var mobile = profile.Mobile?.ToString();
        string number = string.Empty;
        if (mobile.NotNull())
        {
            number = mobile.Length >= 4 ? mobile.Substring(mobile.Length - 4) : mobile;
        }

        if (number.IsNull())
        {
            var userId = User.Id.ToString();
            number = userId.Length >= 4 ? userId.Substring(userId.Length - 4) : userId;
        }

        profile.WatermarkText = $"{profile.Name}@{profile.CorpName} {number}";
            
        return profile;
    }

    /// <summary>
    /// ��ѯ�û��˵��б�
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    [Login]
    public async Task<List<AuthUserMenuOutput>> GetUserMenusAsync(string platform = AdminConsts.WebName)
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["δ��¼"]);
        }

        using (_userRep.Value.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
        {
            var permissionRep = _permissionRep.Value;
            var menuSelect = permissionRep.Select.Where(a => a.Enabled == true);

            Expression<Func<PermissionEntity, bool>> where = null;
            if (platform.NotNull())
            {
                where = where.And(a => a.Platform == platform);
                if (platform.ToLower() == AdminConsts.WebName)
                {
                    where = where.Or(a => string.IsNullOrEmpty(a.Platform));
                }
            }
            else
            {
                where = where.And(a => string.IsNullOrEmpty(a.Platform));
            }
            menuSelect = menuSelect.Where(where);

            if (!User.PlatformAdmin)
            {
                var db = permissionRep.Orm;
                if (User.TenantAdmin)
                {
                    menuSelect = menuSelect.Where(a =>
                       db.Select<TenantPkgEntity, PkgPermissionEntity>()
                       .Where((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                       .Any()
                   );
                }
                else
                {
                    menuSelect = menuSelect.Where(a =>
                       db.Select<RolePermissionEntity>()
                       .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                       .Where(b => b.PermissionId == a.Id)
                       .Any()
                   );
                }
            }

            var menuList = await menuSelect
                .AsTreeCte(up: true)
                .Where(a => a.Type == PermissionType.Group || (a.Type == PermissionType.Menu && a.View.Enabled == true))
                .ToListAsync(a => new AuthUserMenuOutput { ViewPath = a.View.Path });

            return menuList.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort).ToList();

        }
    }

    /// <summary>
    /// ��ѯ�û�Ȩ���б�
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<AuthGetUserPermissionsOutput> GetUserPermissionsAsync(string platform = AdminConsts.WebName)
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["δ��¼"]);
        }

        var userRep = _userRep.Value;
        var permissionRep = _permissionRep.Value;

        using (userRep.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
        {
            var authGetUserPermissionsOutput = new AuthGetUserPermissionsOutput();

            var dotSelect = permissionRep.Select.Where(a => a.Type == PermissionType.Dot);

            Expression<Func<PermissionEntity, bool>> where = null;
            if (platform.NotNull())
            {
                where = where.And(a => a.Platform == platform);
                if (platform.ToLower() == AdminConsts.WebName)
                {
                    where = where.Or(a => string.IsNullOrEmpty(a.Platform));
                }
            }
            else
            {
                where = where.And(a => string.IsNullOrEmpty(a.Platform));
            }
            dotSelect = dotSelect.Where(where);

            if (!User.PlatformAdmin)
            {
                var db = permissionRep.Orm;
                if (User.TenantAdmin)
                {
                    dotSelect = dotSelect.Where(a =>
                       db.Select<TenantPkgEntity, PkgPermissionEntity>()
                       .Where((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                       .Any()
                    );
                }
                else
                {
                    dotSelect = dotSelect.Where(a =>
                        db.Select<RolePermissionEntity>()
                        .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                        .Where(b => b.PermissionId == a.Id)
                        .Any()
                    );
                }
            }

            //�û�Ȩ�޵�
            authGetUserPermissionsOutput.Permissions = await dotSelect.ToListAsync(a => a.Code);

            return authGetUserPermissionsOutput;
        }
    }

    /// <summary>
    /// ��ѯ�û���Ϣ
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<AuthGetUserInfoOutput> GetUserInfoAsync()
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["δ��¼"]);
        }

        var userRep = _userRep.Value;
        var permissionRep = _permissionRep.Value;

        using (userRep.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
        {
            var authGetUserInfoOutput = new AuthGetUserInfoOutput
            {
                //�û���Ϣ
                User = await userRep.GetAsync<AuthUserProfileOutput>(User.Id)
            };

            var menuSelect = permissionRep.Select;
            var dotSelect = permissionRep.Select.Where(a => a.Type == PermissionType.Dot);

            if (!User.PlatformAdmin)
            {
                var db = permissionRep.Orm;
                if (User.TenantAdmin)
                {
                    menuSelect = menuSelect.Where(a =>
                       db.Select<TenantPkgEntity, PkgPermissionEntity>()
                       .Where((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                       .Any()
                   );

                    dotSelect = dotSelect.Where(a =>
                       db.Select<TenantPkgEntity, PkgPermissionEntity>()
                       .Where((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                       .Any()
                    );
                }
                else
                {
                    menuSelect = menuSelect.Where(a =>
                       db.Select<RolePermissionEntity>()
                       .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                       .Where(b => b.PermissionId == a.Id)
                       .Any()
                   );

                    dotSelect = dotSelect.Where(a =>
                        db.Select<RolePermissionEntity>()
                        .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                        .Where(b => b.PermissionId == a.Id)
                        .Any()
                    );
                }

                menuSelect = menuSelect.AsTreeCte(up: true);
            }

            var menuList = await menuSelect
                .Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
                .ToListAsync(a => new AuthUserMenuOutput { ViewPath = a.View.Path });

            //�û��˵�
            authGetUserInfoOutput.Menus = menuList.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort).ToList();

            //�û�Ȩ�޵�
            authGetUserInfoOutput.Permissions = await dotSelect.ToListAsync(a => a.Code);

            return authGetUserInfoOutput;
        }
    }

    /// <summary>
    /// ��¼
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<TokenInfo> LoginAsync(AuthLoginInput input)
    {
        var stopwatch = Stopwatch.StartNew();

        var ip = IPHelper.GetIP(AppInfo.HttpContext?.Request);
        var locationInfo = GetIpLocationInfo(ip);
        var loginLogAddInput = new LoginLogAddInput
        {
            Status = true,
            IP = ip,
            Country = locationInfo?.Country,
            Province = locationInfo?.Province,
            City = locationInfo?.City,
            Isp = locationInfo?.Isp,
            CreatedUserName = input.AccountType switch
            {
                AccountType.UserName => input.UserName,
                AccountType.Email => input.Email,
                AccountType.Mobile => input.Mobile,
                _ => null
            }
        };

        try
        {
            #region ��֤��У��

            if (_appConfig.Value.VarifyCode.Enable)
            {
                if (input.CaptchaId.IsNull() || input.CaptchaData.IsNull())
                {
                    throw ResultOutput.Exception(_adminLocalizer["����ɰ�ȫ��֤"]);
                }
                var validateResult = _captcha.Value.Validate(input.CaptchaId, JsonHelper.Deserialize<SlideTrack>(input.CaptchaData));
                if (validateResult.Result != ValidateResultType.Success)
                {
                    throw ResultOutput.Exception(_adminLocalizer["��ȫ{0}�������µ�¼", validateResult.Message]);
                }
            }

            #endregion

            #region �������

            if (input.PasswordKey.NotNull())
            {
                var passwordEncryptKey = CacheKeys.PassWordEncrypt + input.PasswordKey;
                var existsPasswordKey = await Cache.ExistsAsync(passwordEncryptKey);
                if (existsPasswordKey)
                {
                    var secretKey = await Cache.GetAsync<AuthGetPasswordEncryptKeyOutput>(passwordEncryptKey);
                    if (secretKey.EncryptKey.IsNull())
                    {
                        throw ResultOutput.Exception(_adminLocalizer["����ʧ��"]);
                    }
                    input.Password = SM4Encryption.Decrypt(input.Password, Hex.Decode(secretKey.EncryptKey), Hex.Decode(secretKey.Iv), "CBC", true).TrimEnd('\0');//SM4���ܺ����\0���ţ���Ҫȥ����
                    await Cache.DelAsync(passwordEncryptKey);
                }
                else
                {
                    throw ResultOutput.Exception(_adminLocalizer["����ʧ��"]);
                }
            }

            #endregion

            #region ��¼
            var userRep = _userRep.Value;
            using var _ = userRep.DataFilter.DisableAll();
            using var __ = userRep.DataFilter.Enable(FilterNames.Delete);

            UserEntity user = null;
            switch (input.AccountType)
            {
                case AccountType.UserName:
                    {
                        if (input.UserName.IsNull())
                        {
                            throw ResultOutput.Exception(_adminLocalizer["�������˺�"]);
                        }
                        user = await userRep.Select.Where(a => a.UserName == input.UserName).ToOneAsync();
                        break;
                    }

                case AccountType.Mobile:
                    {
                        if (input.Mobile.IsNull())
                        {
                            throw ResultOutput.Exception(_adminLocalizer["�������ֻ���"]);
                        }
                        user = await userRep.Select.Where(a => a.Mobile == input.Mobile).ToOneAsync();
                        break;
                    }

                case AccountType.Email:
                    {
                        if (input.Email.IsNull())
                        {
                            throw ResultOutput.Exception(_adminLocalizer["�����������ַ"]);
                        }
                        user = await userRep.Select.Where(a => a.Email == input.Email).ToOneAsync();
                        break;
                    }
            }

            var valid = user?.Id > 0;
            if (!valid)
            {
                throw ResultOutput.Exception(_adminLocalizer["�˺Ų�����"]);
            }

            if (valid)
            {
                if (user.PasswordEncryptType == PasswordEncryptType.PasswordHasher)
                {
                    var passwordVerificationResult = _passwordHasher.Value.VerifyHashedPassword(user, user.Password, input.Password);
                    valid = passwordVerificationResult == PasswordVerificationResult.Success || passwordVerificationResult == PasswordVerificationResult.SuccessRehashNeeded;
                }
                else
                {
                    var password = MD5Encrypt.Encrypt32(input.Password);
                    valid = user.Password == password;
                }
            }

            if (!valid)
            {
                throw ResultOutput.Exception(_adminLocalizer["�˺Ż��������"]);
            }

            if (!user.Enabled)
            {
                throw ResultOutput.Exception(_adminLocalizer["�˺���ͣ�ã���ֹ��¼"]);
            }
            #endregion

            loginLogAddInput.Name = user.Name;
            loginLogAddInput.CreatedUserId = user.Id;
            loginLogAddInput.CreatedUserName = user.UserName;
            loginLogAddInput.CreatedUserRealName = user.Name;

            #region ���token
            var authLoginOutput = Mapper.Map<AuthLoginOutput>(user);
            if (_appConfig.Value.Tenant)
            {
                var tenant = await _tenantRep.Value.Select.WhereDynamic(user.TenantId).ToOneAsync<AuthLoginTenantModel>();
                if (!(tenant != null && tenant.Enabled))
                {
                    throw ResultOutput.Exception(_adminLocalizer["��ҵ��ͣ�ã���ֹ��¼"]);
                }
                authLoginOutput.Tenant = tenant;
            } 
            
            var tokenInfo = GetTokenInfo(authLoginOutput);
            #endregion

            loginLogAddInput.TenantId = authLoginOutput.TenantId;

            //��������¼��Ϣ
            await UpdateLastLoginInfoAsync(user.Id, ip, locationInfo);

            return tokenInfo;
        }
        catch (Exception ex)
        {
            loginLogAddInput.Status = false;
            loginLogAddInput.Msg = ex.Message;

            throw;
        }
        finally
        {
            stopwatch.Stop();
            loginLogAddInput.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            await AddLoginLogAsync(loginLogAddInput);
        }
    }

    /// <summary>
    /// �ֻ���¼
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<TokenInfo> MobileLoginAsync(AuthMobileLoginInput input)
    {
        var stopwatch = Stopwatch.StartNew();

        var ip = IPHelper.GetIP(AppInfo.HttpContext?.Request);
        var locationInfo = GetIpLocationInfo(ip);
        var loginLogAddInput = new LoginLogAddInput
        {
            Status = true,
            IP = ip,
            Country = locationInfo?.Country,
            Province = locationInfo?.Province,
            City = locationInfo?.City,
            Isp = locationInfo?.Isp,
            CreatedUserName = input.Mobile
        };

        try
        {
            var userRep = _userRep.Value;

            using var _ = userRep.DataFilter.DisableAll();
            using var __ = userRep.DataFilter.Enable(FilterNames.Delete);

            #region ������֤����֤
            if (input.CodeId.IsNull() || input.Code.IsNull())
            {
                throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
            }
            var codeKey = CacheKeys.GetSmsCodeKey(input.Mobile, input.CodeId);
            var code = await Cache.GetAsync(codeKey);
            if (code.IsNull())
            {
                throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
            }
            await Cache.DelAsync(codeKey);
            if (code != input.Code)
            {
                throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
            }

            #endregion

            #region ��¼
            var user = await userRep.Select.Where(a => a.Mobile == input.Mobile).ToOneAsync();
            if (!(user?.Id > 0))
            {
                throw ResultOutput.Exception(_adminLocalizer["�˺Ų�����"]);
            }

            if (!user.Enabled)
            {
                throw ResultOutput.Exception(_adminLocalizer["�˺���ͣ�ã���ֹ��¼"]);
            }
            #endregion

            loginLogAddInput.Name = user.Name;
            loginLogAddInput.CreatedUserId = user.Id;
            loginLogAddInput.CreatedUserName = user.UserName;
            loginLogAddInput.CreatedUserRealName = user.Name;

            #region ���token
            var authLoginOutput = Mapper.Map<AuthLoginOutput>(user);
            if (_appConfig.Value.Tenant)
            {
                var tenant = await _tenantRep.Value.Select.WhereDynamic(user.TenantId).ToOneAsync<AuthLoginTenantModel>();
                if (!(tenant != null && tenant.Enabled))
                {
                    throw ResultOutput.Exception(_adminLocalizer["��ҵ��ͣ�ã���ֹ��¼"]);
                }
                authLoginOutput.Tenant = tenant;
            }
            var tokenInfo = GetTokenInfo(authLoginOutput);
            #endregion

            loginLogAddInput.TenantId = authLoginOutput.TenantId;

            //��������¼��Ϣ
            await UpdateLastLoginInfoAsync(user.Id, ip, locationInfo);

            stopwatch.Stop();

            await AddLoginLogAsync(GetLoginLogAddInput(authLoginOutput, locationInfo, user, stopwatch));

            return tokenInfo;
        }
        catch (Exception ex)
        {
            loginLogAddInput.Status = false;
            loginLogAddInput.Msg = ex.Message;

            throw;
        }
        finally
        {
            stopwatch.Stop();
            loginLogAddInput.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            await AddLoginLogAsync(loginLogAddInput);
        }
    }

    /// <summary>
    /// �����¼
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task<TokenInfo> EmailLoginAsync(AuthEmailLoginInput input)
    {
        var stopwatch = Stopwatch.StartNew();

        var ip = IPHelper.GetIP(AppInfo.HttpContext?.Request);
        var locationInfo = GetIpLocationInfo(ip);
        var loginLogAddInput = new LoginLogAddInput
        {
            Status = true,
            IP = ip,
            Country = locationInfo?.Country,
            Province = locationInfo?.Province,
            City = locationInfo?.City,
            Isp = locationInfo?.Isp,
            CreatedUserName = input.Email
        };

        try
        {
            var userRep = _userRep.Value;

            using var _ = userRep.DataFilter.DisableAll();
            using var __ = userRep.DataFilter.Enable(FilterNames.Delete);

            #region ������֤����֤
            if (input.CodeId.IsNull() || input.Code.IsNull())
            {
                throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
            }
            var codeKey = CacheKeys.GetEmailCodeKey(input.Email, input.CodeId);
            var code = await Cache.GetAsync(codeKey);
            if (code.IsNull())
            {
                throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
            }
            await Cache.DelAsync(codeKey);
            if (code != input.Code)
            {
                throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
            }

            #endregion

            #region ��¼
            var user = await userRep.Select.Where(a => a.Email == input.Email).ToOneAsync();
            if (!(user?.Id > 0))
            {
                throw ResultOutput.Exception(_adminLocalizer["�˺Ų�����"]);
            }

            if (!user.Enabled)
            {
                throw ResultOutput.Exception(_adminLocalizer["�˺���ͣ�ã���ֹ��¼"]);
            }
            #endregion

            loginLogAddInput.Name = user.Name;
            loginLogAddInput.CreatedUserId = user.Id;
            loginLogAddInput.CreatedUserName = user.UserName;
            loginLogAddInput.CreatedUserRealName = user.Name;

            #region ���token
            var authLoginOutput = Mapper.Map<AuthLoginOutput>(user);
            if (_appConfig.Value.Tenant)
            {
                var tenant = await _tenantRep.Value.Select.WhereDynamic(user.TenantId).ToOneAsync<AuthLoginTenantModel>();
                if (!(tenant != null && tenant.Enabled))
                {
                    throw ResultOutput.Exception(_adminLocalizer["��ҵ��ͣ�ã���ֹ��¼"]);
                }
                authLoginOutput.Tenant = tenant;
            }
            var tokenInfo = GetTokenInfo(authLoginOutput);
            #endregion

            loginLogAddInput.TenantId = authLoginOutput.TenantId;

            //��������¼��Ϣ
            await UpdateLastLoginInfoAsync(user.Id, ip, locationInfo);

            stopwatch.Stop();

            await AddLoginLogAsync(GetLoginLogAddInput(authLoginOutput, locationInfo, user, stopwatch));

            return tokenInfo;
        }
        catch (Exception ex)
        {
            loginLogAddInput.Status = false;
            loginLogAddInput.Msg = ex.Message;

            throw;
        }
        finally
        {
            stopwatch.Stop();
            loginLogAddInput.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            await AddLoginLogAsync(loginLogAddInput);
        }
    }

    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task ChangePasswordByEmailAsync(AuthChangePasswordByEmailInput input)
    {
        if (input.ConfirmPassword.NotNull() && input.ConfirmPassword != input.NewPassword)
        {
            throw ResultOutput.Exception(_adminLocalizer["�������ȷ�����벻һ��"]);
        }

        //��������ʽ
        _userHelper.CheckPassword(input.NewPassword);

        #region ������֤����֤

        if (input.Email.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["�����������ַ"]);
        }

        if (input.CodeId.IsNull() || input.Code.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
        }
        var codeKey = CacheKeys.GetEmailCodeKey(input.Email, input.CodeId);
        var code = await Cache.GetAsync(codeKey);
        if (code.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
        }
        await Cache.DelAsync(codeKey);
        if (code != input.Code)
        {
            throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
        }

        #endregion

        var userRep = _userRep.Value;
        using var _ = userRep.DataFilter.DisableAll();
        using var __ = userRep.DataFilter.Enable(FilterNames.Delete);

        UserEntity user = await userRep.Select.Where(a => a.Email == input.Email).ToOneAsync();

        if (user == null) 
        {
            throw ResultOutput.Exception(_adminLocalizer["�˺Ų�����"]);
        }

        if (user.PasswordEncryptType == PasswordEncryptType.PasswordHasher)
        {
            user.Password = _passwordHasher.Value.HashPassword(user, input.NewPassword);
        }
        else
        {
            user.Password = MD5Encrypt.Encrypt32(input.NewPassword);
        }

        await userRep.UpdateAsync(user);
    }

    /// <summary>
    /// �ֻ���������
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task ChangePasswordByMobileAsync(AuthChangePasswordByMobileInput input)
    {
        if (input.ConfirmPassword.NotNull() && input.ConfirmPassword != input.NewPassword)
        {
            throw ResultOutput.Exception(_adminLocalizer["�������ȷ�����벻һ��"]);
        }

        //��������ʽ
        _userHelper.CheckPassword(input.NewPassword);

        #region ������֤����֤

        if (input.Mobile.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["�������ֻ���"]);
        }

        if (input.CodeId.IsNull() || input.Code.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
        }
        var codeKey = CacheKeys.GetSmsCodeKey(input.Mobile, input.CodeId);
        var code = await Cache.GetAsync(codeKey);
        if (code.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
        }
        await Cache.DelAsync(codeKey);
        if (code != input.Code)
        {
            throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
        }

        #endregion

        var userRep = _userRep.Value;
        using var _ = userRep.DataFilter.DisableAll();
        using var __ = userRep.DataFilter.Enable(FilterNames.Delete);

        UserEntity user = await userRep.Select.Where(a => a.Mobile == input.Mobile).ToOneAsync();

        if (user == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["�˺Ų�����"]);
        }

        if (user.PasswordEncryptType == PasswordEncryptType.PasswordHasher)
        {
            user.Password = _passwordHasher.Value.HashPassword(user, input.NewPassword);
        }
        else
        {
            user.Password = MD5Encrypt.Encrypt32(input.NewPassword);
        }

        await userRep.UpdateAsync(user);
    }

    /// <summary>
    /// ����ע��
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task RegByEmailAsync(AuthRegByEmailInput input)
    {
        //��������ʽ
        if (input.Password.NotNull())
        {
            _userHelper.CheckPassword(input.Password);
        }

        #region ������֤����֤

        if (input.Email.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["�����������ַ"]);
        }

        if (input.CodeId.IsNull() || input.Code.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
        }
        var codeKey = CacheKeys.GetEmailCodeKey(input.Email, input.CodeId);
        var code = await Cache.GetAsync(codeKey);
        if (code.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
        }
        await Cache.DelAsync(codeKey);
        if (code != input.Code)
        {
            throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
        }

        #endregion

        await _tenantService.Value.RegAsync(new TenantRegInput
        {
            Name = input.CorpName,
            UserName = input.Email,
            Email = input.Email,
            Password = input.Password,
            Enabled = true,
        });
    }

    /// <summary>
    /// �ֻ���ע��
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOperationLog]
    public async Task RegByMobileAsync(AuthRegByMobileInput input)
    {
        //��������ʽ
        if (input.Password.NotNull())
        {
            _userHelper.CheckPassword(input.Password);
        }

        #region ������֤����֤

        if (input.Mobile.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["�������ֻ���"]);
        }

        if (input.CodeId.IsNull() || input.Code.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
        }
        var codeKey = CacheKeys.GetSmsCodeKey(input.Mobile, input.CodeId);
        var code = await Cache.GetAsync(codeKey);
        if (code.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
        }
        await Cache.DelAsync(codeKey);
        if (code != input.Code)
        {
            throw ResultOutput.Exception(_adminLocalizer["��֤�����"]);
        }

        #endregion

        await _tenantService.Value.RegAsync(new TenantRegInput
        {
            Name = input.CorpName,
            UserName = input.Mobile,
            Mobile = input.Mobile,
            Password = input.Password,
            Enabled = true,
        });
    }

    /// <summary>
    /// ˢ��Token
    /// �Ծɻ���
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<TokenInfo> Refresh([BindRequired] string token)
    {
        var jwtSecurityToken = _userToken.Decode(token);
        var userClaims = jwtSecurityToken?.Claims?.ToArray();
        if (userClaims == null || userClaims.Length == 0)
        {
            throw ResultOutput.Exception(_adminLocalizer["�޷�����token"]);
        }

        var refreshExpires = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.RefreshExpires)?.Value;
        if (refreshExpires.IsNull() || refreshExpires.ToLong() <= DateTime.Now.ToTimestamp())
        {
            throw ResultOutput.Exception(_adminLocalizer["��¼��Ϣ�ѹ���"]);
        }

        var userId = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.UserId)?.Value;
        if (userId.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["��¼��Ϣ��ʧЧ"]);
        }

        //��ǩ
        var securityKey = _jwtConfig.Value.Value.SecurityKey;
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey)), SecurityAlgorithms.HmacSha256);
        var input = jwtSecurityToken.RawHeader + "." + jwtSecurityToken.RawPayload;
        if (jwtSecurityToken.RawSignature != JwtTokenUtilities.CreateEncodedSignature(input, signingCredentials))
        {
            throw ResultOutput.Exception(_adminLocalizer["��ǩʧ��"]);
        }

        var user = await _userService.GetLoginUserAsync(userId.ToLong());
        if(!(user?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["�˺Ų�����"]);
        }
        if (!user.Enabled)
        {
            throw ResultOutput.Exception(_adminLocalizer["�˺���ͣ�ã���ֹ��¼"]);
        }

        if (_appConfig.Value.Tenant)
        {
            if (!(user.Tenant != null && user.Tenant.Enabled))
            {
                throw ResultOutput.Exception(_adminLocalizer["��ҵ��ͣ�ã���ֹ��¼"]);
            }
        }

        var tokenInfo = GetTokenInfo(user);
        return tokenInfo;
    }

    /// <summary>
    /// �Ƿ�����֤��
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [NoOperationLog]
    public bool IsCaptcha()
    {
        return _appConfig.Value.VarifyCode.Enable;
    }
}
