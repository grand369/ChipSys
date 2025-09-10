using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Services.Product.Dto;

namespace ChipSys.Admin.Services.Product;

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
