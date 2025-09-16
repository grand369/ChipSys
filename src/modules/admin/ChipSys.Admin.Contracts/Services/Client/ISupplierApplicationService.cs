using ChipSys.Admin.Core.Dto;
using ChipSys.Client.Contracts.Services.Client.Dto;

namespace ChipSys.Client.Contracts.Services.Client;

/// <summary>
/// 供应商申请服务接口
/// </summary>
public interface ISupplierApplicationService
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SupplierApplicationGetOutput> GetAsync(long id);

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageOutput<SupplierApplicationGetPageOutput>> GetPageAsync(SupplierApplicationGetPageInput input);

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<long> AddAsync(SupplierApplicationAddInput input);

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateAsync(SupplierApplicationUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(long id);

    /// <summary>
    /// 提交申请
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task SubmitAsync(long id);

    /// <summary>
    /// 撤销申请
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task CancelAsync(long id);

    /// <summary>
    /// 检查是否已有申请
    /// </summary>
    /// <returns></returns>
    Task<bool> HasPendingApplicationAsync();
}
