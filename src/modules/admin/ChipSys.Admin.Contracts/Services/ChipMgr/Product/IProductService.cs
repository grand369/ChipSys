using ChipSys.Admin.Contracts.Services.ChipMgr.Product.Dto;
using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.Product;

/// <summary>
/// 产品服务接口
/// </summary>
public partial interface IProductService
{
    Task<ProductGetOutput> GetAsync(long id);

    Task<PageOutput<ProductGetPageOutput>> GetPageAsync(PageInput<ProductGetPageInput> input);

    Task<List<ProductGetListOutput>> GetListAsync(ProductGetListInput input);

    Task<long> AddAsync(ProductAddInput input);

    Task UpdateAsync(ProductUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}
