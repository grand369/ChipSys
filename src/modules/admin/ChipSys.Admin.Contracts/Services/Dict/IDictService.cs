using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Services.Dict.Dto;
using ChipSys.Admin.Domain.Dict.Dto;

namespace ChipSys.Admin.Services.Dict;

/// <summary>
/// �����ֵ�ӿ�
/// </summary>
public partial interface IDictService
{
    Task<DictGetOutput> GetAsync(long id);

    Task<PageOutput<DictGetPageOutput>> GetPageAsync(PageInput<DictGetPageInput> input);

    Task<long> AddAsync(DictAddInput input);

    Task UpdateAsync(DictUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}
