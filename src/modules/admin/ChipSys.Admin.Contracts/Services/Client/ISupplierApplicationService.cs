using ChipSys.Admin.Core.Dto;
using ChipSys.Client.Contracts.Services.Client.Dto;

namespace ChipSys.Client.Contracts.Services.Client;

/// <summary>
/// 供应商申请服务接口
/// </summary>
public interface ISupplierApplicationService
{
    /// <summary>
    /// 提交供应商申请
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<long> AddAsync(SupplierApplicationAddInput input);

    /// <summary>
    /// 获取申请详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SupplierApplicationGetOutput> GetAsync(long id);

    /// <summary>
    /// 获取申请分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageOutput<SupplierApplicationGetOutput>> GetPageAsync(PageInput<SupplierApplicationGetPageInput> input);

    /// <summary>
    /// 获取我的申请
    /// </summary>
    /// <returns></returns>
    Task<SupplierApplicationGetOutput?> GetMyApplicationAsync();
}