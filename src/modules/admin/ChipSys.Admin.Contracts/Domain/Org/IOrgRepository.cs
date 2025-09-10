using ChipSys.Admin.Core.Repositories;

namespace ChipSys.Admin.Domain.Org;

public interface IOrgRepository : IRepositoryBase<OrgEntity>
{
    /// <summary>
    /// ��ñ����ź��¼�����Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<List<long>> GetChildIdListAsync(long id);
}
