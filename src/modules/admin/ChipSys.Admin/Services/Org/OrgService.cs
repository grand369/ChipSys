using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Domain.Org;
using ChipSys.Admin.Domain.UserOrg;
using ChipSys.Admin.Domain.Role;
using ChipSys.Admin.Domain.RoleOrg;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Services.Org.Input;
using ChipSys.Admin.Services.Org.Output;
using ChipSys.DynamicApi.Attributes;
using ChipSys.DynamicApi;
using ChipSys.Admin.Tools.Cache;
using ChipSys.Admin.Core.Auth;
using ChipSys.Admin.Contracts.Core.Consts;

namespace ChipSys.Admin.Services.Org;

/// <summary>
/// ���ŷ���
/// </summary>
[Order(30)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class OrgService : BaseService, IOrgService, IDynamicApi
{
    private readonly IOrgRepository _orgRep;
    private readonly IUserOrgRepository _userOrgRep;
    private readonly IRoleOrgRepository _roleOrgRep;
    private readonly AdminLocalizer _localizer;
    private readonly ICacheTool _cache;
    private readonly IUser _user;

    public OrgService(
        IOrgRepository orgRep,
        IUserOrgRepository userOrgRep,
        IRoleOrgRepository roleOrgRep,
        AdminLocalizer localizer,
        ICacheTool cache,
        IUser user
    )
    {
        _orgRep = orgRep;
        _userOrgRep = userOrgRep;
        _roleOrgRep = roleOrgRep;
        _localizer = localizer;
        _cache = cache;
        _user = user;
    }

    /// <summary>
    /// �������
    /// </summary>
    private async Task ClearCacheAsync()
    {
        await Cache.DelByPatternAsync(CacheKeys.DataPermission + "*");
        await Cache.DelAsync(AdminCacheKeys.GetOrgKey(_user.TenantId));
    }

    /// <summary>
    /// ��ѯ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrgGetOutput> GetAsync(long id)
    {
        var result = await _orgRep.GetAsync<OrgGetOutput>(id);
        return result;
    }

    /// <summary>
    /// ��ѯ�б�
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<List<OrgGetListOutput>> GetListAsync(string key)
    {
        var dataPermission = User.DataPermission;

        var data = await _orgRep.Select
            .WhereIf(dataPermission.OrgIds.Count > 0, a => dataPermission.OrgIds.Contains(a.Id))
            .WhereIf(dataPermission.DataScope == DataScope.Self, a => a.CreatedUserId == User.Id)
            .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
            .ToListAsync<OrgGetListOutput>();

        return data?.Count > 0 ? data.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort).ToList() : data;
    }

    /// <summary>
    /// ��ȡ����·���б�
    /// </summary>
    /// <returns></returns>
    public async Task<List<OrgGetSimpleListWithPathOutput>> GetSimpleListWithPathAsync()
    {
        return await _cache.GetOrSetAsync(AdminCacheKeys.GetOrgKey(_user.TenantId), async () =>
        {
            return await _orgRep.Select.Where(a => a.ParentId == 0)
            .AsTreeCte(a => a.Name, pathSeparator: "/")
            .ToListAsync(a => new OrgGetSimpleListWithPathOutput
            {
                Id = a.Id,
                Path = "a.cte_path"
            });
        }, TimeSpan.FromDays(365));
        
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(OrgAddInput input)
    {
        if(input.ParentId == 0)
        {
            throw ResultOutput.Exception(_localizer["��ѡ���ϼ�����"]);
        }

        if (await _orgRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_localizer["�˲����Ѵ���"]);
        }

        if (input.Code.NotNull() && await _orgRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_localizer["�˲��ű����Ѵ���"]);
        }

        var entity = Mapper.Map<OrgEntity>(input);

        if (entity.Sort == 0)
        {
            var sort = await _orgRep.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _orgRep.InsertAsync(entity);
        await ClearCacheAsync();

        return entity.Id;
    }

    /// <summary>
    /// �޸�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(OrgUpdateInput input)
    {
        if (input.ParentId == 0)
        {
            throw ResultOutput.Exception(_localizer["��ѡ���ϼ�����"]);
        }

        var entity = await _orgRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_localizer["���Ų�����"]);
        }

        if (input.Id == input.ParentId)
        {
            throw ResultOutput.Exception(_localizer["�ϼ����Ų����Ǳ�����"]);
        }

        if (await _orgRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_localizer["�˲����Ѵ���"]);
        }

        if (input.Code.NotNull() && await _orgRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_localizer["�˲��ű����Ѵ���"]);
        }

        var childIdList = await _orgRep.GetChildIdListAsync(input.Id);
        if (childIdList.Contains(input.ParentId))
        {
            throw ResultOutput.Exception(_localizer["�ϼ����Ų������¼�����"]);
        }

        Mapper.Map(input, entity);
        await _orgRep.UpdateAsync(entity);

        await ClearCacheAsync();
    }

    /// <summary>
    /// ����ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public async Task DeleteAsync(long id)
    {
        //���������Ƿ���Ա��
        if(await _userOrgRep.HasUser(id))
        {
            throw ResultOutput.Exception(_localizer["��ǰ������Ա���޷�ɾ��"]);
        }

        var orgIdList = await _orgRep.GetChildIdListAsync(id);
        //�����ŵ��¼��������Ƿ���Ա��
        if (await _userOrgRep.HasUser(orgIdList))
        {
            throw ResultOutput.Exception(_localizer["�����ŵ��¼�������Ա���޷�ɾ��"]);
        }

        //ɾ�����Ž�ɫ
        await _roleOrgRep.DeleteAsync(a => orgIdList.Contains(a.OrgId));

        //ɾ�������ź��¼�����
        await _orgRep.DeleteAsync(a => orgIdList.Contains(a.Id));

        await ClearCacheAsync();
    }

    /// <summary>
    /// ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public async Task SoftDeleteAsync(long id)
    {
        //���������Ƿ���Ա��
        if (await _userOrgRep.HasUser(id))
        {
            throw ResultOutput.Exception(_localizer["��ǰ������Ա���޷�ɾ��"]);
        }

        var orgIdList = await _orgRep.GetChildIdListAsync(id);
        //�����ŵ��¼��������Ƿ���Ա��
        if (await _userOrgRep.HasUser(orgIdList))
        {
            throw ResultOutput.Exception(_localizer["�����ŵ��¼�������Ա���޷�ɾ��"]);
        }

        //ɾ�����Ž�ɫ
        await _roleOrgRep.SoftDeleteAsync(a => orgIdList.Contains(a.OrgId));

        //ɾ�������ź��¼�����
        await _orgRep.SoftDeleteAsync(a => orgIdList.Contains(a.Id));

        await ClearCacheAsync();
    }
}
