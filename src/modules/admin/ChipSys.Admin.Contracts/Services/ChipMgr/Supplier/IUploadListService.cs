using ChipSys.Admin.Contracts.Services.ChipMgr.Supplier.Dto;
using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.Supplier;

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
