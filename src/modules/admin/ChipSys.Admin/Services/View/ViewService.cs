using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Domain.View;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Services.View.Dto;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;

namespace ChipSys.Admin.Services.View;

/// <summary>
/// ��ͼ����
/// </summary>
[Order(100)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class ViewService : BaseService, IViewService, IDynamicApi
{
    private readonly IViewRepository _viewRep;
    private readonly AdminLocalizer _adminLocalizer;

    public ViewService(IViewRepository viewRep, AdminLocalizer adminLocalizer)
    {
        _viewRep = viewRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// ��ѯ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ViewGetOutput> GetAsync(long id)
    {
        var result = await _viewRep.GetAsync<ViewGetOutput>(id);
        return result;
    }

    /// <summary>
    /// ��ѯ�б�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<ViewGetListOutput>> GetListAsync(ViewGetListInput input)
    {
        var platform = input?.Platform?.Trim();
        var name = input?.Name?.Trim();
        var label = input?.Label?.Trim();
        var path = input?.Path?.Trim();

        var select = _viewRep.Select;
        if (platform.NotNull())
        {
            Expression<Func<ViewEntity, bool>> where = null;
            where = where.And(a => a.Platform == platform);
            if(platform.ToLower() == AdminConsts.WebName)
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
            .WhereIf(name.NotNull(), a => a.Name.Contains(name))
            .WhereIf(label.NotNull(), a => a.Label.Contains(label))
            .WhereIf(path.NotNull(), a => a.Path.Contains(path))
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync<ViewGetListOutput>();

        return data;
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(ViewAddInput input)
    {
        if (await _viewRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Label == input.Label))
        {
            throw ResultOutput.Exception(_adminLocalizer["����ͼ�Ѵ���"]);
        }

        var entity = Mapper.Map<ViewEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _viewRep.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _viewRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// �޸�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(ViewUpdateInput input)
    {
        var entity = await _viewRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["��ͼ������"]);
        }

        if (input.Id == input.ParentId)
        {
            throw ResultOutput.Exception(_adminLocalizer["�ϼ���ͼ�����Ǳ���ͼ"]);
        }

        if (await _viewRep.Select.AnyAsync(a => a.Platform == input.Platform && a.ParentId == input.ParentId && a.Id != input.Id && a.Label == input.Label))
        {
            throw ResultOutput.Exception(_adminLocalizer["����ͼ�Ѵ���"]);
        }

        Mapper.Map(input, entity);
        await _viewRep.UpdateAsync(entity);
    }

    /// <summary>
    /// ����ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        await _viewRep.DeleteAsync(m => m.Id == id);
    }

    /// <summary>
    /// ��������ɾ��
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchDeleteAsync(long[] ids)
    {
        await _viewRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task SoftDeleteAsync(long id)
    {
        await _viewRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// ����ɾ��
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>

    public async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _viewRep.SoftDeleteAsync(ids);
    }

    /// <summary>
    /// ͬ��
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SyncAsync(ViewSyncInput input)
    {
        //��ѯ������ͼ
        var views = await _viewRep.Select.ToListAsync();
        var names = views.Select(a => a.Name).ToList();
        var paths = views.Select(a => a.Path).ToList();

        //path����
        foreach (var view in input.Views)
        {
            view.Path = view.Path?.Trim();
        }

        //��������
        {
            var inputViews = (from a in input.Views where !(paths.Contains(a.Path) || names.Contains(a.Name)) select a).ToList();
            if (inputViews.Count > 0)
            {
                var insertViews = Mapper.Map<List<ViewEntity>>(inputViews);
                foreach (var insertView in insertViews)
                {
                    if (insertView.Label.IsNull())
                    {
                        insertView.Label = insertView.Name;
                    }
                }
                insertViews = await _viewRep.InsertAsync(insertViews);
                views.AddRange(insertViews);
            }
        }

        //��������
        {
            var inputPaths = input.Views.Select(a => a.Path).ToList();
            var inputNames = input.Views.Select(a => a.Name).ToList();

            //�޸�
            var updateViews = (from a in views where inputPaths.Contains(a.Path) || inputNames.Contains(a.Name) select a).ToList();
            if (updateViews.Count > 0)
            {
                foreach (var view in updateViews)
                {
                    var inputView = input.Views.Where(a => a.Name == view.Name || a.Path == view.Path).FirstOrDefault();
                    if (view.Label.IsNull())
                    {
                        view.Label = inputView.Label ?? inputView.Name;
                    }
                    if (view.Description.IsNull())
                    {
                        view.Description = inputView.Description;
                    }
                    view.Name = inputView.Name;
                    view.Path = inputView.Path;
                    view.Enabled = true;
                }
            }

            //����
            var disabledViews = (from a in views where (a.Path.NotNull() || a.Name.NotNull()) && (!inputPaths.Contains(a.Path) || !inputNames.Contains(a.Name)) select a).ToList();
            if (disabledViews.Count > 0)
            {
                foreach (var view in disabledViews)
                {
                    view.Enabled = false;
                }
            }

            updateViews.AddRange(disabledViews);
            await _viewRep.UpdateDiy.SetSource(updateViews)
            .UpdateColumns(a => new { a.Label, a.Name, a.Path, a.Enabled, a.Description })
            .ExecuteAffrowsAsync();
        }
    }
}
