using ChipSys.Admin.Core.Dto;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChipSys.Client.Contracts.Services.Client.Dto;

namespace ChipSys.Client.Contracts.Services.Client;

/// <summary>
/// 报价服务接口
/// </summary>
public interface IQuoteService 
{
    /// <summary>
    /// 查询报价
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<QuoteGetOutput> GetAsync(long id);

    /// <summary>
    /// 查询报价分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    Task<PageOutput<QuoteGetPageOutput>> GetPageAsync(PageInput<QuoteGetPageInput> input);

    /// <summary>
    /// 新增报价
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    Task<long> AddAsync(QuoteAddInput input);

    /// <summary>
    /// 更新报价
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    Task UpdateAsync(QuoteUpdateInput input);

    /// <summary>
    /// 删除报价
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    Task DeleteAsync(long id);

    /// <summary>
    /// 接受报价
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    Task AcceptAsync(long id);

    /// <summary>
    /// 拒绝报价
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    Task RejectAsync(long id);

    /// <summary>
    /// 获取询价的报价列表
    /// </summary>
    /// <param name="inquiryId"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    Task<PageOutput<QuoteGetPageOutput>> GetInquiryQuotesAsync(long inquiryId, QuoteGetPageInput input);

    /// <summary>
    /// 获取报价统计
    /// </summary>
    /// <returns></returns>
    Task<object> GetStatsAsync();
}
