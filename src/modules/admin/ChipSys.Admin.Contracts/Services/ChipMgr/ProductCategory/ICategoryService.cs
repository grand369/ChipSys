using ChipSys.Admin.Contracts.Services.ChipMgr.ProductCategory.Dto;
using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.ProductCategory;

/// <summary>
/// 产品分类服务接口
/// </summary>
public partial interface ICategoryService
{
    Task<CategoryGetOutput> GetAsync(long id);

    Task<PageOutput<CategoryGetPageOutput>> GetPageAsync(PageInput<CategoryGetPageInput> input);

    Task<List<CategoryGetListOutput>> GetListAsync(CategoryGetListInput input);

    Task<long> AddAsync(CategoryAddInput input);

    Task UpdateAsync(CategoryUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}
