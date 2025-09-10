using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ChipSys.Admin.Core;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Configs;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Helpers;
using ChipSys.Admin.Domain.Api;
using ChipSys.Admin.Domain.PermissionApi;
using ChipSys.Admin.Domain.Role;
using ChipSys.Admin.Domain.RolePermission;
using ChipSys.Admin.Domain.Tenant;
using ChipSys.Admin.Domain.User;
using ChipSys.Admin.Domain.UserRole;
using ChipSys.Admin.Domain.UserStaff;
using ChipSys.Admin.Domain.Org;
using ChipSys.Admin.Services.Auth;
using ChipSys.Admin.Services.Auth.Dto;
using ChipSys.Admin.Services.User.Dto;
using ChipSys.Common.Helpers;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Admin.Domain.User.Dto;
using ChipSys.Admin.Domain.RoleOrg;
using ChipSys.Admin.Domain.UserOrg;
using ChipSys.Admin.Domain.PkgPermission;
using ChipSys.Admin.Domain.TenantPkg;
using FreeSql;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Domain.Permission;
using Microsoft.Extensions.Options;
using ChipSys.Admin.Core.Db;
using DotNetCore.CAP;
using ChipSys.Admin.Services.User.Events;
using Mapster;
using ChipSys.Admin.Services.Org;
using ChipSys.Admin.Core.Auth;

namespace ChipSys.Admin.Services.User;

/// <summary>
/// �û�����
/// </summary>
[Order(10)]
[DynamicApi(Area = AdminConsts.AreaName)]
public partial class UserService : BaseService, IUserService, IDynamicApi
{
    private readonly AppConfig _appConfig;
    private readonly ImConfig _imConfig;
    private readonly UserHelper _userHelper;
    private readonly AdminLocalizer _adminLocalizer;
    private readonly IUserRepository _userRep;
    private readonly IUserOrgRepository _userOrgRep;
    private readonly IUserRoleRepository _userRoleRep;
    private readonly IUserStaffRepository _userStaffRep;
    private readonly Lazy<IPasswordHasher<UserEntity>> _passwordHasher;
    private readonly Lazy<IRoleRepository> _roleRep;
    private readonly Lazy<IRolePermissionRepository> _rolePermissionRep;
    private readonly Lazy<IFileService> _fileService;
    private readonly Lazy<IRoleOrgRepository> _roleOrgRep;
    private readonly Lazy<IApiRepository> _apiRep;
    private readonly Lazy<ITenantRepository> _tenantRep;
    private readonly Lazy<IOrgRepository> _orgRep;
    private readonly Lazy<IPermissionRepository> _permissionRep;
    private readonly OrgService _orgService;

    public UserService(
        IOptions<AppConfig> appConfig,
        IOptions<ImConfig> imConfig,
        UserHelper userHelper,
        AdminLocalizer adminLocalizer,
        IUserRepository userRep,
        IUserOrgRepository userOrgRep,
        IUserRoleRepository userRoleRep,
        IUserStaffRepository userStaffRep,
        Lazy<IPasswordHasher<UserEntity>> passwordHasher,
        Lazy<IRoleRepository> roleRep,
        Lazy<IRolePermissionRepository> rolePermissionRep,
        Lazy<IFileService> fileService,
        Lazy<IRoleOrgRepository> roleOrgRep,
        Lazy<IApiRepository> apiRep,
        Lazy<ITenantRepository> tenantRep,
        Lazy<IOrgRepository> orgRep,
        Lazy<IPermissionRepository> permissionRep,
        OrgService orgService
    )
    {
        _appConfig = appConfig.Value;
        _imConfig = imConfig.Value;
        _userHelper = userHelper;
        _adminLocalizer = adminLocalizer;
        _userRep = userRep;
        _userOrgRep = userOrgRep;
        _userRoleRep = userRoleRep;
        _userStaffRep = userStaffRep;
        _passwordHasher = passwordHasher;
        _roleRep = roleRep;
        _rolePermissionRep = rolePermissionRep;
        _fileService = fileService;
        _roleOrgRep = roleOrgRep;
        _apiRep = apiRep;
        _tenantRep = tenantRep;
        _orgRep = orgRep;
        _permissionRep = permissionRep;
        _orgService = orgService;
    }

