using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Services.Supplier.Dto;

namespace ChipSys.Admin.Services.Supplier;

/// <summary>
/// 供应商服务接口
/// </summary>
public partial interface ISupplierService
{
    Task<SupplierGetOutput> GetAsync(long id);

    Task<PageOutput<SupplierGetPageOutput>> GetPageAsync(PageInput<SupplierGetPageInput> input);

    Task<List<SupplierGetListOutput>> GetListAsync(SupplierGetListInput input);

    Task<long> AddAsync(SupplierAddInput input);

    Task UpdateAsync(SupplierUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}
