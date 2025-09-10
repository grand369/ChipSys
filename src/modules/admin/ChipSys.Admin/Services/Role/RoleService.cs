using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Domain;
using ChipSys.Admin.Domain.UserRole;
using ChipSys.Admin.Domain.User;
using ChipSys.Admin.Domain.Org;
using ChipSys.Admin.Domain.Role;
using ChipSys.Admin.Domain.RolePermission;
using ChipSys.Admin.Domain.RoleOrg;
using ChipSys.Admin.Services.Role.Dto;
using ChipSys.Admin.Resources;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;

namespace ChipSys.Admin.Services.Role;

/// <summary>
/// ��ɫ����
/// </summary>
[Order(20)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class RoleService : BaseService, IRoleService, IDynamicApi
{
    private readonly IRoleRepository _roleRep;
    private readonly IUserRepository _userRep;
    private readonly IUserRoleRepository _userRoleRep;
    private readonly IRolePermissionRepository _rolePermissionRep;
    private readonly IRoleOrgRepository _roleOrgRep;
    private readonly AdminLocalizer _adminLocalizer;

    public RoleService(
        IRoleRepository roleRep,
        IUserRepository userRep,
        IUserRoleRepository userRoleRep,
        IRolePermissionRepository rolePermissionRep,
        IRoleOrgRepository roleOrgRep,
        AdminLocalizer adminLocalizer
    )
    {
        _roleRep = roleRep;
        _userRep = userRep;
        _userRoleRep = userRoleRep;
        _rolePermissionRep = rolePermissionRep;
        _roleOrgRep = roleOrgRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// ��ӽ�ɫ����
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="orgIds"></param>
    /// <returns></returns>
    private async Task AddRoleOrgAsync(long roleId, long[] orgIds)
    {
        if (orgIds != null && orgIds.Any())
        {
            var roleOrgs = orgIds.Select(orgId => new RoleOrgEntity
            {
                RoleId = roleId,
                OrgId = orgId
            }).ToList();
            await _roleOrgRep.InsertAsync(roleOrgs);
        }
    }

    /// <summary>
    /// ��ѯ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<RoleGetOutput> GetAsync(long id)
    {
        var roleEntity = await _roleRep.Select
        .WhereDynamic(id)
        .IncludeMany(a => a.Orgs.Select(b => new OrgEntity { Id = b.Id }))
        .ToOneAsync(a => new RoleGetOutput { Orgs = a.Orgs });

        var output = Mapper.Map<RoleGetOutput>(roleEntity);

        return output;
    }

    /// <summary>
    /// ��ѯ�б�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<RoleGetListOutput>> GetListAsync([FromQuery]RoleGetListInput input)
    {
        var list = await _roleRep.Select
        .WhereIf(input.Name.NotNull(), a => a.Name.Contains(input.Name))
        .OrderBy(a => new {a.ParentId, a.Sort})
        .ToListAsync<RoleGetListOutput>();

        return list;
    }

    /// <summary>
    /// ��ѯ��ҳ
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<RoleGetPageOutput>> GetPageAsync(PageInput<RoleGetPageInput> input)
    {
        var key = input.Filter?.Name;

        var list = await _roleRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<RoleGetPageOutput>();

        var data = new PageOutput<RoleGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// ��ѯ��ɫ�û��б�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<RoleGetRoleUserListOutput>> GetRoleUserListAsync([FromQuery] RoleGetRoleUserListInput input)
    {
        var list = await _userRep.Select.From<UserRoleEntity>()
            .InnerJoin(a => a.t2.UserId == a.t1.Id)
            .Where(a => a.t2.RoleId == input.RoleId)
            .WhereIf(input.Name.NotNull(), a => a.t1.Name.Contains(input.Name))
            .OrderByDescending(a => a.t1.Id)
            .ToListAsync<RoleGetRoleUserListOutput>();

        return list;
    }

    /// <summary>
    /// ��ӽ�ɫ�û�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task AddRoleUserAsync(RoleAddRoleUserListInput input)
    {
        var roleId = input.RoleId;
        var userIds = await _userRoleRep.Select.Where(a => a.RoleId == roleId).ToListAsync(a => a.UserId);
        var insertUserIds = input.UserIds.Except(userIds);
        if (insertUserIds != null && insertUserIds.Any())
        {
            var userRoleList = insertUserIds.Select(userId => new UserRoleEntity 
            { 
                UserId = userId, 
                RoleId = roleId 
            }).ToList();
            await _userRoleRep.InsertAsync(userRoleList);
        }

        var clearUserIds = userIds.Concat(input.UserIds).Distinct();
        foreach (var userId in clearUserIds)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }

    /// <summary>
    /// �Ƴ���ɫ�û�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task RemoveRoleUserAsync(RoleAddRoleUserListInput input)
    {
        var userIds = input.UserIds;
        if (userIds != null && userIds.Any())
        {
            await _userRoleRep.Where(a => a.RoleId == input.RoleId && input.UserIds.Contains(a.UserId)).ToDelete().ExecuteAffrowsAsync();
        }

        foreach (var userId in userIds)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(RoleAddInput input)
    {
        if (await _roleRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["��{0}�Ѵ���", input.Type == RoleType.Group ? "����" : "��ɫ"]);
        }

        if (input.Code.NotNull() && await _roleRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["��{0}�����Ѵ���", input.Type == RoleType.Group ? "����" : "��ɫ"]);
        }

        var entity = Mapper.Map<RoleEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _roleRep.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _roleRep.InsertAsync(entity);
        if (input.DataScope == DataScope.Custom)
        {
            await AddRoleOrgAsync(entity.Id, input.OrgIds);
        }

        return entity.Id;
    }

    /// <summary>
    /// �޸�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(RoleUpdateInput input)
    {
        var entity = await _roleRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["��ɫ������"]);
        }

        if (await _roleRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["��{0}�Ѵ���", input.Type == RoleType.Group ? "����" : "��ɫ"]);
        }

        if (input.Code.NotNull() && await _roleRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["��{0}�����Ѵ���", input.Type == RoleType.Group ? "����" : "��ɫ"]);
        }

        Mapper.Map(input, entity);
        await _roleRep.UpdateAsync(entity);
        await _roleOrgRep.DeleteAsync(a => a.RoleId == entity.Id);
        if (input.DataScope == DataScope.Custom)
        {
            await AddRoleOrgAsync(entity.Id, input.OrgIds);
        }

        var userIds = await _userRoleRep.Select.Where(a => a.RoleId == entity.Id).ToListAsync(a => a.UserId);
        foreach (var userId in userIds)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
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
        var roleIdList = await _roleRep.GetChildIdListAsync(id);
        var userIds = await _userRoleRep.Select.Where(a => roleIdList.Contains(a.RoleId)).ToListAsync(a => a.UserId);

        //ɾ���û���ɫ
        await _userRoleRep.DeleteAsync(a => a.UserId == id);
        //ɾ����ɫȨ��
        await _rolePermissionRep.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        //ɾ����ɫ
        await _roleRep.DeleteAsync(a => roleIdList.Contains(a.Id));
        
        foreach (var userId in userIds)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }

    /// <summary>
    /// ��������ɾ��
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        var roleIdList = await _roleRep.GetChildIdListAsync(ids);
        var userIds = await _userRoleRep.Select.Where(a => roleIdList.Contains(a.RoleId)).ToListAsync(a => a.UserId);

        //ɾ���û���ɫ
        await _userRoleRep.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        //ɾ����ɫȨ��
        await _rolePermissionRep.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        //ɾ����ɫ
        await _roleRep.Where(a => roleIdList.Contains(a.Id)).AsTreeCte().ToDelete().ExecuteAffrowsAsync();

        foreach (var userId in userIds)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
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
        var roleIdList = await _roleRep.GetChildIdListAsync(id);
        var userIds = await _userRoleRep.Select.Where(a => roleIdList.Contains(a.RoleId)).ToListAsync(a => a.UserId);
        await _userRoleRep.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        await _rolePermissionRep.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        await _roleRep.SoftDeleteRecursiveAsync(a => roleIdList.Contains(a.Id));
        foreach (var userId in userIds)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
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
        var roleIdList = await _roleRep.GetChildIdListAsync(ids);
        var userIds = await _userRoleRep.Select.Where(a => ids.Contains(a.RoleId)).ToListAsync(a => a.UserId);
        await _userRoleRep.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        await _rolePermissionRep.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        await _roleRep.SoftDeleteRecursiveAsync(a => roleIdList.Contains(a.Id));
        foreach (var userId in userIds)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }

    /// <summary>
    /// ��������Ȩ��
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetDataScopeAsync(RoleSetDataScopeInput input)
    {
        var entity = await _roleRep.GetAsync(input.RoleId);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["��ɫ������"]);
        }

        Mapper.Map(input, entity);
        await _roleRep.UpdateAsync(entity);
        await _roleOrgRep.DeleteAsync(a => a.RoleId == entity.Id);
        if (input.DataScope == DataScope.Custom)
        {
            await AddRoleOrgAsync(entity.Id, input.OrgIds);
        }

        var userIds = await _userRoleRep.Select.Where(a => a.RoleId == entity.Id).ToListAsync(a => a.UserId);
        foreach (var userId in userIds)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }
}
