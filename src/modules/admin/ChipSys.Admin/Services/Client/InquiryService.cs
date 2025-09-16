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
/// 询价服务
/// </summary>
[Order(104)]
[DynamicApi(Area = "client")]
public class InquiryService : BaseService, IInquiryService, IDynamicApi
{
    private readonly AdminRepositoryBase<InquiryEntity> _inquiryRep;
    private readonly AdminRepositoryBase<QuoteEntity> _quoteRep;
    private readonly AdminLocalizer _adminLocalizer;

    public InquiryService(
        AdminRepositoryBase<InquiryEntity> inquiryRep,
        AdminRepositoryBase<QuoteEntity> quoteRep,
        AdminLocalizer adminLocalizer)
    {
        _inquiryRep = inquiryRep;
        _quoteRep = quoteRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 获取询价信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<InquiryGetOutput> GetAsync(long id)
    {
        var result = await _inquiryRep.GetAsync<InquiryGetOutput>(id);
        if (result == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["询价记录不存在"]);
        }

        // 检查权限：只能查看自己的询价
        if (result.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限查看此询价"]);
        }

        return result;
    }

    /// <summary>
    /// 获取询价分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<InquiryGetPageOutput>> GetPageAsync(PageInput<InquiryGetPageInput> input)
    {
        var title = input.Filter?.Title;
        var productName = input.Filter?.ProductName;
        var status = input.Filter?.Status;

        var list = await _inquiryRep.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .Where(a => a.MemberId == User.Id)
            .WhereIf(title.NotNull(), a => a.Title.Contains(title))
            .WhereIf(productName.NotNull(), a => a.ProductName.Contains(productName))
            .WhereIf(status.NotNull(), a => a.Status == status)
            .Count(out var total)
            .OrderByDescending(a => a.CreatedTime)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<InquiryGetPageOutput>();

        return new PageOutput<InquiryGetPageOutput>
        {
            List = list,
            Total = total
        };
    }

    /// <summary>
    /// 新增询价
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(InquiryAddInput input)
    {
        var entity = Mapper.Map<InquiryEntity>(input);
        entity.MemberId = User.Id;
        entity.Status = "Draft";
        entity.QuoteCount = 0;
        await _inquiryRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 更新询价
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(InquiryUpdateInput input)
    {
        var entity = await _inquiryRep.GetAsync(input.Id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["询价记录不存在"]);
        }

        // 检查权限：只能修改自己的询价
        if (entity.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限修改此询价"]);
        }

        // 只有草稿状态才能修改
        if (entity.Status != "Draft")
        {
            throw ResultOutput.Exception(_adminLocalizer["只有草稿状态的询价才能修改"]);
        }

        Mapper.Map(input, entity);
        await _inquiryRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除询价
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        var entity = await _inquiryRep.GetAsync(id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["询价记录不存在"]);
        }

        // 检查权限：只能删除自己的询价
        if (entity.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限删除此询价"]);
        }

        // 只有草稿状态才能删除
        if (entity.Status != "Draft")
        {
            throw ResultOutput.Exception(_adminLocalizer["只有草稿状态的询价才能删除"]);
        }

        await _inquiryRep.DeleteAsync(entity);
    }

    /// <summary>
    /// 发布询价
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task PublishAsync(long id)
    {
        var entity = await _inquiryRep.GetAsync(id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["询价记录不存在"]);
        }

        // 检查权限：只能发布自己的询价
        if (entity.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限发布此询价"]);
        }

        // 只有草稿状态才能发布
        if (entity.Status != "Draft")
        {
            throw ResultOutput.Exception(_adminLocalizer["只有草稿状态的询价才能发布"]);
        }

        entity.Status = "Published";
        await _inquiryRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 关闭询价
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task CloseAsync(long id)
    {
        var entity = await _inquiryRep.GetAsync(id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["询价记录不存在"]);
        }

        // 检查权限：只能关闭自己的询价
        if (entity.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限关闭此询价"]);
        }

        // 只有已发布状态才能关闭
        if (entity.Status != "Published")
        {
            throw ResultOutput.Exception(_adminLocalizer["只有已发布状态的询价才能关闭"]);
        }

        entity.Status = "Closed";
        await _inquiryRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 获取询价统计
    /// </summary>
    /// <returns></returns>
    public async Task<object> GetStatsAsync()
    {
        var memberId = User.Id;

        // 统计自己的询价
        var myInquiries = await _inquiryRep.Select
            .Where(a => a.MemberId == memberId)
            .CountAsync();

        var myInquiriesByStatus = new Dictionary<string, int>();
        var statuses = new[] { "Draft", "Published", "Closed", "Completed", "Cancelled" };

        foreach (var status in statuses)
        {
            var count = await _inquiryRep.Select
                .Where(a => a.MemberId == memberId && a.Status == status)
                .CountAsync();
            myInquiriesByStatus[status] = (int)count;
        }

        // 统计收到的报价总数
        var totalQuotes = await _quoteRep.Select
            .Where(a => a.Inquiry.MemberId == memberId)
            .CountAsync();

        return new
        {
            MyInquiries = myInquiries,
            MyInquiriesByStatus = myInquiriesByStatus,
            TotalQuotes = totalQuotes
        };
    }
}
