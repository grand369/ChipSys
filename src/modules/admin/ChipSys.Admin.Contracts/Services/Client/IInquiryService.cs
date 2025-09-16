using ChipSys.Admin.Core.Dto;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChipSys.Client.Contracts.Services.Client.Dto;

namespace ChipSys.Client.Contracts.Services.Client;

/// <summary>
/// 询价服务接口
/// </summary>
public interface IInquiryService 
{
    /// <summary>
    /// 查询询价
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<InquiryGetOutput> GetAsync(long id);

    /// <summary>
    /// 查询询价分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    Task<PageOutput<InquiryGetPageOutput>> GetPageAsync(PageInput<InquiryGetPageInput> input);

    /// <summary>
    /// 新增询价
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    Task<long> AddAsync(InquiryAddInput input);

    /// <summary>
    /// 更新询价
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    Task UpdateAsync(InquiryUpdateInput input);

    /// <summary>
    /// 删除询价
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    Task DeleteAsync(long id);

    /// <summary>
    /// 发布询价
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    Task PublishAsync(long id);

    /// <summary>
    /// 关闭询价
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    Task CloseAsync(long id);

    /// <summary>
    /// 获取询价统计
    /// </summary>
    /// <returns></returns>
    Task<object> GetStatsAsync();
}
