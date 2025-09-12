using ChipSys.Admin.Contracts.Services.ChipMgr.Supplier.Dto;
using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.Supplier;

/// <summary>
/// 上传清单项目服务接口
/// </summary>
public partial interface IUploadListItemService
{
    Task<UploadListItemGetOutput> GetAsync(long id);

    Task<PageOutput<UploadListItemGetPageOutput>> GetPageAsync(PageInput<UploadListItemGetPageInput> input);

    Task<List<UploadListItemGetListOutput>> GetListAsync(UploadListItemGetListInput input);

    Task<long> AddAsync(UploadListItemAddInput input);

    Task UpdateAsync(UploadListItemUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}
