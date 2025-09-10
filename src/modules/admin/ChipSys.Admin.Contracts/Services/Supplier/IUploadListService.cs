using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Services.Supplier.Dto;

namespace ChipSys.Admin.Services.Supplier;

/// <summary>
/// 上传清单服务接口
/// </summary>
public partial interface IUploadListService
{
    Task<UploadListGetOutput> GetAsync(long id);

    Task<PageOutput<UploadListGetPageOutput>> GetPageAsync(PageInput<UploadListGetPageInput> input);

    Task<List<UploadListGetListOutput>> GetListAsync(UploadListGetListInput input);

    Task<long> AddAsync(UploadListAddInput input);

    Task UpdateAsync(UploadListUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}