    /// <summary>
    /// ��ѯ�û�
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UserGetOutput> GetAsync(long id)
    {
        var userEntity = await _userRep.Select
        .WhereDynamic(id)
        .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Id = b.Id, Name = b.Name }))
        .ToOneAsync(a => new
        {
            a.Id,
            a.UserName,
            a.Name,
            a.Mobile,
            a.Email,
            a.Roles,
            a.ManagerUserId,
            ManagerUserName = a.ManagerUser.Name,
            Staff = new
            {
                a.Staff.JobNumber,
                a.Staff.Sex,
                a.Staff.Position,
                a.Staff.Introduce
            }
        });

        var output = Mapper.Map<UserGetOutput>(userEntity);

        return output;
    }

    /// <summary>
    /// ��ѯ��ҳ
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<UserGetPageOutput>> GetPageAsync(PageInput<UserGetPageInput> input)
    {
        var dataPermission = User.DataPermission;

        var orgId = input.Filter?.OrgId;
        var list = await _userRep.Select
        .WhereIf(dataPermission != null && dataPermission.OrgIds.Count > 0, a => _userOrgRep.Where(b => b.UserId == a.Id && dataPermission.OrgIds.Contains(b.OrgId)).Any())
        .WhereIf(dataPermission != null && dataPermission.DataScope == DataScope.Self, a => a.CreatedUserId == User.Id)
        .WhereIf(orgId.HasValue && orgId > 0, a => _userOrgRep.Where(b => b.UserId == a.Id && b.OrgId == orgId).Any())
        .Where(a => a.Type != UserType.Member)
        .WhereDynamicFilter(input.DynamicFilter)
        .Count(out var total)
        .OrderByDescending(true, a => a.Id)
        .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Name = b.Name }))
        .IncludeMany(a => a.Orgs.Select(b => new OrgEntity { Id = b.Id }))
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync(a => new UserGetPageOutput
        {
            Roles = a.Roles,
            Orgs = a.Orgs,
            Sex = a.Staff.Sex
        });

        //���ò���
        var orgs = await _orgService.GetSimpleListWithPathAsync();

        var orgDict = orgs.ToDictionary(a => a.Id, a => a.Path);
        foreach (var user in list)
        {
            if (user.OrgId > 0 && orgDict.TryGetValue(user.OrgId, out var path))
            {
                user.OrgPath = path;
            }

            if(user.OrgIds.Length > 0)
            {
                var orgPathList = user.OrgIds.Select(a => orgDict.GetValueOrDefault(a)).Where(a => a != null).ToList();
                user.OrgPaths = string.Join(" ; ", orgPathList);
            }
        }

        //��������
        if (orgId.HasValue && orgId > 0)
        {
            var managerUserIds = await _userOrgRep.Select
                .Where(a => a.OrgId == orgId && a.IsManager == true).ToListAsync(a => a.UserId);

            if (managerUserIds.Count != 0)
            {
                var managerUsers = list.Where(a => managerUserIds.Contains(a.Id));
                foreach (var user in managerUsers)
                {
                    user.IsManager = true;
                }
            }
        }

        //�����û�����
        if (_imConfig.Enable)
        {
            var clientIdList = ImHelper.GetClientListByOnline();
            if (clientIdList.Any())
            {
                var onlineUsers = list.Where(a => clientIdList.Contains(a.Id));
                foreach (var user in onlineUsers)
                {
                    user.Online = true;
                }
            }
        }

        var data = new PageOutput<UserGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// ��ѯ��ɾ����ҳ�б�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<UserGetDeletedUserPageOutput>> GetDeletedPageAsync(PageInput input)
    {
        var dataPermission = User.DataPermission;

        var list = await _userRep.Select.DisableGlobalFilter(FilterNames.Delete)
        .Where(a => a.IsDeleted == true)
        .WhereIf(dataPermission != null && dataPermission.OrgIds.Count > 0, a => _userOrgRep.Where(b => b.UserId == a.Id && dataPermission.OrgIds.Contains(b.OrgId)).Any())
        .WhereIf(dataPermission != null && dataPermission.DataScope == DataScope.Self, a => a.CreatedUserId == User.Id)
        .Where(a => a.Type != UserType.Member)
        .WhereDynamicFilter(input.DynamicFilter)
        .Count(out var total)
        .OrderByDescending(true, a => a.Id)
        .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Name = b.Name }))
        .IncludeMany(a => a.Orgs.Select(b => new OrgEntity { Id = b.Id }))
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync(a => new UserGetDeletedUserPageOutput
        {
            Roles = a.Roles,
            Orgs = a.Orgs,
            Sex = a.Staff.Sex
        });

        //���ò���
        var orgs = await _orgService.GetSimpleListWithPathAsync();

        var orgDict = orgs.ToDictionary(a => a.Id, a => a.Path);
        foreach (var user in list)
        {
            if (user.OrgId > 0 && orgDict.TryGetValue(user.OrgId, out var path))
            {
                user.OrgPath = path;
            }

            if (user.OrgIds.Length > 0)
            {
                var orgPathList = user.OrgIds.Select(a => orgDict.GetValueOrDefault(a)).Where(a => a != null).ToList();
                user.OrgPaths = string.Join(" ; ", orgPathList);
            }
        }

        var data = new PageOutput<UserGetDeletedUserPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// ��ѯ��¼�û���Ϣ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<AuthLoginOutput> GetLoginUserAsync(long id)
    {
        var output = await _userRep.Select
            .DisableGlobalFilter(FilterNames.Tenant)
            .WhereDynamic(id)
            .ToOneAsync<AuthLoginOutput>();

        if (_appConfig.Tenant && output?.TenantId.Value > 0)
        {
            var tenant = await _tenantRep.Value.Select
                .DisableGlobalFilter(FilterNames.Tenant)
                .WhereDynamic(output.TenantId)
                .ToOneAsync<AuthLoginTenantModel>();

            output.Tenant = tenant;
        }
        return output;
    }

    /// <summary>
    /// �������Ȩ��
    /// </summary>
    /// <param name="apiPath"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataPermissionOutput> GetDataPermissionAsync(string? apiPath)
    {
        if (!(User?.Id > 0))
        {
            return null;
        }

        return await Cache.GetOrSetAsync(CacheKeys.GetDataPermissionKey(User.Id, apiPath), async () =>
        {
            using var _ = _userRep.DataFilter.Disable(FilterNames.Self, FilterNames.Data);

            var user = await _userRep.Select
            .WhereDynamic(User.Id)
            .ToOneAsync(a => new { a.OrgId });

            if (user == null)
                return null;

            var orgId = user.OrgId;

            //��ѯ��ɫ
            var roleRepository = _roleRep.Value;
            var rolePermissionRepository = _rolePermissionRep.Value;
            var roles = await roleRepository.Select
            .InnerJoin<UserRoleEntity>((a, b) => a.Id == b.RoleId && b.UserId == User.Id)
            .WhereIf(apiPath.NotNull(), r => rolePermissionRepository.Select
                .From<PermissionApiEntity, ApiEntity>((s, b, c) =>
                    s.InnerJoin(a => a.PermissionId == b.PermissionId)
                    .InnerJoin(a => b.ApiId == c.Id && c.Path == apiPath)
                ).ToList((a, b, c) => a.RoleId).Contains(r.Id))
            .ToListAsync(a => new { a.Id, a.DataScope });

            //���ݷ�Χ
            DataScope dataScope = DataScope.Self;
            var customRoleIds = new List<long>();
            roles?.ToList().ForEach(role =>
            {
                if (role.DataScope == DataScope.Custom)
                {
                    customRoleIds.Add(role.Id);
                }
                else if (role.DataScope <= dataScope)
                {
                    dataScope = role.DataScope;
                }
            });

            //�����б�
            var orgIds = new List<long>();
            if (dataScope != DataScope.All)
            {
                //������
                if (dataScope == DataScope.Dept)
                {
                    orgIds.Add(orgId);
                }
                //�����ź��¼�����
                else if (dataScope == DataScope.DeptWithChild)
                {
                    orgIds = await _orgRep.Value
                    .Where(a => a.Id == orgId)
                    .AsTreeCte()
                    .ToListAsync(a => a.Id);
                }

                //ָ������
                if (customRoleIds.Count > 0)
                {
                    if (dataScope == DataScope.Self)
                    {
                        dataScope = DataScope.Custom;
                    }

                    var customRoleOrgIds = await _roleOrgRep.Value.Select
                    .Where(a => customRoleIds.Contains(a.RoleId))
                    .ToListAsync(a => a.OrgId);

                    orgIds = orgIds.Concat(customRoleOrgIds).ToList();
                }
            }
            var orgName = await _orgRep.Value
                         .Where(a => a.Id == orgId)
                         .ToOneAsync(a => a.Name);

            return new DataPermissionOutput
            {
                OrgName = orgName,
                OrgId = orgId,
                OrgIds = orgIds.Distinct().ToList(),
                DataScope = (User.PlatformAdmin || User.TenantAdmin) ? DataScope.All : dataScope
            };
        });
    }

    /// <summary>
    /// ��ѯ�û�������Ϣ
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<UserGetBasicOutput> GetBasicAsync()
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["δ��¼"]);
        }

        var user = await _userRep.GetAsync<UserGetBasicOutput>(User.Id);

        if (user == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["�û�������"]);
        }

        user.Mobile = DataMaskHelper.PhoneMask(user.Mobile);
        user.Email = DataMaskHelper.EmailMask(user.Email);

        return user;
    }

    /// <summary>
    /// ��ѯ�û�Ȩ����Ϣ
    /// </summary>
    /// <returns></returns>
    public async Task<UserGetPermissionOutput> GetPermissionAsync()
    {
        var result = await Cache.GetOrSetAsync(CacheKeys.GetUserPermissionKey(User.Id), async () =>
        {
            var output = new UserGetPermissionOutput();
            if (User.TenantAdmin)
            {
                var cloud = LazyGetRequiredService<FreeSqlCloud>();
                var db = cloud.Use(DbKeys.AdminDb);

                //�ײͽӿ�
                var pkgApis = await db.Select<ApiEntity>()
                .Where(a => db.Select<TenantPkgEntity, PkgPermissionEntity, PermissionApiEntity>()
                    .InnerJoin((b, c, d) => b.PkgId == c.PkgId && c.PermissionId == d.PermissionId && b.TenantId == User.TenantId)
                    .Where((b, c, d) => d.ApiId == a.Id).Any())
                .ToListAsync<UserGetPermissionOutput.Models.ApiModel>();

                //�ײ�Ȩ�޵����
                var pkgCodes = await db.Select<PermissionEntity>()
                .Where(p => db.Select<TenantPkgEntity, PkgPermissionEntity>()
                    .InnerJoin((tp, pp) => tp.PkgId == pp.PkgId && pp.PermissionId == p.Id && tp.TenantId == User.TenantId).Any()
                    && p.Type == PermissionType.Dot && p.Code != null && p.Code != "")
                .ToListAsync(p => p.Code);

                output.Apis = pkgApis.Distinct().ToList();

                output.Codes = pkgCodes.Distinct().ToList();

                return output;
            }

            //��ɫ�ӿ�
            output.Apis = await _apiRep.Value
            .Where(a => _apiRep.Value.Orm.Select<UserRoleEntity, RolePermissionEntity, PermissionApiEntity>()
                .InnerJoin((b, c, d) => b.RoleId == c.RoleId && b.UserId == User.Id)
                .InnerJoin((b, c, d) => c.PermissionId == d.PermissionId)
                .Where((b, c, d) => d.ApiId == a.Id).Any())
            .ToListAsync<UserGetPermissionOutput.Models.ApiModel>();

            //��ɫȨ�޵����
            output.Codes = await _permissionRep.Value.Where(p => _permissionRep.Value.Orm.Select<UserRoleEntity, RolePermissionEntity>()
                .InnerJoin((ur, rp) => ur.RoleId == rp.RoleId && ur.UserId == User.Id
                && rp.PermissionId == p.Id && p.Type == PermissionType.Dot && p.Code != null && p.Code != "").Any()
            ).ToListAsync(p => p.Code);
            output.Codes = output.Codes.Distinct().ToList();

            return output;
        });
        return result;
    }

    /// <summary>
    /// �����û�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task<long> AddAsync(UserAddInput input)
    {
        //�������
        if (input.Password.IsNull())
        {
            input.Password = _appConfig.DefaultPassword;
        }
        _userHelper.CheckPassword(input.Password);

        using var _ = _userRep.DataFilter.DisableAll();

        Expression<Func<UserEntity, bool>> where = (a => a.UserName == input.UserName);
        where = where.Or(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
            .Or(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRep.Select.Where(where)
            .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception(_adminLocalizer["�˺��Ѵ���"]);
            }

            if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
            {
                throw ResultOutput.Exception(_adminLocalizer["�ֻ����Ѵ���"]);
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception(_adminLocalizer["�����Ѵ���"]);
            }
        }

        // �û���Ϣ
        var entity = Mapper.Map<UserEntity>(input);
        entity.Type = UserType.DefaultUser;
        if (_appConfig.PasswordHasher)
        {
            entity.Password = _passwordHasher.Value.HashPassword(entity, input.Password);
            entity.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
        }
        else
        {
            entity.Password = MD5Encrypt.Encrypt32(input.Password);
            entity.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
        }
        var user = await _userRep.InsertAsync(entity);
        var userId = user.Id;

        //�û���ɫ
        if (input.RoleIds != null && input.RoleIds.Any())
        {
            var roles = input.RoleIds.Select(roleId => new UserRoleEntity
            {
                UserId = userId,
                RoleId = roleId
            }).ToList();
            await _userRoleRep.InsertAsync(roles);
        }

        // Ա����Ϣ
        var staff = input.Staff == null ? new UserStaffEntity() : Mapper.Map<UserStaffEntity>(input.Staff);
        staff.Id = userId;
        await _userStaffRep.InsertAsync(staff);

        //��������
        if (input.OrgIds != null && input.OrgIds.Any())
        {
            var orgs = input.OrgIds.Select(orgId => new UserOrgEntity
            {
                UserId = userId,
                OrgId = orgId
            }).ToList();
            await _userOrgRep.InsertAsync(orgs);
        }

        return userId;
    }

    /// <summary>
    /// �޸��û�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task UpdateAsync(UserUpdateInput input)
    {
        if (input.Id == input.ManagerUserId)
        {
            throw ResultOutput.Exception(_adminLocalizer["ֱ�����ܲ������Լ�"]);
        }

        using var _ = _userRep.DataFilter.DisableAll();

        Expression<Func<UserEntity, bool>> where = (a => a.UserName == input.UserName);
        where = where.Or(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
            .Or(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRep.Select.Where(a => a.Id != input.Id).Where(where)
            .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception(_adminLocalizer["�˺��Ѵ���"]);
            }

            if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
            {
                throw ResultOutput.Exception(_adminLocalizer["�ֻ����Ѵ���"]);
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception(_adminLocalizer["�����Ѵ���"]);
            }
        }

        var user = await _userRep.GetAsync(input.Id);
        if (!(user?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["�û�������"]);
        }

        Mapper.Map(input, user);
        await _userRep.UpdateAsync(user);

        var userId = user.Id;

        // �û���ɫ
        await _userRoleRep.DeleteAsync(a => a.UserId == userId);
        if (input.RoleIds != null && input.RoleIds.Any())
        {
            var roles = input.RoleIds.Select(roleId => new UserRoleEntity
            {
                UserId = userId,
                RoleId = roleId
            }).ToList();
            await _userRoleRep.InsertAsync(roles);
        }

        // Ա����Ϣ
        var staff = await _userStaffRep.GetAsync(userId);
        var existsStaff = staff != null;
        staff ??= new UserStaffEntity();
        Mapper.Map(input.Staff, staff);
        staff.Id = userId;
        if (existsStaff)
        {
            await _userStaffRep.UpdateAsync(staff);
        }
        else
        {
            await _userStaffRep.InsertAsync(staff);
        }

        await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
    }

    /// <summary>
    /// ������Ա
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<long> AddMemberAsync(UserAddMemberInput input)
    {
        if (input.Password.IsNull())
        {
            input.Password = _appConfig.DefaultPassword;
        }
        _userHelper.CheckPassword(input.Password);

        using var _ = _userRep.DataFilter.DisableAll();

        Expression<Func<UserEntity, bool>> where = (a => a.UserName == input.UserName);
        where = where.Or(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
            .Or(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRep.Select.Where(where)
            .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception(_adminLocalizer["�˺��Ѵ���"]);
            }

            if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
            {
                throw ResultOutput.Exception(_adminLocalizer["�ֻ����Ѵ���"]);
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception(_adminLocalizer["�����Ѵ���"]);
            }
        }

        // �û���Ϣ
        var entity = Mapper.Map<UserEntity>(input);
        entity.Type = UserType.Member;
        if (_appConfig.PasswordHasher)
        {
            entity.Password = _passwordHasher.Value.HashPassword(entity, input.Password);
            entity.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
        }
        else
        {
            entity.Password = MD5Encrypt.Encrypt32(input.Password);
            entity.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
        }
        var user = await _userRep.InsertAsync(entity);

        return user.Id;
    }

    /// <summary>
    /// �޸Ļ�Ա
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task UpdateMemberAsync(UserUpdateMemberInput input)
    {
        using var _ = _userRep.DataFilter.DisableAll();

        Expression<Func<UserEntity, bool>> where = (a => a.UserName == input.UserName);
        where = where.Or(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
            .Or(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRep.Select.Where(a => a.Id != input.Id).Where(where)
            .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception(_adminLocalizer["�˺��Ѵ���"]);
            }

            if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
            {
                throw ResultOutput.Exception(_adminLocalizer["�ֻ����Ѵ���"]);
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception(_adminLocalizer["�����Ѵ���"]);
            }
        }

        var user = await _userRep.GetAsync(input.Id);
        if (!(user?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["�û�������"]);
        }

        Mapper.Map(input, user);
        await _userRep.UpdateAsync(user);
    }

    /// <summary>
    /// �����û�������Ϣ
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Login]
    public async Task UpdateBasicAsync(UserUpdateBasicInput input)
    {
        var entity = await _userRep.GetAsync(User.Id);
        entity = Mapper.Map(input, entity);
        await _userRep.UpdateAsync(entity);
    }

    /// <summary>
    /// �޸��û�����
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Login]
    public async Task ChangePasswordAsync(UserChangePasswordInput input)
    {
        if (input.ConfirmPassword != input.NewPassword)
        {
            throw ResultOutput.Exception(_adminLocalizer["�������ȷ�����벻һ��"]);
        }

        _userHelper.CheckPassword(input.NewPassword);

        var entity = await _userRep.GetAsync(User.Id);
        var oldPassword = MD5Encrypt.Encrypt32(input.OldPassword);
        if (oldPassword != entity.Password)
        {
            throw ResultOutput.Exception(_adminLocalizer["�����벻��ȷ"]);
        }

        if (entity.PasswordEncryptType == PasswordEncryptType.PasswordHasher)
        {
            entity.Password = _passwordHasher.Value.HashPassword(entity, input.NewPassword);
        }
        else
        {
            entity.Password = MD5Encrypt.Encrypt32(input.NewPassword);
        }
       
        await _userRep.UpdateAsync(entity);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<string> ResetPasswordAsync(UserResetPasswordInput input)
    {
        var password = input.Password;
        if (password.IsNull())
        {
            password = _appConfig.DefaultPassword;
        }
        else
        {
            _userHelper.CheckPassword(password);
        }
        if (password.IsNull())
        {
            password = "123asd";
        }

        var entity = await _userRep.GetAsync(input.Id);
        if (entity.PasswordEncryptType == PasswordEncryptType.PasswordHasher)
        {
            entity.Password = _passwordHasher.Value.HashPassword(entity, password);
        }
        else
        {
            entity.Password = MD5Encrypt.Encrypt32(password);
        }
        await _userRep.UpdateAsync(entity);
        return password;
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetManagerAsync(UserSetManagerInput input)
    {
        var entity = await _userOrgRep.Where(a => a.UserId == input.UserId && a.OrgId == input.OrgId).FirstAsync();
        entity.IsManager = input.IsManager;
        await _userOrgRep.UpdateAsync(entity);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetEnableAsync(UserSetEnableInput input)
    {
        var entity = await _userRep.GetAsync(input.UserId);
        if (entity.Type == UserType.PlatformAdmin)
        {
            throw ResultOutput.Exception(_adminLocalizer["ƽ̨����Ա��ֹ����"]);
        }
        if (entity.Type == UserType.TenantAdmin)
        {
            throw ResultOutput.Exception(_adminLocalizer["��ҵ����Ա��ֹ����"]);
        }
        entity.Enabled = input.Enabled;
        await _userRep.UpdateAsync(entity);
    }

    /// <summary>
    /// �ָ�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task RestoreAsync(UserRestoreInput input)
    {
        await _userRep.UpdateDiy.DisableGlobalFilter(FilterNames.Delete).Set(a => new UserEntity
        {
            IsDeleted = false,
            ModifiedUserId = User.Id,
            ModifiedUserName = User.UserName,
            ModifiedUserRealName = User.Name,
            ModifiedTime = DbHelper.ServerTime
        })
       .WhereDynamic(input.UserIds)
       .ExecuteAffrowsAsync();
    }

    /// <summary>
    /// �������ò���
    /// </summary>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSetOrgAsync(UserBatchSetOrgInput input)
    {
        //��������
        await _userRep.UpdateDiy.Set(a => new UserEntity
        {
            OrgId = input.OrgId,
            ModifiedUserId = User.Id,
            ModifiedUserName = User.UserName,
            ModifiedUserRealName = User.Name,
            ModifiedTime = DbHelper.ServerTime,
        })
       .Where(a => a.Id.In(input.UserIds))
       .ExecuteAffrowsAsync();

        //��������
        var orgIds = await _userOrgRep.Select.Where(a => a.UserId.In(input.UserIds)).ToListAsync(a => a.OrgId);
        var insertOrgIds = input.OrgIds.Except(orgIds);

        var deleteOrgIds = orgIds.Except(input.OrgIds).ToArray();
        if (deleteOrgIds != null && deleteOrgIds.Any())
        {
            await _userOrgRep.DeleteAsync(a => a.UserId.In(input.UserIds) && a.OrgId.In(deleteOrgIds));
        }

        if (insertOrgIds != null && insertOrgIds.Any())
        {
            var orgs = new List<UserOrgEntity>();
            foreach (var userId in input.UserIds)
            {
                orgs.AddRange(insertOrgIds.Select(orgId => new UserOrgEntity
                {
                    UserId = userId,
                    OrgId = orgId
                }).ToList());
            }
          
            await _userOrgRep.InsertAsync(orgs);
        }

        var capPublisher= AppInfo.GetRequiredService<ICapPublisher>();
        //���Ͳ���ת��
        var userOrgChangeEvent = input.Adapt<UserOrgChangeEvent>();
        await capPublisher.PublishAsync(SubscribeNames.UserOrgChange, userOrgChangeEvent);
    }

    /// <summary>
    /// ����ɾ���û�
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var user = await _userRep.Select.WhereDynamic(id).ToOneAsync(a => new { a.Type });
        if (user == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["�û�������"]);
        }

        if (user.Type == UserType.PlatformAdmin)
        {
            throw ResultOutput.Exception(_adminLocalizer["ƽ̨����Ա��ֹɾ��"]);
        }

        if (user.Type == UserType.TenantAdmin)
        {
            throw ResultOutput.Exception(_adminLocalizer["��ҵ����Ա��ֹɾ��"]);
        }

        //ɾ���û���ɫ
        await _userRoleRep.DeleteAsync(a => a.UserId == id);
        //ɾ���û���������
        await _userOrgRep.DeleteAsync(a => a.UserId == id);
        //ɾ��Ա��
        await _userStaffRep.DeleteAsync(a => a.Id == id);
        //ɾ���û�
        await _userRep.DeleteAsync(a => a.Id == id);

        //ɾ���û�����Ȩ�޻���
        await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(id));
    }

    /// <summary>
    /// ��������ɾ���û�
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        var admin = await _userRep.Select.Where(a => ids.Contains(a.Id) &&
        (a.Type == UserType.PlatformAdmin || a.Type == UserType.TenantAdmin)).AnyAsync();

        if (admin)
        {
            throw ResultOutput.Exception(_adminLocalizer["ƽ̨����Ա��ֹɾ��"]);
        }

        //ɾ���û���ɫ
        await _userRoleRep.DeleteAsync(a => ids.Contains(a.UserId));
        //ɾ���û���������
        await _userOrgRep.DeleteAsync(a => ids.Contains(a.UserId));
        //ɾ��Ա��
        await _userStaffRep.DeleteAsync(a => ids.Contains(a.Id));
        //ɾ���û�
        await _userRep.DeleteAsync(a => ids.Contains(a.Id));

        foreach (var userId in ids)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }

    /// <summary>
    /// ɾ���û�
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        var user = await _userRep.Select.WhereDynamic(id).ToOneAsync(a => new { a.Type });
        if (user == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["�û�������"]);
        }

        if (user.Type == UserType.PlatformAdmin || user.Type == UserType.TenantAdmin)
        {
            throw ResultOutput.Exception(_adminLocalizer["ƽ̨����Ա��ֹɾ��"]);
        }

        await _userRep.SoftDeleteAsync(id);

        await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(id));
    }

    /// <summary>
    /// ����ɾ���û�
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        var admin = await _userRep.Select.Where(a => ids.Contains(a.Id) &&
        (a.Type == UserType.PlatformAdmin || a.Type == UserType.TenantAdmin)).AnyAsync();

        if (admin)
        {
            throw ResultOutput.Exception(_adminLocalizer["ƽ̨����Ա��ֹɾ��"]);
        }

        await _userRep.SoftDeleteAsync(ids);

        foreach (var userId in ids)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }

    /// <summary>
    /// �ϴ�ͷ��
    /// </summary>
    /// <param name="file"></param>
    /// <param name="autoUpdate"></param>
    /// <returns></returns>
    [HttpPost]
    [Login]
    public async Task<string> AvatarUpload(IFormFile file, bool autoUpdate = false)
    {
        var fileInfo = await _fileService.Value.UploadFileAsync(file);
        if (autoUpdate)
        {
            var entity = await _userRep.GetAsync(User.Id);
            entity.Avatar = fileInfo.LinkUrl;
            await _userRep.UpdateAsync(entity);
        }
        return fileInfo.LinkUrl;
    }

    /// <summary>
    /// һ����¼�û�
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<TokenInfo> OneClickLoginAsync([Required] string userName)
    {
        if (userName.IsNull())
        {
            throw ResultOutput.Exception(_adminLocalizer["��ѡ���û�"]);
        }

        var userRep = _userRep;
        using var _ = userRep.DataFilter.DisableAll();
        using var __ = userRep.DataFilter.Enable(FilterNames.Tenant);

        var authLoginOutput = await userRep.Select.Where(a => a.UserName == userName).ToOneAsync<AuthLoginOutput>();

        if (authLoginOutput == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["�û�������"]);
        }

        if (_appConfig.Tenant)
        {
            var tenant = await _tenantRep.Value.Select.WhereDynamic(authLoginOutput.TenantId).ToOneAsync<AuthLoginTenantModel>();
            authLoginOutput.Tenant = tenant;
        }

        var tokenInfo = AppInfo.GetRequiredService<IAuthService>().GetTokenInfo(authLoginOutput);

        return tokenInfo;
    }

    /// <summary>
    /// ǿ���û�����
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void ForceOfflineAsync(long id)
    {
        if (_imConfig.Enable)
        {
            //������Ϣ
            ImHelper.SendMessage(0, [id], new
            {
                evts = new[]
                {
                    new { name = "forceOffline", data = new { } }
                }
            });

            //ǿ������
            ImHelper.ForceOffline(id);
        }
        else
        {
            throw ResultOutput.Exception(_adminLocalizer["�뿪��im��ʱͨѶ"]);
        }
    }
}
