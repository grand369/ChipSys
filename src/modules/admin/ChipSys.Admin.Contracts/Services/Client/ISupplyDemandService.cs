using ChipSys.Admin.Core.Dto;
using ChipSys.Client.Contracts.Services.Client.Dto;

namespace ChipSys.Client.Contracts.Services.Client;

/// <summary>
/// 供求信息服务接口
/// </summary>
public interface ISupplyDemandService
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SupplyDemandGetOutput> GetAsync(long id);

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageOutput<SupplyDemandGetPageOutput>> GetPageAsync(SupplyDemandGetPageInput input);

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<long> AddAsync(SupplyDemandAddInput input);

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateAsync(SupplyDemandUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(long id);

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task BatchDeleteAsync(long[] ids);

    /// <summary>
    /// 发布
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task PublishAsync(long id);

    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task CloseAsync(long id);

    /// <summary>
    /// 查询公开的供求信息（供其他用户查看）
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageOutput<SupplyDemandGetPageOutput>> GetPublicPageAsync(SupplyDemandGetPageInput input);
}
