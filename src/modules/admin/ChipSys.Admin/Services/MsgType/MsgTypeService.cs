using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Domain.MsgType;
using ChipSys.Admin.Services.MsgType.Dto;
using ChipSys.Admin.Resources;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Admin.Repositories;

namespace ChipSys.Admin.Services.MsgType;

/// <summary>
/// ��Ϣ�������
/// </summary>
[Order(20)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class MsgTypeService : BaseService, IDynamicApi
{
    private readonly AdminRepositoryBase<MsgTypeEntity> _msgTypeRep;
    private readonly AdminLocalizer _adminLocalizer;

    public MsgTypeService(AdminRepositoryBase<MsgTypeEntity> msgTypeRep, AdminLocalizer adminLocalizer)
    {
        _msgTypeRep = msgTypeRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// ��ñ������¼�Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<List<long>> GetChildIdListAsync(long id)
    {
        return await _msgTypeRep.Select
        .Where(a => a.Id == id)
        .AsTreeCte()
        .ToListAsync(a => a.Id);
    }

    /// <summary>
    /// ��ñ������¼�Id
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<List<long>> GetChildIdListAsync(long[] ids)
    {
        return await _msgTypeRep.Select
        .Where(a => ids.Contains(a.Id))
        .AsTreeCte()
        .ToListAsync(a => a.Id);
    }

    /// <summary>
    /// ��ѯ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<MsgTypeGetOutput> GetAsync(long id)
    {
        var output = await _msgTypeRep.Select
        .WhereDynamic(id)
        .ToOneAsync<MsgTypeGetOutput>();

        return output;
    }

    /// <summary>
    /// ��ѯ�б�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Login]
    public async Task<List<MsgTypeGetListOutput>> GetListAsync([FromQuery]MsgTypeGetListInput input)
    {
        var list = await _msgTypeRep.Select
        .WhereIf(input.Name.NotNull(), a => a.Name.Contains(input.Name))
        .OrderBy(a => new {a.ParentId, a.Sort})
        .ToListAsync<MsgTypeGetListOutput>();

        return list;
    }
        
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(MsgTypeAddInput input)
    {
        if (await _msgTypeRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["�˷����Ѵ���"]);
        }

        if (input.Code.NotNull() && await _msgTypeRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["�˷�������Ѵ���"]);
        }

        var entity = Mapper.Map<MsgTypeEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _msgTypeRep.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _msgTypeRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// �޸�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(MsgTypeUpdateInput input)
    {
        if (input.Id == input.ParentId) 
        {
            throw ResultOutput.Exception(_adminLocalizer["�ϼ����鲻�����Լ�"]);
        }

        var entity = await _msgTypeRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["���಻����"]);
        }

        if (await _msgTypeRep.Select.AnyAsync(a => a.Id != input.Id && a.ParentId == input.ParentId && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["�˷����Ѵ���"]);
        }

        if (input.Code.NotNull() && await _msgTypeRep.Select.AnyAsync(a => a.Id != input.Id && a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["�˷�������Ѵ���"]);
        }

        Mapper.Map(input, entity);
        await _msgTypeRep.UpdateAsync(entity);
    }

    /// <summary>
    /// ����ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task DeleteAsync(long id)
    {
        var msgTypeIdList = await GetChildIdListAsync(id);
        await _msgTypeRep.DeleteAsync(a => msgTypeIdList.Contains(a.Id));
    }

    /// <summary>
    /// ��������ɾ��
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        var msgTypeIdList = await GetChildIdListAsync(ids);
        await _msgTypeRep.Where(a => msgTypeIdList.Contains(a.Id)).AsTreeCte().ToDelete().ExecuteAffrowsAsync();
    }

    /// <summary>
    /// ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task SoftDeleteAsync(long id)
    {
        var msgTypeIdList = await GetChildIdListAsync(id);
        await _msgTypeRep.SoftDeleteRecursiveAsync(a => msgTypeIdList.Contains(a.Id));
    }

    /// <summary>
    /// ����ɾ��
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        var msgTypeIdList = await GetChildIdListAsync(ids);
        await _msgTypeRep.SoftDeleteRecursiveAsync(a => msgTypeIdList.Contains(a.Id));
    }
}
