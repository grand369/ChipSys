using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using FreeSql;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Configs;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Domain.Permission;
using ChipSys.Admin.Domain.RolePermission;
using ChipSys.Admin.Domain.UserRole;
using ChipSys.Admin.Domain.PermissionApi;
using ChipSys.Admin.Domain.Role;
using ChipSys.Admin.Domain.Tenant;
using ChipSys.Admin.Domain.PkgPermission;
using ChipSys.Admin.Domain.TenantPkg;
using ChipSys.Admin.Services.Permission.Dto;
using ChipSys.Admin.Resources;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using ChipSys.Common.Extensions;

namespace ChipSys.Admin.Services.Permission;

/// <summary>
/// Ȩ�޷���
/// </summary>
[Order(40)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class PermissionService : BaseService, IPermissionService, IDynamicApi
{
    private readonly IPermissionRepository _permissionRep;
    private readonly IPermissionApiRepository _permissionApiRep;
    private readonly AdminLocalizer _adminLocalizer;
    private readonly Lazy<IOptions<AppConfig>> _appConfig;
    private readonly Lazy<IRoleRepository> _roleRep;
    private readonly Lazy<IRolePermissionRepository> _rolePermissionRep;
    private readonly Lazy<IUserRoleRepository> _userRoleRep;
    

    public PermissionService(
        IPermissionRepository permissionRep,
        IPermissionApiRepository permissionApiRep,
        AdminLocalizer adminLocalizer,
        Lazy<IOptions<AppConfig>> appConfig,
        Lazy<IRoleRepository> roleRep,
        Lazy<IRolePermissionRepository> rolePermissionRep,
        Lazy<IUserRoleRepository> userRoleRep
    )
    {
        _permissionRep = permissionRep;
        _permissionApiRep = permissionApiRep;
        _adminLocalizer = adminLocalizer;
        _appConfig = appConfig;
        _roleRep = roleRep;
        _rolePermissionRep = rolePermissionRep;
        _userRoleRep = userRoleRep;
    }

    /// <summary>
    /// ���Ȩ���¹������û�Ȩ�޻���
    /// </summary>
    /// <param name="permissionIds"></param>
    /// <returns></returns>
    private async Task ClearUserPermissionsAsync(List<long> permissionIds)
    {
        var userIds = await _userRoleRep.Value.Select.Where(a =>
            _rolePermissionRep.Value
            .Where(b => b.RoleId == a.RoleId && permissionIds.Contains(b.PermissionId))
            .Any()
        ).ToListAsync(a => a.UserId);
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.UserPermission + userId);
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }

    /// <summary>
    /// ��ѯ����
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PermissionGetGroupOutput> GetGroupAsync(long id)
    {
        var result = await _permissionRep.GetAsync<PermissionGetGroupOutput>(id);
        return result;
    }

    /// <summary>
    /// ��ѯ�˵�
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PermissionGetMenuOutput> GetMenuAsync(long id)
    {
        var result = await _permissionRep.GetAsync<PermissionGetMenuOutput>(id);
        return result;
    }

    /// <summary>
    /// ��ѯȨ�޵�
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PermissionGetDotOutput> GetDotAsync(long id)
    {
        var output = await _permissionRep.Select
        .WhereDynamic(id)
        .ToOneAsync(a => new PermissionGetDotOutput
        {
            ApiIds = _permissionApiRep.Where(b => b.PermissionId == a.Id).OrderBy(a => a.Id).ToList(b => b.Api.Id)
        });
        return output;
    }

    /// <summary>
    /// ��ѯȨ���б�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<PermissionGetListOutput>> GetListAsync(PermissionGetListInput input)
    {
        var platform = input?.Platform?.Trim();
        var label = input?.Label?.Trim();
        var path = input?.Path?.Trim();

        var select = _permissionRep.Select;
        if (platform.NotNull())
        {
            Expression<Func<PermissionEntity, bool>> where = null;
            where = where.And(a => a.Platform == platform);
            if (platform.ToLower() == AdminConsts.WebName)
            {
                where = where.Or(a => string.IsNullOrEmpty(a.Platform));
            }
            select = select.Where(where);
        }
        else
        {
            select = select.Where(a => string.IsNullOrEmpty(a.Platform));
        }

        var data = await select
            .WhereIf(label.NotNull(), a => a.Label.Contains(label))
            .WhereIf(path.NotNull(), a => a.Path.Contains(path))
            .Include(a => a.View)
            .OrderBy(a => new { a.ParentId, a.Sort })
            .ToListAsync(a => new PermissionGetListOutput
            {
                ViewPath = a.View.Path,
                ApiPaths = string.Join(";", _permissionApiRep.Where(b => b.PermissionId == a.Id).ToList(b => b.Api.Path))
            });

        return data;
    }

    /// <summary>
    /// ��ѯ��ȨȨ���б�
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    public async Task<List<PermissionGetPermissionListOutput>> GetPermissionListAsync(string platform)
    {
        var select = _permissionRep.Select;
        if (platform.NotNull())
        {
            Expression<Func<PermissionEntity, bool>> where = null;
            where = where.And(a => a.Platform == platform);
            if (platform.ToLower() == AdminConsts.WebName)
            {
                where = where.Or(a => string.IsNullOrEmpty(a.Platform));
            }
            select = select.Where(where);
        }
        else
        {
            select = select.Where(a => string.IsNullOrEmpty(a.Platform));
        }

        var permissions = await select
            .Where(a => a.Enabled == true && a.IsSystem == false)
            .WhereIf(_appConfig.Value.Value.Tenant && User.TenantType == TenantType.Tenant, a =>
                _permissionRep.Orm.Select<TenantPkgEntity, PkgPermissionEntity>()
                .Where((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                .Any()
            )
           .AsTreeCte(up: true)
           .Where(a => a.Type != PermissionType.Menu || (a.Type == PermissionType.Menu && a.View.Enabled == true))
           .ToListAsync(a => new { a.Id, a.ParentId, a.Label, a.Type, a.Sort });

        var menus = permissions.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort)
            .Select(a => new PermissionGetPermissionListOutput
            {
                Id = a.Id,
                ParentId = a.ParentId,
                Label = a.Label,
                Row = a.Type == PermissionType.Menu,
            }).ToList();

        menus = menus.ToTree((r, c) =>
        {
            return c.ParentId == 0;
        },
        (r, c) =>
        {
            return r.Id == c.ParentId;
        },
        (r, datalist) =>
        {
            r.Children ??= [];
            r.Children.AddRange(datalist);
        });

        return menus;
    }

    /// <summary>
    /// ��ѯ��ɫȨ���б�
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<List<long>> GetRolePermissionListAsync(long roleId = 0)
    {
        var permissionIds = await _rolePermissionRep.Value
            .Select.Where(d => d.RoleId == roleId)
            .ToListAsync(a => a.PermissionId);

        return permissionIds;
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddGroupAsync(PermissionAddGroupInput input)
    {
        if (await _permissionRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Label == input.Label))
        {
            throw ResultOutput.Exception(_adminLocalizer["�˷����Ѵ���"]);
        }

        var entity = Mapper.Map<PermissionEntity>(input);
        entity.Type = PermissionType.Group;

        if (entity.Sort == 0)
        {
            var sort = await _permissionRep.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _permissionRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// �����˵�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddMenuAsync(PermissionAddMenuInput input)
    {
        if (await _permissionRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Label == input.Label))
        {
            throw ResultOutput.Exception(_adminLocalizer["�˲˵��Ѵ���"]);
        }

        var entity = Mapper.Map<PermissionEntity>(input);
        entity.Type = PermissionType.Menu;
        if (entity.Sort == 0)
        {
            var sort = await _permissionRep.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _permissionRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// ����Ȩ�޵�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task<long> AddDotAsync(PermissionAddDotInput input)
    {
        if (await _permissionRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Label == input.Label))
        {
            throw ResultOutput.Exception(_adminLocalizer["��Ȩ�޵��Ѵ���"]);
        }

        if (await _permissionRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Label == input.Label))
        {
            throw ResultOutput.Exception(_adminLocalizer["��Ȩ�޵��Ѵ���"]);
        }

        if (await _permissionRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["��Ȩ�޵�����Ѵ���"]);
        }

        var entity = Mapper.Map<PermissionEntity>(input);
        entity.Type = PermissionType.Dot;
        if (entity.Sort == 0)
        {
            var sort = await _permissionRep.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _permissionRep.InsertAsync(entity);

        if (input.ApiIds != null && input.ApiIds.Any())
        {
            var permissionApis = input.ApiIds.Select(a => new PermissionApiEntity { PermissionId = entity.Id, ApiId = a }).ToList();
            await _permissionApiRep.InsertAsync(permissionApis);
        }

        return entity.Id;
    }

    /// <summary>
    /// �޸ķ���
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateGroupAsync(PermissionUpdateGroupInput input)
    {
        var entity = await _permissionRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["���鲻����"]);
        }

        if (input.Id == input.ParentId)
        {
            throw ResultOutput.Exception(_adminLocalizer["�ϼ����鲻���Ǳ�����"]);
        }

        if (await _permissionRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Id != input.Id && a.Label == input.Label))
        {
            throw ResultOutput.Exception(_adminLocalizer["�˷����Ѵ���"]);
        }

        entity = Mapper.Map(input, entity);
        await _permissionRep.UpdateAsync(entity);
    }

    /// <summary>
    /// �޸Ĳ˵�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateMenuAsync(PermissionUpdateMenuInput input)
    {
        var entity = await _permissionRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["�˵�������"]);
        }

        if (await _permissionRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Id != input.Id && a.Label == input.Label))
        {
            throw ResultOutput.Exception(_adminLocalizer["�˲˵��Ѵ���"]);
        }

        entity = Mapper.Map(input, entity);
        await _permissionRep.UpdateAsync(entity);
    }

    /// <summary>
    /// �޸�Ȩ�޵�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task UpdateDotAsync(PermissionUpdateDotInput input)
    {
        var entity = await _permissionRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["Ȩ�޵㲻����"]);
        }

        if (await _permissionRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Id != input.Id && a.Label == input.Label))
        {
            throw ResultOutput.Exception(_adminLocalizer["��Ȩ�޵��Ѵ���"]);
        }

        Mapper.Map(input, entity);
        await _permissionRep.UpdateAsync(entity);
        await _permissionApiRep.DeleteAsync(a => a.PermissionId == entity.Id);

        if (input.ApiIds != null && input.ApiIds.Any())
        {
            var permissionApis = input.ApiIds.Select(a => new PermissionApiEntity { PermissionId = entity.Id, ApiId = a });
            await _permissionApiRep.InsertAsync(permissionApis.ToList());
        }

        //����û�Ȩ�޻���
        await ClearUserPermissionsAsync(new List<long> { entity.Id });
    }

    /// <summary>
    /// ����ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        //�ݹ��ѯ����Ȩ�޵�
        var ids = _permissionRep.Select
        .Where(a => a.Id == id)
        .AsTreeCte()
        .ToList(a => a.Id);

        //ɾ��Ȩ�޹����ӿ�
        await _permissionApiRep.DeleteAsync(a => ids.Contains(a.PermissionId));

        //ɾ�����Ȩ��
        await _permissionRep.DeleteAsync(a => ids.Contains(a.Id));

        //����û�Ȩ�޻���
        await ClearUserPermissionsAsync(ids);
    }

    /// <summary>
    /// ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task SoftDeleteAsync(long id)
    {
        //�ݹ��ѯ����Ȩ�޵�
        var ids = _permissionRep.Select
        .Where(a => a.Id == id)
        .AsTreeCte()
        .ToList(a => a.Id);

        //ɾ��Ȩ��
        await _permissionRep.SoftDeleteAsync(a => ids.Contains(a.Id));

        //����û�Ȩ�޻���
        await ClearUserPermissionsAsync(ids);
    }

    /// <summary>
    /// �����ɫȨ��
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task AssignAsync(PermissionAssignInput input)
    {
        //����Ȩ�޵�ʱ���жϽ�ɫ�Ƿ����
        var exists = await _roleRep.Value.Select.DisableGlobalFilter(FilterNames.Tenant).WhereDynamic(input.RoleId).AnyAsync();
        if (!exists)
        {
            throw ResultOutput.Exception(_adminLocalizer["�ý�ɫ�����ڻ��ѱ�ɾ��"]);
        }

        var platform = input?.Platform;
        Expression<Func<RolePermissionEntity, bool>> where = null;
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

        //��ѯ��ɫȨ��
        var permissionIds = await _rolePermissionRep.Value.Where(where).Where(a => a.RoleId == input.RoleId).ToListAsync(a => a.PermissionId);

        //����ɾ��Ȩ��
        var deleteIds = permissionIds.Where(a => !input.PermissionIds.Contains(a));
        if (deleteIds.Any())
        {
            await _rolePermissionRep.Value.Where(where).Where(a => a.RoleId == input.RoleId && deleteIds.Contains(a.PermissionId)).ToDelete().ExecuteAffrowsAsync();
        }

        //��������Ȩ��
        var insertRolePermissions = new List<RolePermissionEntity>();
        var insertPermissionIds = input.PermissionIds.Where(a => !permissionIds.Contains(a));

        //��ֹ�⻧�Ƿ���Ȩ����ѯ�����⻧Ȩ�޷�Χ
        if (_appConfig.Value.Value.Tenant && User.TenantType == TenantType.Tenant)
        {
            var cloud = ServiceProvider.GetRequiredService<FreeSqlCloud>();
            var mainDb = cloud.Use(DbKeys.AdminDb);
            var pkgPermissionIds = await mainDb.Select<PkgPermissionEntity>()
                .Where(a => 
                    mainDb.Select<TenantPkgEntity>()
                    .Where((b) => b.PkgId == a.PkgId && b.TenantId == User.TenantId)
                    .Any()
                )
                .ToListAsync(a => a.PermissionId);

            insertPermissionIds = insertPermissionIds.Where(a => pkgPermissionIds.Contains(a));
        }

        if (insertPermissionIds.Any())
        {
            foreach (var permissionId in insertPermissionIds)
            {
                insertRolePermissions.Add(new RolePermissionEntity()
                {
                    Platform = platform,
                    RoleId = input.RoleId,
                    PermissionId = permissionId,
                });
            }
            await _rolePermissionRep.Value.InsertAsync(insertRolePermissions);
        }

        //�����ɫ�¹������û�Ȩ�޻���
        var userIds = await _userRoleRep.Value.Select.Where(a => a.RoleId == input.RoleId).ToListAsync(a => a.UserId);
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.UserPermission + userId);
        }
    }
}
