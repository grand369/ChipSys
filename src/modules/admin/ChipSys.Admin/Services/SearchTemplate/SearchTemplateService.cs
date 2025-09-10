using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Resources;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Admin.Repositories;
using ChipSys.Admin.Services.SearchTemplate.Outputs;
using ChipSys.Admin.Services.SearchTemplate.Inputs;
using ChipSys.Admin.Domain.SearchTemplate;

namespace ChipSys.Admin.Services.SearchTemplate;

/// <summary>
/// 查询模板服务
/// </summary>
[Order(20)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class SearchTemplateService : BaseService, IDynamicApi
{
    private readonly AdminRepositoryBase<SearchTemplateEntity> _searchTemplateRep;
    private readonly AdminLocalizer _adminLocalizer;

    public SearchTemplateService(
        AdminRepositoryBase<SearchTemplateEntity> searchTemplateRep,
        AdminLocalizer adminLocalizer
    )
    {
        _searchTemplateRep = searchTemplateRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Login]
    public async Task<SearchTemplateGetUpdateOutput> GetAsync(long id)
    {
        var output = await _searchTemplateRep.Select
        .Where(a=>a.CreatedUserId == User.Id && a.Id == id)
        .ToOneAsync<SearchTemplateGetUpdateOutput>();

        return output;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="moduleId"></param>
    /// <returns></returns>
    [Login]
    public async Task<List<SearchTemplateGetListOutput>> GetListAsync(long moduleId)
    {
        var dataList = await _searchTemplateRep.Select
            .Where(a => a.CreatedUserId == User.Id && a.ModuleId == moduleId)
            .ToListAsync<SearchTemplateGetListOutput>();

        return dataList;
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Login]
    public async Task<long> SaveAsync(SearchTemplateSaveInput input)
    {
        var entity = await _searchTemplateRep.Where(a => a.CreatedUserId == User.Id && a.ModuleId == input.ModuleId && a.Name == input.Name).ToOneAsync();
        if (entity != null)
        {
            if (entity.Version != input.Version)
            {
                throw ResultOutput.Exception(_adminLocalizer["查询模板已被修改，请刷新后重试"]);
            }

            entity.Template = input.Template;
            await _searchTemplateRep.UpdateAsync(entity);
        }
        else
        {
            entity = Mapper.Map<SearchTemplateEntity>(input);
            await _searchTemplateRep.InsertAsync(entity);
        }

        return entity.Id;
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Login]
    public virtual async Task DeleteAsync(long id)
    {
        await _searchTemplateRep.DeleteAsync(a => a.CreatedUserId == User.Id && a.Id == id);
    }
}
