using ChipSys.Admin.Services.Org.Input;
using ChipSys.Admin.Services.Org.Output;

namespace ChipSys.Admin.Services.Org;

/// <summary>
/// 部门接口
/// </summary>
public partial interface IOrgService
{
    Task<OrgGetOutput> GetAsync(long id);

    Task<List<OrgGetListOutput>> GetListAsync(string key);

    Task<long> AddAsync(OrgAddInput input);

    Task UpdateAsync(OrgUpdateInput input);

    Task DeleteAsync(long id);

    Task SoftDeleteAsync(long id);
}
