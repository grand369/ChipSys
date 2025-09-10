using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Domain.Api;
using ChipSys.Admin.Services.Api.Dto;

namespace ChipSys.Admin.Services.Api;

/// <summary>
/// api½Ó¿Ú
/// </summary>
public interface IApiService
{
    Task<ApiGetOutput> GetAsync(long id);

    Task<List<ApiGetListOutput>> GetListAsync(string key);

    Task<PageOutput<ApiEntity>> GetPageAsync(PageInput<ApiGetPageInput> input);

    Task<long> AddAsync(ApiAddInput input);

    Task UpdateAsync(ApiUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);

    Task SyncAsync(ApiSyncInput input);
}
