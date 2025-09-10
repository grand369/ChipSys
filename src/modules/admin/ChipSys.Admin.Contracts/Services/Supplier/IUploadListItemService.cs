using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Services.Supplier.Dto;

namespace ChipSys.Admin.Services.Supplier;

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
