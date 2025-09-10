using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Domain.PrintTemplate;
using ChipSys.Admin.Resources;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Admin.Repositories;
using ChipSys.Admin.Services.PrintTemplate.Outputs;
using ChipSys.Admin.Services.PrintTemplate.Inputs;

namespace ChipSys.Admin.Services.PrintTemplate;

/// <summary>
/// ��ӡģ�����
/// </summary>
[Order(20)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class PrintTemplateService : BaseService, IDynamicApi
{
    private readonly AdminRepositoryBase<PrintTemplateEntity> _printTemplateRep;
    private readonly AdminLocalizer _adminLocalizer;

    public PrintTemplateService(
        AdminRepositoryBase<PrintTemplateEntity> printTemplateRep,
        AdminLocalizer adminLocalizer
    )
    {
        _printTemplateRep = printTemplateRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// ��ѯ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PrintTemplateGetOutput> GetAsync(long id)
    {
        var output = await _printTemplateRep.Select
        .WhereDynamic(id)
        .ToOneAsync<PrintTemplateGetOutput>();

        return output;
    }

    /// <summary>
    /// ��ѯ�޸�ģ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PrintTemplateGetUpdateTemplateOutput> GetUpdateTemplateAsync(long id)
    {
        var output = await _printTemplateRep.Select
        .WhereDynamic(id)
        .ToOneAsync<PrintTemplateGetUpdateTemplateOutput>();

        return output;
    }

    /// <summary>
    /// ��ѯ��ҳ
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<PrintTemplateGetPageOutput>> GetPageAsync(PageInput<PrintTemplateGetPageInput> input)
    {
        var list = await _printTemplateRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(input.Filter != null && input.Filter.Name.NotNull(), a => a.Name.Contains(input.Filter.Name))
        .WhereIf(input.Filter != null && input.Filter.Code.NotNull(), a => a.Code.Contains(input.Filter.Code))
        .Count(out var total)
        .OrderByDescending(true, a => a.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync(a => new PrintTemplateGetPageOutput { });

        var data = new PageOutput<PrintTemplateGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(PrintTemplateAddInput input)
    {
        if (await _printTemplateRep.Select.AnyAsync(a => a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["��ӡģ�������Ѵ���"]);
        }

        if (input.Code.NotNull() && await _printTemplateRep.Select.AnyAsync(a => a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["��ӡģ������Ѵ���"]);
        }

        var entity = Mapper.Map<PrintTemplateEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _printTemplateRep.Select.MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _printTemplateRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// �޸�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(PrintTemplateUpdateInput input)
    {
        if (await _printTemplateRep.Select.AnyAsync(a => a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["��ӡģ�������Ѵ���"]);
        }

        if (input.Code.NotNull() && await _printTemplateRep.Select.AnyAsync(a => a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["��ӡģ������Ѵ���"]);
        }

        var entity = await _printTemplateRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["��ӡģ�岻����"]);
        }

        if(entity.Version != input.Version)
        {
            throw ResultOutput.Exception(_adminLocalizer["��ӡģ���ѱ��޸ģ���ˢ�º�����"]);
        }

        Mapper.Map(input, entity);
        await _printTemplateRep.UpdateAsync(entity);
    }

    /// <summary>
    /// �޸�ģ��
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateTemplateAsync(PrintTemplateUpdateTemplateInput input)
    {
        var entity = await _printTemplateRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["��ӡģ�岻����"]);
        }

        if (entity.Version != input.Version)
        {
            throw ResultOutput.Exception(_adminLocalizer["��ӡģ���ѱ��޸ģ���ˢ�º�����"]);
        }

        entity.Template = input.Template;
        entity.PrintData = input.PrintData;
        await _printTemplateRep.UpdateAsync(entity);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetEnableAsync(PrintTemplateSetEnableInput input)
    {
        var printTemplateRep = _printTemplateRep;
        using var _ = printTemplateRep.DataFilter.DisableAll();

        var entity = await printTemplateRep.GetAsync(input.PrintTemplateId);
        entity.Enabled = input.Enabled;
        await printTemplateRep.UpdateAsync(entity);
    }

    /// <summary>
    /// ����ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        await _printTemplateRep.DeleteAsync(id);
    }

    /// <summary>
    /// ��������ɾ��
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        await _printTemplateRep.Where(a => ids.Contains(a.Id)).ToDelete().ExecuteAffrowsAsync();
    }

    /// <summary>
    /// ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        await _printTemplateRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// ����ɾ��
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _printTemplateRep.SoftDeleteAsync(a => ids.Contains(a.Id));
    }
}
