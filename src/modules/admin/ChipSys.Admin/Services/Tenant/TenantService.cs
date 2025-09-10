using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Mapster;
using Yitter.IdGenerator;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Configs;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Helpers;
using ChipSys.Admin.Domain.Role;
using ChipSys.Admin.Domain.RolePermission;
using ChipSys.Admin.Domain.Tenant;
using ChipSys.Admin.Domain.User;
using ChipSys.Admin.Domain.UserRole;
using ChipSys.Admin.Domain.Org;
using ChipSys.Admin.Domain.UserStaff;
using ChipSys.Admin.Domain.UserOrg;
using ChipSys.Admin.Domain.Pkg;
using ChipSys.Admin.Domain.TenantPkg;
using ChipSys.Admin.Services.Tenant.Dto;
using ChipSys.Admin.Services.Pkg;
using ChipSys.Common.Helpers;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Core;
using ChipSys.Admin.Services.Auth.Dto;
using ChipSys.Admin.Services.Auth;
using ChipSys.Admin.Core.Validators;
using ChipSys.Admin.Core.Auth;

namespace ChipSys.Admin.Services.Tenant;

/// <summary>
/// �⻧����
/// </summary>
[Order(50)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class TenantService : BaseService, ITenantService, IDynamicApi
{
    private readonly ITenantRepository _tenantRep;
    private readonly ITenantPkgRepository _tenantPkgRep;
    private readonly IRoleRepository _roleRep;
    private readonly IUserRepository _userRep;
    private readonly IOrgRepository _orgRep;
    private readonly AppConfig _appConfig;
    private readonly Lazy<IUserRoleRepository> _userRoleRep;
    private readonly Lazy<IRolePermissionRepository> _rolePermissionRep;
    private readonly Lazy<IUserStaffRepository> _userStaffRep;
    private readonly Lazy<IUserOrgRepository> _userOrgRep;
    private readonly Lazy<IPasswordHasher<UserEntity>> _passwordHasher;
    private readonly Lazy<UserHelper> _userHelper;
    private readonly AdminLocalizer _adminLocalizer;

    public TenantService(
        ITenantRepository tenantRep,
        ITenantPkgRepository tenantPkgRep,
        IRoleRepository roleRep,
        IUserRepository userRep,
        IOrgRepository orgRep,
        IOptions<AppConfig> appConfig,
        Lazy<IUserRoleRepository> userRoleRep,
        Lazy<IRolePermissionRepository> rolePermissionRep,
        Lazy<IUserStaffRepository> userStaffRep,
        Lazy<IUserOrgRepository> userOrgRep,
        Lazy<IPasswordHasher<UserEntity>> passwordHasher,
        Lazy<UserHelper> userHelper,
        AdminLocalizer adminLocalizer
    )
    {
        _tenantRep = tenantRep;
        _tenantPkgRep = tenantPkgRep;
        _roleRep = roleRep;
        _userRep = userRep;
        _orgRep = orgRep;
        _appConfig = appConfig.Value;
        _userRoleRep = userRoleRep;
        _rolePermissionRep = rolePermissionRep;
        _userStaffRep = userStaffRep;
        _userOrgRep = userOrgRep;
        _passwordHasher = passwordHasher;
        _userHelper = userHelper;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// ��ѯ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TenantGetOutput> GetAsync(long id)
    {
        using (_tenantRep.DataFilter.Disable(FilterNames.Tenant))
        {
            var tenant = await _tenantRep.Select
            .WhereDynamic(id)
            .IncludeMany(a => a.Pkgs.Select(b => new PkgEntity { Id = b.Id, Name = b.Name }))
            .FirstAsync(a => new TenantGetOutput
            {
                Name = a.Org.Name,
                Code = a.Org.Code,
                Pkgs = a.Pkgs,
                UserName = a.User.UserName,
                RealName = a.User.Name,
                Phone = a.User.Mobile,
                Email = a.User.Email,
            });
            return tenant;
        }
    }

    /// <summary>
    /// ��ѯ��ҳ
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<TenantGetPageOutput>> GetPageAsync(PageInput<TenantGetPageInput> input)
    {
        using var _ = _tenantRep.DataFilter.Disable(FilterNames.Tenant);

        var key = input.Filter?.Name;

        var list = await _tenantRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Org.Name.Contains(key))
        .Count(out var total)
        .OrderByDescending(true, a => a.Id)
        .IncludeMany(a => a.Pkgs.Select(b => new PkgEntity { Name = b.Name }))
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync(a => new TenantGetPageOutput
        {
            Name = a.Org.Name,
            Code = a.Org.Code,
            UserName = a.User.UserName,
            RealName = a.User.Name,
            Phone = a.User.Mobile,
            Email = a.User.Email,
            Pkgs = a.Pkgs,
        });

        var data = new PageOutput<TenantGetPageOutput>()
        {
            List = Mapper.Map<List<TenantGetPageOutput>>(list),
            Total = total
        };
         
        return data;
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task<long> AddAsync(TenantAddInput input)
    {
        if (input.Password.IsNull())
        {
            input.Password = _appConfig.DefaultPassword;
        }
        _userHelper.Value.CheckPassword(input.Password);

        using var _t = _tenantRep.DataFilter.Disable(FilterNames.Tenant);
        using var _o = _orgRep.DataFilter.Disable(FilterNames.Tenant);
        using var _u = _userRep.DataFilter.Disable(FilterNames.Tenant);

        var existsOrg = await _orgRep.Select
        .Where(a => a.Name == input.Name && a.ParentId == 0)
        .WhereIf(input.Code.NotNull(), a => a.Code == input.Code)
        .FirstAsync(a => new { a.Name, a.Code });

        if (existsOrg != null)
        {
            if (existsOrg.Name == input.Name)
            {
                throw ResultOutput.Exception(_adminLocalizer["��ҵ�����Ѵ���"]);
            }

            if (input.Code.NotNull() && existsOrg.Code == input.Code)
            {
                throw ResultOutput.Exception(_adminLocalizer["��ҵ�����Ѵ���"]);
            }
        }

        Expression<Func<UserEntity, bool>> where = (a => a.UserName == input.UserName);
        where = where.Or(input.Phone.NotNull(), a => a.Mobile == input.Phone)
            .Or(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRep.Select.Where(where)
            .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception(_adminLocalizer["��ҵ�˺��Ѵ���"]);
            }

            if (input.Phone.NotNull() && existsUser.Mobile == input.Phone)
            {
                throw ResultOutput.Exception(_adminLocalizer["��ҵ�ֻ����Ѵ���"]);
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception(_adminLocalizer["��ҵ�����Ѵ���"]);
            }
        }

        //�⻧Id
        long tenantId = input.Id > 0 ? input.Id : YitIdHelper.NextId();

        //����⻧�ײ�
        if (input.PkgIds != null && input.PkgIds.Any())
        {
            var pkgs = input.PkgIds.Select(pkgId => new TenantPkgEntity
            {
                TenantId = tenantId,
                PkgId = pkgId
            }).ToList();

            await _tenantPkgRep.InsertAsync(pkgs);
        }

        //��Ӳ���
        var org = new OrgEntity
        {
            TenantId = tenantId,
            Name = input.Name,
            Code = input.Code,
            ParentId = 0,
            MemberCount = 1,
            Sort = 1,
            Enabled = true
        };
        await _orgRep.InsertAsync(org);

        //����û�
        var user = new UserEntity
        {
            TenantId = tenantId,
            UserName = input.UserName,
            Name = input.RealName,
            Mobile = input.Phone,
            Email = input.Email,
            Type = UserType.TenantAdmin,
            OrgId = org.Id,
            Enabled = true
        };
        if (_appConfig.PasswordHasher)
        {
            user.Password = _passwordHasher.Value.HashPassword(user, input.Password);
            user.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
        }
        else
        {
            user.Password = MD5Encrypt.Encrypt32(input.Password);
            user.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
        }
        await _userRep.InsertAsync(user);

        long userId = user.Id;

        //����û�Ա��
        var emp = new UserStaffEntity
        {
            Id = userId,
            TenantId = tenantId
        };
        await _userStaffRep.Value.InsertAsync(emp);

        //����û�����
        var userOrg = new UserOrgEntity
        {
            UserId = userId,
            OrgId = org.Id
        };
        await _userOrgRep.Value.InsertAsync(userOrg);

        //��ӽ�ɫ����ͽ�ɫ
        var roleId = YitIdHelper.NextId();
        var jobGroupId = YitIdHelper.NextId();
        var roles = new List<RoleEntity>
        {
            new()
            {
                Id= jobGroupId,
                ParentId = 0,
                TenantId = tenantId,
                Type = RoleType.Group,
                Name = "��λ",
                Sort = 1
            },
            new()
            {
                Id = roleId,
                TenantId = tenantId,
                Type = RoleType.Role,
                Name = "������Ա",
                Code = "main-admin",
                ParentId = jobGroupId,
                DataScope = DataScope.All,
                Sort = 1
            },
            new()
            {
                TenantId = tenantId,
                Type = RoleType.Role,
                Name = "��ͨԱ��",
                Code = "emp",
                ParentId = jobGroupId,
                DataScope = DataScope.Self,
                Sort = 1
            }
        };
        await _roleRep.InsertAsync(roles);

        //����û���ɫ
        var userRole = new UserRoleEntity()
        {
            UserId = userId,
            RoleId = roleId
        };
        await _userRoleRep.Value.InsertAsync(userRole);

        //����⻧
        var tenant = input.Adapt<TenantEntity>();
        tenant.Id = tenantId;
        tenant.UserId = userId;
        tenant.OrgId = org.Id;
        await _tenantRep.InsertAsync(tenant);

        return tenant.Id;
    }

    /// <summary>
    /// ע��
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    [NonAction]
    public virtual async Task<long> RegAsync(TenantRegInput input)
    {
        if (input.Password.IsNull())
        {
            input.Password = _appConfig.DefaultPassword;
        }
        _userHelper.Value.CheckPassword(input.Password);

        using var _t = _tenantRep.DataFilter.Disable(FilterNames.Tenant);
        using var _o = _orgRep.DataFilter.Disable(FilterNames.Tenant);
        using var _u = _userRep.DataFilter.Disable(FilterNames.Tenant);
 
        Expression<Func<UserEntity, bool>> where = (a => a.UserName == input.UserName);
        where = where.Or(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
            .Or(input.Email.NotNull(), a => a.Email == input.Email);

        //����û�
        var existsUser = await _userRep.Select.Where(where)
            .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception(_adminLocalizer["��ҵ�˺���ע��"]);
            }

            if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
            {
                throw ResultOutput.Exception(_adminLocalizer["��ҵ�ֻ�����ע��"]);
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception(_adminLocalizer["��ҵ������ע��"]);
            }
        }

        //��鲿��
        var existsOrg = await _orgRep.Select
       .Where(a => a.Name == input.Name && a.ParentId == 0)
       .WhereIf(input.Code.NotNull(), a => a.Code == input.Code)
       .FirstAsync(a => new { a.Id, a.TenantId });

        var hasTenant = existsOrg?.TenantId > 0;

        //�⻧Id
        long tenantId = hasTenant ? existsOrg.TenantId.Value : (input.Id > 0 ? input.Id : YitIdHelper.NextId());

        //��Ӳ���
        var orgId = existsOrg?.Id > 0 ? existsOrg.Id : YitIdHelper.NextId();
        if (existsOrg == null)
        {
            var org = new OrgEntity
            {
                Id = orgId,
                TenantId = tenantId,
                Name = input.Name,
                Code = input.Code,
                ParentId = 0,
                MemberCount = 1,
                Sort = 1,
                Enabled = true
            };
            await _orgRep.InsertAsync(org);
        }
       
        //����û�
        var user = new UserEntity
        {
            TenantId = tenantId,
            UserName = input.UserName,
            Name = input.RealName,
            Mobile = input.Mobile,
            Email = input.Email,
            Type = hasTenant ? UserType.DefaultUser : UserType.TenantAdmin,
            OrgId = orgId,
            Enabled = true
        };
        if (_appConfig.PasswordHasher)
        {
            user.Password = _passwordHasher.Value.HashPassword(user, input.Password);
            user.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
        }
        else
        {
            user.Password = MD5Encrypt.Encrypt32(input.Password);
            user.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
        }
        await _userRep.InsertAsync(user);

        long userId = user.Id;

        //����û�Ա��
        var emp = new UserStaffEntity
        {
            Id = userId,
            TenantId = tenantId,
        };
        await _userStaffRep.Value.InsertAsync(emp);

        //����û�����
        var userOrg = new UserOrgEntity
        {
            UserId = userId,
            OrgId = orgId
        };
        await _userOrgRep.Value.InsertAsync(userOrg);

        //��ӽ�ɫ����ͽ�ɫ
        if (!hasTenant)
        {
            var roleId = YitIdHelper.NextId();
            var jobGroupId = YitIdHelper.NextId();
            var roles = new List<RoleEntity>
            {
                new()
                {
                    Id= jobGroupId,
                    ParentId = 0,
                    TenantId = tenantId,
                    Type = RoleType.Group,
                    Name = "��λ",
                    Sort = 1
                },
                new()
                {
                    Id = roleId,
                    TenantId = tenantId,
                    Type = RoleType.Role,
                    Name = "������Ա",
                    Code = "main-admin",
                    ParentId = jobGroupId,
                    DataScope = DataScope.All,
                    Sort = 1
                },
                new()
                {
                    TenantId = tenantId,
                    Type = RoleType.Role,
                    Name = "��ͨԱ��",
                    Code = "emp",
                    ParentId = jobGroupId,
                    DataScope = DataScope.Self,
                    Sort = 1
                }
            };
            await _roleRep.InsertAsync(roles);

            //����û���ɫ
            var userRole = new UserRoleEntity()
            {
                UserId = userId,
                RoleId = roleId
            };
            await _userRoleRep.Value.InsertAsync(userRole);
        }

        //����⻧
        if(!hasTenant)
        {
            var tenant = input.Adapt<TenantEntity>();
            tenant.Id = tenantId;
            tenant.UserId = userId;
            tenant.OrgId = orgId;
            await _tenantRep.InsertAsync(tenant);

            //����⻧�ײ�
            if (input.PkgIds != null && input.PkgIds.Length != 0)
            {
                var pkgs = input.PkgIds.Select(pkgId => new TenantPkgEntity
                {
                    TenantId = tenantId,
                    PkgId = pkgId
                }).ToList();

                await _tenantPkgRep.InsertAsync(pkgs);
            }
        }

        return tenantId;
    }

    /// <summary>
    /// �޸�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(TenantUpdateInput input)
    {
        using var _t = _tenantRep.DataFilter.Disable(FilterNames.Tenant);
        using var _o = _orgRep.DataFilter.Disable(FilterNames.Tenant);
        using var _u = _userRep.DataFilter.Disable(FilterNames.Tenant);

        var tenant = await _tenantRep.GetAsync(input.Id);
        if (!(tenant?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["�⻧������"]);
        }

        var existsOrg = await _orgRep.Select
            .Where(a => a.Id != tenant.OrgId && a.ParentId == 0 && (a.Name == input.Name || a.Code == input.Code))
            .FirstAsync(a => new { a.Name, a.Code });

        if (existsOrg != null)
        {
            if (existsOrg.Name == input.Name)
            {
                throw ResultOutput.Exception(_adminLocalizer["��ҵ�����Ѵ���"]);
            }

            if (existsOrg.Code == input.Code)
            {
                throw ResultOutput.Exception(_adminLocalizer["��ҵ�����Ѵ���"]);
            }
        }

        Expression<Func<UserEntity, bool>> where = (a => a.UserName == input.UserName);
        where = where.Or(input.Phone.NotNull(), a => a.Mobile == input.Phone)
            .Or(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRep.Select.Where(a => a.Id != tenant.UserId).Where(where)
            .FirstAsync(a => new { a.Id, a.Name, a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception(_adminLocalizer["��ҵ�˺��Ѵ���"]);
            }

            if (input.Phone.NotNull() && existsUser.Mobile == input.Phone)
            {
                throw ResultOutput.Exception(_adminLocalizer["��ҵ�ֻ����Ѵ���"]);
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception(_adminLocalizer["��ҵ�����Ѵ���"]);
            }
        }

        //�����û�
        await _userRep.UpdateDiy.DisableGlobalFilter(FilterNames.Tenant).SetSource(
        new UserEntity()
        {
            Id = tenant.UserId,
            Name = input.RealName,
            UserName = input.UserName,
            Mobile = input.Phone,
            Email = input.Email
        })
        .UpdateColumns(a => new { a.Name, a.UserName, a.Mobile, a.Email, a.ModifiedTime }).ExecuteAffrowsAsync();

        //���²���
        await _orgRep.UpdateDiy.DisableGlobalFilter(FilterNames.Tenant).SetSource(
        new OrgEntity()
        {
            Id = tenant.OrgId,
            Name = input.Name,
            Code = input.Code
        })
        .UpdateColumns(a => new { a.Name, a.Code, a.ModifiedTime }).ExecuteAffrowsAsync();

        //�����⻧
        await _tenantRep.UpdateDiy.DisableGlobalFilter(FilterNames.Tenant).SetSource(
        new TenantEntity()
        {
            Id = tenant.Id,
            Domain = input.Domain,
            Description = input.Description,
        })
        .UpdateColumns(a => new { a.Description, a.Domain, a.ModifiedTime }).ExecuteAffrowsAsync();

        //�����⻧�ײ�
        await _tenantPkgRep.DeleteAsync(a => a.TenantId == tenant.Id);
        if (input.PkgIds != null && input.PkgIds.Any())
        {
            var pkgs = input.PkgIds.Select(pkgId => new TenantPkgEntity
            {
                TenantId = tenant.Id,
                PkgId = pkgId
            }).ToList();

            await _tenantPkgRep.InsertAsync(pkgs);

            //����⻧�������û�Ȩ�޻���
            await LazyGetRequiredService<PkgService>().ClearUserPermissionsAsync(new List<long> { tenant.Id });
        }
    }

    /// <summary>
    /// ����ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        using (_tenantRep.DataFilter.Disable(FilterNames.Tenant))
        {
            var tenantType = await _tenantRep.Select.WhereDynamic(id).ToOneAsync(a => a.TenantType);
            if (tenantType == TenantType.Platform)
            {
                throw ResultOutput.Exception(_adminLocalizer["ƽ̨�⻧��ֹɾ��"]);
            }

            //ɾ����ɫȨ��
            await _rolePermissionRep.Value.Where(a => a.Role.TenantId == id).DisableGlobalFilter(FilterNames.Tenant).ToDelete().ExecuteAffrowsAsync();

            //ɾ���û���ɫ
            await _userRoleRep.Value.Where(a => a.User.TenantId == id).DisableGlobalFilter(FilterNames.Tenant).ToDelete().ExecuteAffrowsAsync();

            //ɾ��Ա��
            await _userStaffRep.Value.Where(a => a.TenantId == id).DisableGlobalFilter(FilterNames.Tenant).ToDelete().ExecuteAffrowsAsync();

            //ɾ���û�����
            await _userOrgRep.Value.Where(a => a.User.TenantId == id).DisableGlobalFilter(FilterNames.Tenant).ToDelete().ExecuteAffrowsAsync();

            //ɾ������
            await _orgRep.Where(a => a.TenantId == id).DisableGlobalFilter(FilterNames.Tenant).ToDelete().ExecuteAffrowsAsync();

            //ɾ���û�
            await _userRep.Where(a => a.TenantId == id && a.Type != UserType.Member).DisableGlobalFilter(FilterNames.Tenant).ToDelete().ExecuteAffrowsAsync();

            //ɾ����ɫ
            await _roleRep.Where(a => a.TenantId == id).DisableGlobalFilter(FilterNames.Tenant).ToDelete().ExecuteAffrowsAsync();

            //ɾ���⻧�ײ�
            await _tenantPkgRep.DeleteAsync(a => a.TenantId == id);

            //ɾ���⻧
            await _tenantRep.DeleteAsync(id);

            //����⻧�������û�Ȩ�޻���
            await LazyGetRequiredService<PkgService>().ClearUserPermissionsAsync(new List<long> { id });
        }
    }

    /// <summary>
    /// ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        using (_tenantRep.DataFilter.Disable(FilterNames.Tenant))
        {
            var tenantType = await _tenantRep.Select.WhereDynamic(id).ToOneAsync(a => a.TenantType);
            if (tenantType == TenantType.Platform)
            {
                throw ResultOutput.Exception(_adminLocalizer["ƽ̨�⻧��ֹɾ��"]);
            }

            //ɾ������
            await _orgRep.SoftDeleteAsync(a => a.TenantId == id, FilterNames.Tenant);

            //ɾ���û�
            await _userRep.SoftDeleteAsync(a => a.TenantId == id && a.Type != UserType.Member, FilterNames.Tenant);

            //ɾ����ɫ
            await _roleRep.SoftDeleteAsync(a => a.TenantId == id, FilterNames.Tenant);

            //ɾ���⻧
            var result = await _tenantRep.SoftDeleteAsync(id);

            //����⻧�������û�Ȩ�޻���
            await LazyGetRequiredService<PkgService>().ClearUserPermissionsAsync(new List<long> { id });
        }
    }

    /// <summary>
    /// ����ɾ��
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        using (_tenantRep.DataFilter.Disable(FilterNames.Tenant))
        {
            var tenantType = await _tenantRep.Select.WhereDynamic(ids).ToOneAsync(a => a.TenantType);
            if (tenantType == TenantType.Platform)
            {
                throw ResultOutput.Exception(_adminLocalizer["ƽ̨�⻧��ֹɾ��"]);
            }

            //ɾ������
            await _orgRep.SoftDeleteAsync(a => ids.Contains(a.TenantId.Value), FilterNames.Tenant);

            //ɾ���û�
            await _userRep.SoftDeleteAsync(a => ids.Contains(a.TenantId.Value) && a.Type != UserType.Member, FilterNames.Tenant);

            //ɾ����ɫ
            await _roleRep.SoftDeleteAsync(a => ids.Contains(a.TenantId.Value), FilterNames.Tenant);

            //ɾ���⻧
            var result = await _tenantRep.SoftDeleteAsync(ids);

            //����⻧�������û�Ȩ�޻���
            await LazyGetRequiredService<PkgService>().ClearUserPermissionsAsync(ids.ToList());
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetEnableAsync(TenantSetEnableInput input)
    {
        var entity = await _tenantRep.GetAsync(input.TenantId);
        if (entity.TenantType == TenantType.Platform)
        {
            throw ResultOutput.Exception(_adminLocalizer["ƽ̨�⻧��ֹ����"]);
        }
        entity.Enabled = input.Enabled;
        await _tenantRep.UpdateAsync(entity);
    }

    /// <summary>
    /// һ����¼
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<TokenInfo> OneClickLoginAsync([ValidateRequired] long tenantId)
    {
        if (!(tenantId > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["��ѡ���⻧"]);
        }

        var userRep = _userRep;
        using var _ = userRep.DataFilter.DisableAll();

        var authLoginOutput = await userRep.Select
            .Where(a => a.Tenant.Id == tenantId && a.Tenant.UserId == a.Id)
            .ToOneAsync(a=> new AuthLoginOutput
            {
                Tenant = new AuthLoginTenantModel
                {
                    DbKey = a.Tenant.DbKey,
                    Enabled = a.Tenant.Enabled,
                    TenantType = a.Tenant.TenantType,
                }
            });

        if (authLoginOutput == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["��������Ա������"]);
        }

        var tokenInfo = AppInfo.GetRequiredService<IAuthService>().GetTokenInfo(authLoginOutput);

        return tokenInfo;
    }
}
