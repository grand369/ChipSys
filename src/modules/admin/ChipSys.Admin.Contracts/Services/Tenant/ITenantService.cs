using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Services.Tenant.Dto;

namespace ChipSys.Admin.Services.Tenant;

/// <summary>
/// ×â»§½Ó¿Ú
/// </summary>
public interface ITenantService
{
    Task<TenantGetOutput> GetAsync(long id);

    Task<PageOutput<TenantGetPageOutput>> GetPageAsync(PageInput<TenantGetPageInput> input);

    Task<long> AddAsync(TenantAddInput input);

    Task<long> RegAsync(TenantRegInput input);

    Task UpdateAsync(TenantUpdateInput input);

    Task DeleteAsync(long id);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}
