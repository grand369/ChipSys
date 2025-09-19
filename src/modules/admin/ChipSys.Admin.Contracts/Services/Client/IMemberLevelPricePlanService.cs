using ChipSys.Admin.Core.Dto;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChipSys.Client.Contracts.Services.Client.Dto;

namespace ChipSys.Client.Contracts.Services.Client;

/// <summary>
/// 会员等级价格方案服务接口
/// </summary>
public interface IMemberLevelPricePlanService
{
    /// <summary>
    /// 查询会员等级价格方案
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<MemberLevelPricePlanGetOutput> GetAsync(long id);

    /// <summary>
    /// 查询会员等级价格方案分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    Task<PageOutput<MemberLevelPricePlanGetPageOutput>> GetPageAsync(PageInput<MemberLevelPricePlanGetPageInput> input);

    /// <summary>
    /// 添加会员等级价格方案
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    Task<long> AddAsync(MemberLevelPricePlanAddInput input);

    /// <summary>
    /// 更新会员等级价格方案
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    Task UpdateAsync(MemberLevelPricePlanUpdateInput input);

    /// <summary>
    /// 删除会员等级价格方案
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    Task DeleteAsync(long id);

    /// <summary>
    /// 获取指定等级的价格方案列表
    /// </summary>
    /// <param name="memberLevelId"></param>
    /// <returns></returns>
    [HttpGet]
    Task<List<MemberLevelPricePlanGetOutput>> GetByMemberLevelIdAsync(long memberLevelId);

    /// <summary>
    /// 批量设置价格方案
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    Task BatchSetPricePlansAsync(BatchSetPricePlansInput input);
}

/// <summary>
/// 批量设置价格方案输入
/// </summary>
public class BatchSetPricePlansInput
{
    public long MemberLevelId { get; set; }
    public List<MemberLevelPricePlanAddInput> PricePlans { get; set; } = new();
}
