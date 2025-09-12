using ChipSys.Admin.Contracts.Services.ChipMgr.Supplier.Dto;
using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.Supplier;

/// <summary>
/// 用户清单服务接口
/// </summary>
public partial interface IUserListService
{
    Task<UserListGetOutput> GetAsync(long id);

    Task<PageOutput<UserListGetPageOutput>> GetPageAsync(PageInput<UserListGetPageInput> input);

    Task<List<UserListGetListOutput>> GetListAsync(UserListGetListInput input);

    Task<long> AddAsync(UserListAddInput input);

    Task UpdateAsync(UserListUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}
