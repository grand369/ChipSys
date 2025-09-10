using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Services.Supplier.Dto;

namespace ChipSys.Admin.Services.Supplier;

/// <summary>
/// 联系人服务接口
/// </summary>
public partial interface IContactService
{
    Task<ContactGetOutput> GetAsync(long id);

    Task<PageOutput<ContactGetPageOutput>> GetPageAsync(PageInput<ContactGetPageInput> input);

    Task<List<ContactGetListOutput>> GetListAsync(ContactGetListInput input);

    Task<long> AddAsync(ContactAddInput input);

    Task UpdateAsync(ContactUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}
