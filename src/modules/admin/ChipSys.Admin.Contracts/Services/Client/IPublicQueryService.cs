using ChipSys.Admin.Core.Dto;

namespace ChipSys.Client.Contracts.Services.Client;

/// <summary>
/// 公开查询服务接口（供会员查看公开信息）
/// </summary>
public interface IPublicQueryService
{
    /// <summary>
    /// 查询公开的供应商列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageOutput<object>> GetPublicSuppliersAsync(object input);

    /// <summary>
    /// 查询公开的供应商详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<object> GetPublicSupplierAsync(long id);

    /// <summary>
    /// 查询公开的产品列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageOutput<object>> GetPublicProductsAsync(object input);

    /// <summary>
    /// 查询公开的产品详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<object> GetPublicProductAsync(long id);

    /// <summary>
    /// 查询供应商的产品列表
    /// </summary>
    /// <param name="supplierId"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageOutput<object>> GetSupplierProductsAsync(long supplierId, object input);

    /// <summary>
    /// 搜索供应商和产品
    /// </summary>
    /// <param name="keyword"></param>
    /// <param name="type">搜索类型：all-全部，supplier-供应商，product-产品</param>
    /// <returns></returns>
    Task<object> SearchAsync(string keyword, string type = "all");
}
