using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Domain.DictType;
using ChipSys.Admin.Domain.Dict;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Repositories;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Services.DictType.Dto;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;

namespace ChipSys.Admin.Services.DictType;

/// <summary>
/// �����ֵ����ͷ���
/// </summary>
[Order(61)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class DictTypeService : BaseService, IDictTypeService, IDynamicApi
{
    private readonly AdminRepositoryBase<DictTypeEntity> _dictTypeRep;
    private readonly AdminRepositoryBase<DictEntity> _dictRep;
    private readonly AdminLocalizer _adminLocalizer;

    public DictTypeService(AdminRepositoryBase<DictTypeEntity> dictTypeRep, 
        AdminRepositoryBase<DictEntity> dictRep,
        AdminLocalizer adminLocalizer)
    {
        _dictTypeRep = dictTypeRep;
        _dictRep = dictRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// ��ѯ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DictTypeGetOutput> GetAsync(long id)
    {
        var result = await _dictTypeRep.GetAsync<DictTypeGetOutput>(id);
        return result;
    }

    /// <summary>
    /// ��ѯ��ҳ
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Obsolete($"��ʹ��{nameof(GetListAsync)}�������")]
    [HttpPost]
    public async Task<PageOutput<DictTypeGetPageOutput>> GetPageAsync(PageInput<DictTypeGetPageInput> input)
    {
        var key = input.Filter?.Name;

        var list = await _dictTypeRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
        .Count(out var total)
        .OrderByDescending(a => a.Sort)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<DictTypeGetPageOutput>();

        var data = new PageOutput<DictTypeGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// ��ѯ�б�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<DictTypeGetListOutput>> GetListAsync(DictTypeGetListInput input)
    {
        var key = input?.Name;

        var select = _dictTypeRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
        .OrderBy(a => a.ParentId);

        if (input.SortList != null && input.SortList.Count > 0)
        {
            input.SortList.ForEach(sort =>
            {
                select = select.OrderByPropertyNameIf(sort.Order.HasValue, sort.PropName, sort.IsAscending.Value);
            });
        }
        else
        {
            select = select.OrderBy(a => a.Sort);
        }

        return await select.ToListAsync<DictTypeGetListOutput>();
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(DictTypeAddInput input)
    {
        if (await _dictTypeRep.Select.AnyAsync(a => a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["�ֵ������Ѵ���"]);
        }

        if (input.Code.NotNull() && await _dictTypeRep.Select.AnyAsync(a => a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["�ֵ����ͱ����Ѵ���"]);
        }

        var entity = Mapper.Map<DictTypeEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _dictTypeRep.Select.MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _dictTypeRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// �޸�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(DictTypeUpdateInput input)
    {
        var entity = await _dictTypeRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["�ֵ����Ͳ�����"]);
        }

        if (input.Id == input.ParentId)
        {
            throw ResultOutput.Exception(_adminLocalizer["�ϼ��ֵ����Ͳ����Ǳ��ֵ�����"]);
        }

        if (await _dictTypeRep.Select.AnyAsync(a => a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["�ֵ������Ѵ���"]);
        }

        if (input.Code.NotNull() && await _dictTypeRep.Select.AnyAsync(a => a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["�ֵ����ͱ����Ѵ���"]);
        }

        var childIdList = await _dictTypeRep.Select
        .Where(a => a.Id == input.Id)
        .AsTreeCte()
        .ToListAsync(a => a.Id);
        if (childIdList.Contains(input.ParentId))
        {
            throw ResultOutput.Exception(_adminLocalizer["�ϼ��ֵ����Ͳ������¼��ֵ�����"]);
        }

        Mapper.Map(input, entity);
        await _dictTypeRep.UpdateAsync(entity);
    }

    /// <summary>
    /// ����ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        //ɾ���ֵ�����
        await _dictRep.DeleteAsync(a => a.DictTypeId == id);

        //ɾ�������ֵ�����
        await _dictTypeRep.DeleteAsync(a => a.Id == id);
    }

    /// <summary>
    /// ��������ɾ��
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        //ɾ���ֵ�����
        await _dictRep.DeleteAsync(a => ids.Contains(a.DictTypeId));

        //ɾ�������ֵ�����
        await _dictTypeRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        await _dictRep.SoftDeleteAsync(a => a.DictTypeId == id);
        await _dictTypeRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// ����ɾ��
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _dictRep.SoftDeleteAsync(a => ids.Contains(a.DictTypeId));
        await _dictTypeRep.SoftDeleteAsync(ids);
    }
}
