using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Services;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Repositories;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Client.Contracts.Domain.Client;
using ChipSys.Client.Contracts.Services.Client;
using ChipSys.Client.Contracts.Services.Client.Dto;
using FreeSql;
using System.Linq;
using System.Collections.Generic;

namespace ChipSys.Admin.Services.Client;

/// <summary>
/// 供求信息服务
/// </summary>
[Order(101)]
[DynamicApi(Area = "client")]
public class SupplyDemandService : BaseService, ISupplyDemandService, IDynamicApi
{
    private readonly AdminRepositoryBase<SupplyDemandEntity> _supplyDemandRep;
    private readonly AdminRepositoryBase<MemberLevelEntity> _memberLevelRep;
    private readonly AdminLocalizer _adminLocalizer;

    public SupplyDemandService(
        AdminRepositoryBase<SupplyDemandEntity> supplyDemandRep,
        AdminRepositoryBase<MemberLevelEntity> memberLevelRep,
        AdminLocalizer adminLocalizer)
    {
        _supplyDemandRep = supplyDemandRep;
        _memberLevelRep = memberLevelRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SupplyDemandGetOutput> GetAsync(long id)
    {
        var result = await _supplyDemandRep.GetAsync<SupplyDemandGetOutput>(id);
        if (result == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["供求信息不存在"]);
        }

        // 检查权限：只能查看自己的信息或已发布的信息
        if (result.MemberId != User.Id && result.Status != "Published")
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限查看此信息"]);
        }

        return result;
    }

    /// <summary>
    /// 查询分页（我的信息）
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<SupplyDemandGetPageOutput>> GetPageAsync(SupplyDemandGetPageInput input)
    {
        var infoType = input.Filter?.InfoType;
        var productName = input.Filter?.ProductName;
        var status = input.Filter?.Status;

        var list = await _supplyDemandRep.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .Where(a => a.MemberId == User.Id)
            .WhereIf(infoType.NotNull(), a => a.InfoType == infoType)
            .WhereIf(productName.NotNull(), a => a.ProductName.Contains(productName))
            .WhereIf(status.NotNull(), a => a.Status == status)
            .Count(out var total)
            .OrderByDescending(a => a.CreatedTime)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<SupplyDemandGetPageOutput>();

        return new PageOutput<SupplyDemandGetPageOutput>
        {
            List = list,
            Total = total
        };
    }

    /// <summary>
    /// 查询公开的供求信息（供其他用户查看）
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<SupplyDemandGetPageOutput>> GetPublicPageAsync(SupplyDemandGetPageInput input)
    {
        var infoType = input.Filter?.InfoType;
        var productName = input.Filter?.ProductName;

        var list = await _supplyDemandRep.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .Where(a => a.Status == "Published" && a.Enabled)
            .WhereIf(infoType.NotNull(), a => a.InfoType == infoType)
            .WhereIf(productName.NotNull(), a => a.ProductName.Contains(productName))
            .Count(out var total)
            .OrderByDescending(a => a.CreatedTime)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<SupplyDemandGetPageOutput>();

        return new PageOutput<SupplyDemandGetPageOutput>
        {
            List = list,
            Total = total
        };
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(SupplyDemandAddInput input)
    {
        // 检查会员权限
        await CheckMemberPermissionAsync(User.Id, input.InfoType);

        var entity = Mapper.Map<SupplyDemandEntity>(input);
        entity.MemberId = User.Id;
        entity.Status = "Draft"; // 默认为草稿状态
        await _supplyDemandRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(SupplyDemandUpdateInput input)
    {
        var entity = await _supplyDemandRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["供求信息不存在"]);
        }

        if (entity.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限修改此信息"]);
        }

        if (entity.Status == "Published")
        {
            throw ResultOutput.Exception(_adminLocalizer["已发布的信息不能修改"]);
        }

        Mapper.Map(input, entity);
        await _supplyDemandRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var entity = await _supplyDemandRep.GetAsync(id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["供求信息不存在"]);
        }

        if (entity.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限删除此信息"]);
        }

        await _supplyDemandRep.DeleteAsync(a => a.Id == id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        var entities = await _supplyDemandRep.Select
            .Where(a => ids.Contains(a.Id) && a.MemberId == User.Id)
            .ToListAsync();

        if (entities.Count != ids.Length)
        {
            throw ResultOutput.Exception(_adminLocalizer["部分信息无权限删除"]);
        }

        await _supplyDemandRep.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 发布
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task PublishAsync(long id)
    {
        var entity = await _supplyDemandRep.GetAsync(id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["供求信息不存在"]);
        }

        if (entity.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限发布此信息"]);
        }

        if (entity.Status == "Published")
        {
            throw ResultOutput.Exception(_adminLocalizer["信息已发布"]);
        }

        entity.Status = "Published";
        await _supplyDemandRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task CloseAsync(long id)
    {
        var entity = await _supplyDemandRep.GetAsync(id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["供求信息不存在"]);
        }

        if (entity.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限关闭此信息"]);
        }

        entity.Status = "Closed";
        await _supplyDemandRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 检查会员权限
    /// </summary>
    /// <param name="memberId"></param>
    /// <param name="infoType"></param>
    /// <returns></returns>
    private async Task CheckMemberPermissionAsync(long memberId, string infoType)
    {
        // 获取会员等级
        var memberLevel = await _memberLevelRep.Select
            .Where(a => a.MemberId == memberId && a.Enabled)
            .OrderByDescending(a => a.CreatedTime)
            .FirstAsync();

        if (memberLevel == null)
        {
            // 默认免费会员权限
            memberLevel = new MemberLevelEntity
            {
                MemberId = memberId,
                Level = "Free",
                CategoryLimit = 0,
                ProductDataLimit = 50,
                SupplierDataLimit = 50,
                ShowFullContactInfo = false
            };
        }

        // 根据会员等级和操作类型检查权限
        switch (infoType)
        {
            case "Inquiry":
                // 询价功能需要付费会员
                if (memberLevel.Level == "Free")
                {
                    throw ResultOutput.Exception(_adminLocalizer["询价功能需要付费会员权限"]);
                }
                break;
            case "Supply":
            case "Demand":
                // 供应和需求信息所有会员都可以发布
                break;
            default:
                throw ResultOutput.Exception(_adminLocalizer["不支持的信息类型"]);
        }
    }
}
