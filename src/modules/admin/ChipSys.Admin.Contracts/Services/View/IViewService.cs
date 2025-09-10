using ChipSys.Admin.Services.View.Dto;

namespace ChipSys.Admin.Services.View;

/// <summary>
/// йсм╪╫с©з
/// </summary>
public interface IViewService
{
    Task<ViewGetOutput> GetAsync(long id);

    Task<List<ViewGetListOutput>> GetListAsync(ViewGetListInput input);

    Task<long> AddAsync(ViewAddInput input);

    Task UpdateAsync(ViewUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);

    Task SyncAsync(ViewSyncInput input);
}
