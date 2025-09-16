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
/// 报价服务
/// </summary>
[Order(105)]
[DynamicApi(Area = "client")]
public class QuoteService : BaseService, IQuoteService, IDynamicApi
{
    private readonly AdminRepositoryBase<QuoteEntity> _quoteRep;
    private readonly AdminRepositoryBase<InquiryEntity> _inquiryRep;
    private readonly AdminLocalizer _adminLocalizer;

    public QuoteService(
        AdminRepositoryBase<QuoteEntity> quoteRep,
        AdminRepositoryBase<InquiryEntity> inquiryRep,
        AdminLocalizer adminLocalizer)
    {
        _quoteRep = quoteRep;
        _inquiryRep = inquiryRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 获取报价信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<QuoteGetOutput> GetAsync(long id)
    {
        var result = await _quoteRep.GetAsync<QuoteGetOutput>(id);
        if (result == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["报价记录不存在"]);
        }

        // 检查权限：只能查看自己询价的报价或自己提交的报价
        var quote = await _quoteRep.GetAsync(id);
        if (quote == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["报价记录不存在"]);
        }

        var inquiry = await _inquiryRep.GetAsync(quote.InquiryId);
        if (inquiry == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["询价记录不存在"]);
        }

        if (inquiry.MemberId != User.Id && quote.SupplierId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限查看此报价"]);
        }

        return result;
    }

    /// <summary>
    /// 获取报价分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<QuoteGetPageOutput>> GetPageAsync(PageInput<QuoteGetPageInput> input)
    {
        var inquiryId = input.Filter?.InquiryId;
        var supplierName = input.Filter?.SupplierName;
        var status = input.Filter?.Status;

        var list = await _quoteRep.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .Where(a => a.SupplierId == User.Id) // 只显示自己提交的报价
            .WhereIf(inquiryId.HasValue, a => a.InquiryId == inquiryId.Value)
            .WhereIf(supplierName.NotNull(), a => a.SupplierName.Contains(supplierName))
            .WhereIf(status.NotNull(), a => a.Status == status)
            .Count(out var total)
            .OrderByDescending(a => a.CreatedTime)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<QuoteGetPageOutput>();

        return new PageOutput<QuoteGetPageOutput>
        {
            List = list,
            Total = total
        };
    }

    /// <summary>
    /// 新增报价
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(QuoteAddInput input)
    {
        // 验证询价是否存在
        var inquiry = await _inquiryRep.GetAsync(input.InquiryId);
        if (inquiry == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["询价记录不存在"]);
        }

        if (inquiry.Status != "Published")
        {
            throw ResultOutput.Exception(_adminLocalizer["只能对已发布的询价进行报价"]);
        }

        // 检查是否已有报价
        var existing = await _quoteRep.Select
            .Where(a => a.InquiryId == input.InquiryId && a.SupplierId == User.Id)
            .FirstAsync();

        if (existing != null)
        {
            throw ResultOutput.Exception(_adminLocalizer["您已对此询价进行过报价"]);
        }

        var entity = Mapper.Map<QuoteEntity>(input);
        entity.SupplierId = User.Id;
        entity.Status = "Pending";
        entity.ValidUntil = DateTime.Now.AddDays(input.ValidDays);
        await _quoteRep.InsertAsync(entity);

        // 更新询价的报价数量
        inquiry.QuoteCount++;
        await _inquiryRep.UpdateAsync(inquiry);

        return entity.Id;
    }

    /// <summary>
    /// 更新报价
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(QuoteUpdateInput input)
    {
        var entity = await _quoteRep.GetAsync(input.Id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["报价记录不存在"]);
        }

        // 检查权限：只能修改自己的报价
        if (entity.SupplierId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限修改此报价"]);
        }

        // 只有待审核状态才能修改
        if (entity.Status != "Pending")
        {
            throw ResultOutput.Exception(_adminLocalizer["只有待审核状态的报价才能修改"]);
        }

        Mapper.Map(input, entity);
        entity.ValidUntil = DateTime.Now.AddDays(input.ValidDays);
        await _quoteRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除报价
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        var entity = await _quoteRep.GetAsync(id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["报价记录不存在"]);
        }

        // 检查权限：只能删除自己的报价
        if (entity.SupplierId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限删除此报价"]);
        }

        // 只有待审核状态才能删除
        if (entity.Status != "Pending")
        {
            throw ResultOutput.Exception(_adminLocalizer["只有待审核状态的报价才能删除"]);
        }

        await _quoteRep.DeleteAsync(entity);

        // 更新询价的报价数量
        var inquiry = await _inquiryRep.GetAsync(entity.InquiryId);
        if (inquiry != null)
        {
            inquiry.QuoteCount--;
            await _inquiryRep.UpdateAsync(inquiry);
        }
    }

    /// <summary>
    /// 接受报价
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task AcceptAsync(long id)
    {
        var entity = await _quoteRep.GetAsync(id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["报价记录不存在"]);
        }

        // 验证询价是否属于当前用户
        var inquiry = await _inquiryRep.GetAsync(entity.InquiryId);
        if (inquiry == null || inquiry.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限接受此报价"]);
        }

        // 只有已审核通过的报价才能被接受
        if (entity.Status != "Approved")
        {
            throw ResultOutput.Exception(_adminLocalizer["只有已审核通过的报价才能被接受"]);
        }

        entity.Status = "Accepted";
        entity.AcceptedTime = DateTime.Now;
        await _quoteRep.UpdateAsync(entity);

        // 关闭询价
        inquiry.Status = "Closed";
        await _inquiryRep.UpdateAsync(inquiry);
    }

    /// <summary>
    /// 拒绝报价
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task RejectAsync(long id)
    {
        var entity = await _quoteRep.GetAsync(id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["报价记录不存在"]);
        }

        // 验证询价是否属于当前用户
        var inquiry = await _inquiryRep.GetAsync(entity.InquiryId);
        if (inquiry == null || inquiry.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限拒绝此报价"]);
        }

        // 只有已审核通过的报价才能被拒绝
        if (entity.Status != "Approved")
        {
            throw ResultOutput.Exception(_adminLocalizer["只有已审核通过的报价才能被拒绝"]);
        }

        entity.Status = "Rejected";
        entity.RejectedTime = DateTime.Now;
        await _quoteRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 获取询价的报价列表
    /// </summary>
    /// <param name="inquiryId"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PageOutput<QuoteGetPageOutput>> GetInquiryQuotesAsync(long inquiryId, QuoteGetPageInput input)
    {
        // 验证询价是否存在且属于当前用户
        var inquiry = await _inquiryRep.GetAsync(inquiryId);
        if (inquiry == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["询价记录不存在"]);
        }

        if (inquiry.MemberId != User.Id)
        {
            throw ResultOutput.Exception(_adminLocalizer["无权限查看此询价的报价"]);
        }

        var status = input.Status;

        var list = await _quoteRep.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .Where(a => a.InquiryId == inquiryId)
            .WhereIf(status.NotNull(), a => a.Status == status)
            .Count(out var total)
            .OrderByDescending(a => a.CreatedTime)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<QuoteGetPageOutput>();

        return new PageOutput<QuoteGetPageOutput>
        {
            List = list,
            Total = total
        };
    }

    /// <summary>
    /// 获取报价统计
    /// </summary>
    /// <returns></returns>
    public async Task<object> GetStatsAsync()
    {
        var memberId = User.Id;

        // 统计自己提交的报价
        var myQuotes = await _quoteRep.Select
            .Where(a => a.SupplierId == memberId)
            .CountAsync();

        var myQuotesByStatus = new Dictionary<string, int>();
        var statuses = new[] { "Pending", "Approved", "Rejected", "Accepted" };

        foreach (var status in statuses)
        {
            var count = await _quoteRep.Select
                .Where(a => a.SupplierId == memberId && a.Status == status)
                .CountAsync();
            myQuotesByStatus[status] = (int)count;
        }

        return new
        {
            MyQuotes = myQuotes,
            MyQuotesByStatus = myQuotesByStatus
        };
    }
}
