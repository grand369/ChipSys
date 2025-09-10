using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Services.Product.Dto;

namespace ChipSys.Admin.Services.Product;

/// <summary>
/// 产品供应商关系服务接口
/// </summary>
public partial interface IProductSupplierService
{
    Task<ProductSupplierGetOutput> GetAsync(long id);

    Task<PageOutput<ProductSupplierGetPageOutput>> GetPageAsync(PageInput<ProductSupplierGetPageInput> input);

    Task<List<ProductSupplierGetListOutput>> GetListAsync(ProductSupplierGetListInput input);

    Task<long> AddAsync(ProductSupplierAddInput input);

    Task UpdateAsync(ProductSupplierUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}
