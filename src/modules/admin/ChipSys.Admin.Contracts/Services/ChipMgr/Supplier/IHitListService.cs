using ChipSys.Admin.Contracts.Services.ChipMgr.Supplier.Dto;
using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.Supplier;

/// <summary>
/// 命中清单服务接口
/// </summary>
public partial interface IHitListService
{
    Task<HitListGetOutput> GetAsync(long id);

    Task<PageOutput<HitListGetPageOutput>> GetPageAsync(PageInput<HitListGetPageInput> input);

    Task<List<HitListGetListOutput>> GetListAsync(HitListGetListInput input);

    Task<long> AddAsync(HitListAddInput input);

    Task UpdateAsync(HitListUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}
